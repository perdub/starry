using System.Globalization;
using System.Net;
using System.Text.Json.Nodes;
using Starry.Core.Configuration;

namespace Starry.Infrastructure.Configuration;

/// <summary>
/// Validates cluster configuration schema, references, node constraints, and secrets syntax.
/// </summary>
public static class ConfigValidator
{
    public static ConfigValidationResult Validate(ConfigTree tree)
    {
        ArgumentNullException.ThrowIfNull(tree);

        var issues = new List<ValidationIssue>();

        ValidateNodes(tree, issues);
        ValidateServices(tree, issues);
        ValidateSecrets(tree, issues);

        var hasErrors = issues.Any(i => !i.IsWarning);
        return new ConfigValidationResult(!hasErrors, issues);
    }

    private static void ValidateNodes(ConfigTree tree, List<ValidationIssue> issues)
    {
        if (tree.Root.TryGetPropertyValue("nodes", out var nodesNode) && nodesNode is JsonObject nodesObj)
        {
            foreach (var (nodeName, nodeVal) in nodesObj)
            {
                var nodePath = $"nodes.{nodeName}";
                tree.SourceMap.TryGetValue(nodePath, out var sourceEntry);

                if (nodeVal is not JsonObject nodeObj)
                {
                    issues.Add(new ValidationIssue(nodePath, $"Node '{nodeName}' must be an object.", sourceEntry?.FilePath));
                    continue;
                }

                if (!nodeObj.TryGetPropertyValue("host", out var hostVal) || hostVal == null || string.IsNullOrWhiteSpace(hostVal.ToString()))
                {
                    issues.Add(new ValidationIssue($"{nodePath}.host", $"Node '{nodeName}' is missing required 'host' address.", sourceEntry?.FilePath));
                }

                if (nodeObj.TryGetPropertyValue("port", out var portVal) && portVal != null &&
                    (!int.TryParse(portVal.ToString(), CultureInfo.InvariantCulture, out var port) || port < 1 || port > 65535))
                {
                    issues.Add(new ValidationIssue($"{nodePath}.port", $"Node '{nodeName}' has invalid SSH port: {portVal}.", sourceEntry?.FilePath));
                }

                if (nodeObj.TryGetPropertyValue("vpn_ip", out var vpnIpVal) && vpnIpVal != null)
                {
                    var ipStr = vpnIpVal.ToString();
                    if (!IPAddress.TryParse(ipStr, out _))
                    {
                        issues.Add(new ValidationIssue($"{nodePath}.vpn_ip", $"Node '{nodeName}' has invalid VPN IP address: '{ipStr}'.", sourceEntry?.FilePath));
                    }
                }
            }
        }
    }

    private static void ValidateServices(ConfigTree tree, List<ValidationIssue> issues)
    {
        HashSet<string>? knownNodes = null;
        if (tree.Root.TryGetPropertyValue("nodes", out var nodesNode) && nodesNode is JsonObject nodesObj)
        {
            knownNodes = nodesObj.Select(kv => kv.Key).ToHashSet(StringComparer.OrdinalIgnoreCase);
        }

        if (tree.Root.TryGetPropertyValue("services", out var servicesNode) && servicesNode is JsonObject servicesObj)
        {
            foreach (var (svcName, svcVal) in servicesObj)
            {
                var svcPath = $"services.{svcName}";
                tree.SourceMap.TryGetValue(svcPath, out var sourceEntry);

                if (svcVal is not JsonObject svcObj)
                {
                    issues.Add(new ValidationIssue(svcPath, $"Service '{svcName}' must be an object.", sourceEntry?.FilePath));
                    continue;
                }

                if (!svcObj.TryGetPropertyValue("image", out var imageVal) || imageVal == null || string.IsNullOrWhiteSpace(imageVal.ToString()))
                {
                    issues.Add(new ValidationIssue($"{svcPath}.image", $"Service '{svcName}' is missing required 'image'.", sourceEntry?.FilePath));
                }

                if (svcObj.TryGetPropertyValue("priority", out var priorityVal) && priorityVal != null &&
                    (!int.TryParse(priorityVal.ToString(), CultureInfo.InvariantCulture, out var priority) || priority < 0 || priority > 100))
                {
                    issues.Add(new ValidationIssue($"{svcPath}.priority", $"Service '{svcName}' priority must be an integer between 0 and 100.", sourceEntry?.FilePath));
                }

                if (svcObj.TryGetPropertyValue("node", out var targetNodeVal) && targetNodeVal != null && knownNodes != null)
                {
                    var targetNode = targetNodeVal.ToString();
                    if (!knownNodes.Contains(targetNode))
                    {
                        issues.Add(new ValidationIssue($"{svcPath}.node", $"Service '{svcName}' references unknown node '{targetNode}'.", sourceEntry?.FilePath));
                    }
                }
            }
        }
    }

    private static void ValidateSecrets(ConfigTree tree, List<ValidationIssue> issues)
    {
        ScanSecretsRecursive(tree.Root, string.Empty, tree.SourceMap, issues);
    }

    private static void ScanSecretsRecursive(
        JsonNode? current,
        string currentPath,
        IReadOnlyDictionary<string, SourceMapEntry> sourceMap,
        List<ValidationIssue> issues)
    {
        if (current is null)
        {
            return;
        }

        if (current is JsonObject obj)
        {
            foreach (var (key, val) in obj)
            {
                var path = string.IsNullOrEmpty(currentPath) ? key : $"{currentPath}.{key}";
                ScanSecretsRecursive(val, path, sourceMap, issues);
            }
            return;
        }

        if (current is JsonArray arr)
        {
            for (var i = 0; i < arr.Count; i++)
            {
                ScanSecretsRecursive(arr[i], $"{currentPath}[{i}]", sourceMap, issues);
            }
            return;
        }

        if (current is JsonValue valNode)
        {
            var strVal = valNode.ToString();
            sourceMap.TryGetValue(currentPath, out var sourceEntry);

            if (strVal.StartsWith("PLAIN[", StringComparison.Ordinal) && strVal.EndsWith(']'))
            {
                issues.Add(new ValidationIssue(
                    currentPath,
                    "Open unencrypted secret detected: PLAIN[...]. Use '--encrypt-open-secrets' to encrypt.",
                    sourceEntry?.FilePath,
                    IsWarning: true));
            }
            else if (strVal.StartsWith("ENC[", StringComparison.Ordinal) &&
                     (!strVal.EndsWith(']') || !strVal.StartsWith("ENC[AES256_GCM:v1:", StringComparison.Ordinal)))
            {
                issues.Add(new ValidationIssue(
                    currentPath,
                    "Invalid encrypted secret format. Expected ENC[AES256_GCM:v1:<nonce>:<cipher>:<tag>].",
                    sourceEntry?.FilePath));
            }
        }
    }
}
