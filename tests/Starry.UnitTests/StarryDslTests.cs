using Starry.Dsl;
using Xunit;

namespace Starry.UnitTests;

public class StarryDslTests
{
    [Fact]
    public void ConfigAst_ParsesYamlJsonTomlCorrectly()
    {
        const string yaml = """
            cluster_name: kessoku-v3
            nodes:
              germany:
                host: 142.132.10.20
            """;

        var node = ConfigAst.parseYaml(yaml, Microsoft.FSharp.Core.FSharpOption<string>.Some("starry.yaml"));
        Assert.True(node.IsObjectNode);

        var jsonNode = ConfigAst.toJsonNode(node);
        Assert.Equal("kessoku-v3", jsonNode["cluster_name"]?.ToString());
        Assert.Equal("142.132.10.20", jsonNode["nodes"]?["germany"]?["host"]?.ToString());
    }

    [Fact]
    public void GraphResolver_WholeSubtreeSubstitution_ReplacesReference()
    {
        const string yaml = """
            presets:
              db_env:
                POSTGRES_DB: starry
                POSTGRES_USER: admin
            services:
              postgres:
                env: $presets.db_env
            """;

        var root = ConfigAst.parseYaml(yaml, Microsoft.FSharp.Core.FSharpOption<string>.None);
        var resolvedResult = GraphResolver.resolve(root, Microsoft.FSharp.Core.FSharpOption<Microsoft.FSharp.Core.FSharpFunc<string, string>>.None);

        Assert.True(resolvedResult.IsOk);
        var resolvedJson = ConfigAst.toJsonNode(resolvedResult.ResultValue);

        Assert.Equal("starry", resolvedJson["services"]?["postgres"]?["env"]?["POSTGRES_DB"]?.ToString());
        Assert.Equal("admin", resolvedJson["services"]?["postgres"]?["env"]?["POSTGRES_USER"]?.ToString());
    }

    [Fact]
    public void GraphResolver_Inheritance_MergesAndOverrides()
    {
        const string yaml = """
            presets:
              base_service:
                image: base:latest
                priority: 50
                restart: always
            services:
              api:
                $: $presets.base_service
                image: custom-api:v1
                priority: 30
            """;

        var root = ConfigAst.parseYaml(yaml, Microsoft.FSharp.Core.FSharpOption<string>.None);
        var resolvedResult = GraphResolver.resolve(root, Microsoft.FSharp.Core.FSharpOption<Microsoft.FSharp.Core.FSharpFunc<string, string>>.None);

        Assert.True(resolvedResult.IsOk);
        var resolvedJson = ConfigAst.toJsonNode(resolvedResult.ResultValue);

        // Explicit properties override inherited ones
        Assert.Equal("custom-api:v1", resolvedJson["services"]?["api"]?["image"]?.ToString());
        Assert.Equal("30", resolvedJson["services"]?["api"]?["priority"]?.ToString());
        // Inherited non-overridden properties are preserved
        Assert.Equal("always", resolvedJson["services"]?["api"]?["restart"]?.ToString());
    }

    [Fact]
    public void GraphResolver_StringInterpolation_SubstitutesValues()
    {
        const string yaml = """
            nodes:
              germany:
                host: 142.132.10.20
            services:
              api:
                env:
                  REMOTE_HOST: "connect to $nodes.germany.host please"
            """;

        var root = ConfigAst.parseYaml(yaml, Microsoft.FSharp.Core.FSharpOption<string>.None);
        var resolvedResult = GraphResolver.resolve(root, Microsoft.FSharp.Core.FSharpOption<Microsoft.FSharp.Core.FSharpFunc<string, string>>.None);

        Assert.True(resolvedResult.IsOk);
        var resolvedJson = ConfigAst.toJsonNode(resolvedResult.ResultValue);

        Assert.Equal("connect to 142.132.10.20 please", resolvedJson["services"]?["api"]?["env"]?["REMOTE_HOST"]?.ToString());
    }

    [Fact]
    public void GraphResolver_DetectsCircularReferences()
    {
        const string yaml = """
            a: $b
            b: $c
            c: $a
            """;

        var root = ConfigAst.parseYaml(yaml, Microsoft.FSharp.Core.FSharpOption<string>.None);
        var resolvedResult = GraphResolver.resolve(root, Microsoft.FSharp.Core.FSharpOption<Microsoft.FSharp.Core.FSharpFunc<string, string>>.None);

        Assert.True(resolvedResult.IsError);
        Assert.True(resolvedResult.ErrorValue.IsCircularReference);
    }

    [Fact]
    public void Generators_ProducesWireGuardConfigWithFd67Prefix()
    {
        var hostSpec = new Generators.WireGuardHostSpec(
            nodeName: "germany",
            hostId: 1,
            privateKey: "priv1",
            publicKey: "pub1",
            listenPort: 51820,
            endpoint: Microsoft.FSharp.Core.FSharpOption<string>.Some("142.132.10.20"),
            isPublic: true,
            peers:
            [
                new Generators.WireGuardPeerSpec(
                    nodeName: "finland",
                    hostId: 2,
                    publicKey: "pub2",
                    endpoint: Microsoft.FSharp.Core.FSharpOption<string>.Some("65.21.10.30"),
                    listenPort: 51820,
                    isPublic: true)
            ]);

        var wgConf = Generators.generateWireGuardConfig(hostSpec);

        Assert.Contains("Address = fd67:b00b:0001::1/32", wgConf, StringComparison.Ordinal);
        Assert.Contains("AllowedIPs = fd67:b00b:0002::/48", wgConf, StringComparison.Ordinal);
        Assert.Contains("Endpoint = 65.21.10.30:51820", wgConf, StringComparison.Ordinal);
    }

    [Fact]
    public void Generators_ProducesNginxConfigWithSniAndHttp()
    {
        var routes = new List<Generators.NginxRoute>
        {
            Generators.NginxRoute.NewSniPassthrough("vpn.bocchi.rocks", "xray:8443", true),
            Generators.NginxRoute.NewHttpReverseProxy("shop.bocchi.rocks", "fd67:b00b:0001:0002::1", 3000, Microsoft.FSharp.Core.FSharpOption<string>.None, Microsoft.FSharp.Core.FSharpOption<string>.None)
        };

        var nginxConf = Generators.generateNginxConfig(1, Microsoft.FSharp.Collections.ListModule.OfSeq(routes));

        Assert.Contains("stream {", nginxConf, StringComparison.Ordinal);
        Assert.Contains("ssl_preread on;", nginxConf, StringComparison.Ordinal);
        Assert.Contains("\"vpn.bocchi.rocks\" xray:8443;", nginxConf, StringComparison.Ordinal);
        Assert.Contains("http {", nginxConf, StringComparison.Ordinal);
        Assert.Contains("proxy_pass http://[fd67:b00b:0001:0002::1]:3000;", nginxConf, StringComparison.Ordinal);
    }
}
