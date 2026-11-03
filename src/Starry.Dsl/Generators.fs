namespace Starry.Dsl

open System
open System.Text

module Generators =

    type WireGuardPeerSpec = {
        NodeName: string
        HostId: int
        PublicKey: string
        Endpoint: string option
        ListenPort: int
        IsPublic: bool
    }

    type WireGuardHostSpec = {
        NodeName: string
        HostId: int
        PrivateKey: string
        PublicKey: string
        ListenPort: int
        Endpoint: string option
        IsPublic: bool
        Peers: WireGuardPeerSpec list
    }

    type NftablePortRule = {
        Name: string
        Protocol: string // "tcp", "udp", "both"
        HostPort: int
        TargetIpv6: string
        TargetPort: int
        Bidirectional: bool
        AddressFamily: string // "ipv4", "ipv6", "any"
    }

    type MeshSubnetPair = {
        SourceSubnet: string // e.g. "fd67:b00b:0001:0002::/64"
        TargetSubnet: string // e.g. "fd67:b00b:0002:0001::/64"
    }

    type NginxRoute =
        | SniPassthrough of serverName: string * targetEndpoint: string * proxyProtocol: bool
        | HttpReverseProxy of serverName: string * targetIpv6: string * targetPort: int * certPath: string option * keyPath: string option

    // 1. Generate WireGuard Config (starry0.conf) for a host
    let generateWireGuardConfig (spec: WireGuardHostSpec) : string =
        let sb = StringBuilder()
        let hostHex = sprintf "%04x" spec.HostId

        sb.AppendLine("# Managed by Starry GitOps Orchestrator - fd67:b00b::/32 Mesh") |> ignore
        sb.AppendLine("# Interface: starry0") |> ignore
        sb.AppendLine("[Interface]") |> ignore
        sb.AppendLine(sprintf "Address = fd67:b00b:%s::1/32" hostHex) |> ignore
        sb.AppendLine(sprintf "ListenPort = %d" spec.ListenPort) |> ignore
        sb.AppendLine(sprintf "PrivateKey = %s" spec.PrivateKey) |> ignore
        sb.AppendLine() |> ignore

        for peer in spec.Peers do
            let peerHex = sprintf "%04x" peer.HostId
            sb.AppendLine(sprintf "# Peer: %s (Host ID: %d)" peer.NodeName peer.HostId) |> ignore
            sb.AppendLine("[Peer]") |> ignore
            sb.AppendLine(sprintf "PublicKey = %s" peer.PublicKey) |> ignore
            // Each host routes the whole /48 statically so container changes never reload WG!
            sb.AppendLine(sprintf "AllowedIPs = fd67:b00b:%s::/48" peerHex) |> ignore

            match peer.Endpoint with
            | Some ep when peer.IsPublic && not (String.IsNullOrWhiteSpace(ep)) ->
                let formattedEp =
                    if ep.Contains(":") then ep
                    else sprintf "%s:%d" ep peer.ListenPort
                sb.AppendLine(sprintf "Endpoint = %s" formattedEp) |> ignore
            | _ -> ()

            sb.AppendLine("PersistentKeepalive = 25") |> ignore
            sb.AppendLine() |> ignore

        sb.ToString()

    // 2. Generate Nftables Ruleset (table inet starry)
    let generateNftablesRuleset
        (sshPort: int)
        (wireGuardPort: int)
        (allowedPublicPorts: int list)
        (portRules: NftablePortRule list)
        (allowedSubnetPairs: MeshSubnetPair list)
        (wanInterface: string option) : string =

        let sb = StringBuilder()
        let wan = wanInterface |> Option.defaultValue "eth0"

        sb.AppendLine("#!/usr/sbin/nft -f") |> ignore
        sb.AppendLine("# Managed by Starry GitOps Orchestrator - Table: inet starry") |> ignore
        sb.AppendLine("table inet starry") |> ignore
        sb.AppendLine("delete table inet starry") |> ignore
        sb.AppendLine("table inet starry {") |> ignore

        // Network isolation set for inter-container communication
        sb.AppendLine("    set allowed_mesh_pairs {") |> ignore
        sb.AppendLine("        type ipv6_addr . ipv6_addr") |> ignore
        sb.AppendLine("        flags interval") |> ignore
        if allowedSubnetPairs.Length > 0 then
            sb.AppendLine("        elements = {") |> ignore
            for pair in allowedSubnetPairs do
                sb.AppendLine(sprintf "            %s . %s," pair.SourceSubnet pair.TargetSubnet) |> ignore
            sb.AppendLine("        }") |> ignore
        sb.AppendLine("    }") |> ignore
        sb.AppendLine() |> ignore

        // Chain input
        sb.AppendLine("    chain input {") |> ignore
        sb.AppendLine("        type filter hook input priority filter; policy drop;") |> ignore
        sb.AppendLine("        ct state established,related accept") |> ignore
        sb.AppendLine("        iifname \"lo\" accept") |> ignore
        sb.AppendLine("        iifname \"starry0\" accept") |> ignore
        sb.AppendLine(sprintf "        tcp dport %d accept" sshPort) |> ignore
        sb.AppendLine(sprintf "        udp dport %d accept" wireGuardPort) |> ignore

        for p in allowedPublicPorts do
            sb.AppendLine(sprintf "        tcp dport %d accept" p) |> ignore
            sb.AppendLine(sprintf "        udp dport %d accept" p) |> ignore

        sb.AppendLine("        icmp type echo-request accept") |> ignore
        sb.AppendLine("        icmpv6 type { echo-request, nd-neighbor-solicit, nd-neighbor-advert, nd-router-advert } accept") |> ignore
        sb.AppendLine("    }") |> ignore
        sb.AppendLine() |> ignore

        // Chain forward
        sb.AppendLine("    chain forward {") |> ignore
        sb.AppendLine("        type filter hook forward priority filter; policy drop;") |> ignore
        sb.AppendLine("        ct state established,related accept") |> ignore
        sb.AppendLine("        iifname \"starry0\" oifname \"starry0\" ip6 saddr . ip6 daddr @allowed_mesh_pairs accept") |> ignore
        sb.AppendLine("        iifname \"starry0\" oifname \"starry0\" accept") |> ignore
        sb.AppendLine("        iifname \"starry0\" accept") |> ignore
        sb.AppendLine("        oifname \"starry0\" accept") |> ignore
        sb.AppendLine("    }") |> ignore
        sb.AppendLine() |> ignore

        // Chain prerouting (DNAT)
        sb.AppendLine("    chain prerouting {") |> ignore
        sb.AppendLine("        type nat hook prerouting priority dstnat; policy accept;") |> ignore
        for rule in portRules do
            let proto = rule.Protocol.ToLowerInvariant()
            let protoList = if proto = "both" then [ "tcp"; "udp" ] else [ proto ]
            for p in protoList do
                match rule.AddressFamily.ToLowerInvariant() with
                | "ipv4" ->
                    sb.AppendLine(sprintf "        meta nfproto ipv4 %s dport %d dnat to [%s]:%d" p rule.HostPort rule.TargetIpv6 rule.TargetPort) |> ignore
                | "ipv6" ->
                    sb.AppendLine(sprintf "        meta nfproto ipv6 %s dport %d dnat to [%s]:%d" p rule.HostPort rule.TargetIpv6 rule.TargetPort) |> ignore
                | _ ->
                    sb.AppendLine(sprintf "        %s dport %d dnat to [%s]:%d" p rule.HostPort rule.TargetIpv6 rule.TargetPort) |> ignore
        sb.AppendLine("    }") |> ignore
        sb.AppendLine() |> ignore

        // Chain postrouting (SNAT / Masquerade)
        sb.AppendLine("    chain postrouting {") |> ignore
        sb.AppendLine("        type nat hook postrouting priority srcnat; policy accept;") |> ignore
        sb.AppendLine("        oifname \"starry0\" masquerade") |> ignore
        sb.AppendLine(sprintf "        oifname \"%s\" masquerade" wan) |> ignore
        sb.AppendLine("    }") |> ignore
        sb.AppendLine("}") |> ignore

        sb.ToString()

    // 3. Generate Nginx Managed ($ingress) Config
    let generateNginxConfig (ingressHostId: int) (routes: NginxRoute list) : string =
        let sb = StringBuilder()
        let ingressIp = sprintf "fd67:b00b:%04x:ffff::1" ingressHostId

        sb.AppendLine("# Managed by Starry GitOps Orchestrator - $ingress SNI & HTTP Proxy") |> ignore
        sb.AppendLine("user nginx;") |> ignore
        sb.AppendLine("worker_processes auto;") |> ignore
        sb.AppendLine("pid /run/nginx.pid;") |> ignore
        sb.AppendLine() |> ignore
        sb.AppendLine("events {") |> ignore
        sb.AppendLine("    worker_connections 4096;") |> ignore
        sb.AppendLine("}") |> ignore
        sb.AppendLine() |> ignore

        // Stream block for L4 SNI routing
        sb.AppendLine("stream {") |> ignore
        sb.AppendLine("    map $ssl_preread_server_name $sni_upstream {") |> ignore
        for r in routes do
            match r with
            | SniPassthrough (name, target, _) ->
                sb.AppendLine(sprintf "        \"%s\" %s;" name target) |> ignore
            | HttpReverseProxy (name, _, _, _, _) ->
                // Route to internal HTTP block for TLS termination
                sb.AppendLine(sprintf "        \"%s\" 127.0.0.1:8443;" name) |> ignore
        sb.AppendLine("        default 127.0.0.1:8443;") |> ignore
        sb.AppendLine("    }") |> ignore
        sb.AppendLine() |> ignore
        sb.AppendLine("    server {") |> ignore
        sb.AppendLine("        listen 443;") |> ignore
        sb.AppendLine(sprintf "        listen [%s]:443;" ingressIp) |> ignore
        sb.AppendLine("        ssl_preread on;") |> ignore
        sb.AppendLine("        proxy_pass $sni_upstream;") |> ignore
        sb.AppendLine("    }") |> ignore
        sb.AppendLine("}") |> ignore
        sb.AppendLine() |> ignore

        // HTTP block for L7 Reverse Proxying
        sb.AppendLine("http {") |> ignore
        sb.AppendLine("    include /etc/nginx/mime.types;") |> ignore
        sb.AppendLine("    default_type application/octet-stream;") |> ignore
        sb.AppendLine() |> ignore

        for r in routes do
            match r with
            | HttpReverseProxy (name, targetIp, targetPort, certOpt, keyOpt) ->
                sb.AppendLine("    server {") |> ignore
                sb.AppendLine("        listen 127.0.0.1:8443 ssl http2;") |> ignore
                sb.AppendLine(sprintf "        server_name %s;" name) |> ignore
                match certOpt, keyOpt with
                | Some cert, Some key ->
                    sb.AppendLine(sprintf "        ssl_certificate %s;" cert) |> ignore
                    sb.AppendLine(sprintf "        ssl_certificate_key %s;" key) |> ignore
                | _ -> ()
                sb.AppendLine("        location / {") |> ignore
                sb.AppendLine(sprintf "            proxy_pass http://[%s]:%d;" targetIp targetPort) |> ignore
                sb.AppendLine("            proxy_set_header Host $host;") |> ignore
                sb.AppendLine("            proxy_set_header X-Real-IP $remote_addr;") |> ignore
                sb.AppendLine("            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;") |> ignore
                sb.AppendLine("            proxy_set_header X-Forwarded-Proto $scheme;") |> ignore
                sb.AppendLine("            proxy_http_version 1.1;") |> ignore
                sb.AppendLine("            proxy_set_header Upgrade $http_upgrade;") |> ignore
                sb.AppendLine("            proxy_set_header Connection \"upgrade\";") |> ignore
                sb.AppendLine("        }") |> ignore
                sb.AppendLine("    }") |> ignore
                sb.AppendLine() |> ignore
            | _ -> ()

        sb.AppendLine("}") |> ignore
        sb.ToString()
