namespace Starry.Dsl

open System
open System.IO
open System.Text
open System.Text.Json
open System.Text.Json.Nodes
open YamlDotNet.Serialization
open Tomlyn
open Tomlyn.Model

type ConfigNode =
    | ObjectNode of Map<string, ConfigNode> * sourceFile: string option
    | ArrayNode of ConfigNode list * sourceFile: string option
    | StringNode of string * sourceFile: string option
    | IntNode of int64 * sourceFile: string option
    | FloatNode of float * sourceFile: string option
    | BoolNode of bool * sourceFile: string option
    | NullNode of sourceFile: string option

module ConfigAst =

    let rec sourceOf node =
        match node with
        | ObjectNode (_, s)
        | ArrayNode (_, s)
        | StringNode (_, s)
        | IntNode (_, s)
        | FloatNode (_, s)
        | BoolNode (_, s)
        | NullNode s -> s

    let rec withSource src node =
        match node with
        | ObjectNode (m, _) -> ObjectNode (m, Some src)
        | ArrayNode (l, _) -> ArrayNode (l, Some src)
        | StringNode (s, _) -> StringNode (s, Some src)
        | IntNode (i, _) -> IntNode (i, Some src)
        | FloatNode (f, _) -> FloatNode (f, Some src)
        | BoolNode (b, _) -> BoolNode (b, Some src)
        | NullNode _ -> NullNode (Some src)

    // Convert raw parsed objects into ConfigNode
    let rec fromRawObject (src: string option) (value: obj) : ConfigNode =
        match value with
        | null -> NullNode src
        | :? ConfigNode as cn -> cn
        | :? bool as b -> BoolNode (b, src)
        | :? int as i -> IntNode (int64 i, src)
        | :? int64 as i -> IntNode (i, src)
        | :? float as f -> FloatNode (f, src)
        | :? decimal as m -> FloatNode (double m, src)
        | :? string as s -> StringNode (s, src)
        | :? JsonValue as jv ->
            let s = jv.ToString()
            let mutable b = false
            let mutable i = 0L
            let mutable d = 0.0
            if jv.TryGetValue<bool>(&b) then BoolNode (b, src)
            elif jv.TryGetValue<int64>(&i) then IntNode (i, src)
            elif jv.TryGetValue<double>(&d) then FloatNode (d, src)
            else StringNode (s, src)
        | :? JsonObject as jo ->
            let map =
                jo
                |> Seq.map (fun kvp -> kvp.Key, fromRawObject src kvp.Value)
                |> Map.ofSeq
            ObjectNode (map, src)
        | :? JsonArray as ja ->
            let list = ja |> Seq.map (fromRawObject src) |> Seq.toList
            ArrayNode (list, src)
        | :? TomlTable as tt ->
            let map =
                tt
                |> Seq.map (fun kvp -> kvp.Key, fromRawObject src kvp.Value)
                |> Map.ofSeq
            ObjectNode (map, src)
        | :? TomlArray as ta ->
            let list = ta |> Seq.map (fromRawObject src) |> Seq.toList
            ArrayNode (list, src)
        | :? TomlTableArray as tta ->
            let list = tta |> Seq.map (fromRawObject src) |> Seq.toList
            ArrayNode (list, src)
        | :? System.Collections.IDictionary as dict ->
            let seq = seq { for k in dict.Keys do yield string k, fromRawObject src dict.[k] }
            ObjectNode (Map.ofSeq seq, src)
        | :? System.Collections.IEnumerable as enum when not (enum :? string) ->
            let seq = seq { for item in enum do yield fromRawObject src item }
            ArrayNode (Seq.toList seq, src)
        | other -> StringNode (string other, src)

    // Convert ConfigNode into untyped .NET objects for serializers
    let rec toRawObject (node: ConfigNode) : obj =
        match node with
        | NullNode _ -> null
        | BoolNode (b, _) -> box b
        | IntNode (i, _) -> box i
        | FloatNode (f, _) -> box f
        | StringNode (s, _) -> box s
        | ArrayNode (items, _) ->
            let list = System.Collections.Generic.List<obj>()
            items |> List.iter (fun item -> list.Add(toRawObject item))
            box list
        | ObjectNode (props, _) ->
            let dict = System.Collections.Generic.Dictionary<string, obj>(StringComparer.Ordinal)
            props |> Map.iter (fun k v -> dict.[k] <- toRawObject v)
            box dict

    // Interop with System.Text.Json.Nodes.JsonNode
    let rec toJsonNode (node: ConfigNode) : JsonNode =
        match node with
        | NullNode _ -> null
        | BoolNode (b, _) -> JsonValue.Create(b) :> JsonNode
        | IntNode (i, _) -> JsonValue.Create(i) :> JsonNode
        | FloatNode (f, _) -> JsonValue.Create(f) :> JsonNode
        | StringNode (s, _) -> JsonValue.Create(s) :> JsonNode
        | ArrayNode (items, _) ->
            let arr = JsonArray()
            items |> List.iter (fun item ->
                let jn = toJsonNode item
                if jn <> null then arr.Add(jn) else arr.Add(null)
            )
            arr :> JsonNode
        | ObjectNode (props, _) ->
            let jo = JsonObject()
            props |> Map.iter (fun k v ->
                let jn = toJsonNode v
                jo.[k] <- jn
            )
            jo :> JsonNode

    let fromJsonNode (src: string option) (node: JsonNode) : ConfigNode =
        fromRawObject src node

    // Parsers for individual files
    let parseYaml (content: string) (sourcePath: string option) : ConfigNode =
        let deserializer = DeserializerBuilder().Build()
        use reader = new StringReader(content)
        let raw = deserializer.Deserialize(reader)
        fromRawObject sourcePath raw

    let parseJson (content: string) (sourcePath: string option) : ConfigNode =
        let parsed = JsonNode.Parse(content)
        fromRawObject sourcePath parsed

    let parseToml (content: string) (sourcePath: string option) : ConfigNode =
        let model = Toml.ToModel(content)
        fromRawObject sourcePath model

    let parseFile (filePath: string) : ConfigNode =
        let ext = Path.GetExtension(filePath).ToLowerInvariant()
        let content = File.ReadAllText(filePath, Encoding.UTF8)
        let src = Some filePath
        match ext with
        | ".yaml" | ".yml" -> parseYaml content src
        | ".json" -> parseJson content src
        | ".toml" -> parseToml content src
        | _ -> StringNode (content, src)

    // Merge two ObjectNode maps
    let rec mergeObjects (a: Map<string, ConfigNode>) (b: Map<string, ConfigNode>) : Map<string, ConfigNode> =
        b |> Map.fold (fun acc k v2 ->
            match acc.TryFind(k) with
            | Some (ObjectNode (m1, src1)) ->
                match v2 with
                | ObjectNode (m2, src2) ->
                    let merged = mergeObjects m1 m2
                    Map.add k (ObjectNode (merged, src2 |> Option.orElse src1)) acc
                | _ -> Map.add k v2 acc
            | _ -> Map.add k v2 acc
        ) a

    let setAtPath (pathSegments: string list) (value: ConfigNode) (root: ConfigNode) : ConfigNode =
        let rec loop segments current =
            match segments with
            | [] -> value
            | [ last ] ->
                match current with
                | ObjectNode (m, src) -> ObjectNode (Map.add last value m, src)
                | _ -> ObjectNode (Map.ofList [ last, value ], None)
            | head :: tail ->
                match current with
                | ObjectNode (m, src) ->
                    let child = m.TryFind(head) |> Option.defaultValue (ObjectNode (Map.empty, None))
                    let updatedChild = loop tail child
                    ObjectNode (Map.add head updatedChild m, src)
                | _ ->
                    let updatedChild = loop tail (ObjectNode (Map.empty, None))
                    ObjectNode (Map.ofList [ head, updatedChild ], None)
        loop pathSegments root

    let rec getAtPath (pathSegments: string list) (root: ConfigNode) : ConfigNode option =
        match pathSegments with
        | [] -> Some root
        | head :: tail ->
            match root with
            | ObjectNode (m, _) ->
                match m.TryFind(head) with
                | Some child -> getAtPath tail child
                | None -> None
            | _ -> None

    // Unified Directory-to-Graph AST Loader
    let loadDirectory (dirPath: string) : ConfigNode =
        let dir = Path.GetFullPath(dirPath)
        if not (Directory.Exists(dir)) then
            failwithf "Directory not found: %s" dir

        let allFiles =
            Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
            |> Array.filter (fun f ->
                let rel = Path.GetRelativePath(dir, f).Replace('\\', '/')
                let segments = rel.Split('/')
                let isIgnored =
                    segments |> Array.exists (fun s ->
                        s.Equals(".git", StringComparison.OrdinalIgnoreCase) ||
                        s.Equals(".starry", StringComparison.OrdinalIgnoreCase) ||
                        s.Equals(".github", StringComparison.OrdinalIgnoreCase) ||
                        s.Equals("node_modules", StringComparison.OrdinalIgnoreCase) ||
                        s.Equals("dist", StringComparison.OrdinalIgnoreCase) ||
                        s.Equals("bin", StringComparison.OrdinalIgnoreCase) ||
                        s.Equals("obj", StringComparison.OrdinalIgnoreCase)
                    )
                not isIgnored
            )
            |> Array.sort

        let mutable rootNode = ObjectNode (Map.empty, Some dir)

        for file in allFiles do
            let rel = Path.GetRelativePath(dir, file).Replace('\\', '/')
            let parsed = parseFile file

            let dirName = Path.GetDirectoryName(rel)
            let fileName = Path.GetFileName(rel)
            let fileNameNoExt = Path.GetFileNameWithoutExtension(rel)

            if String.IsNullOrEmpty(dirName) &&
               (fileNameNoExt.Equals("starry", StringComparison.OrdinalIgnoreCase) ||
                fileNameNoExt.Equals("config", StringComparison.OrdinalIgnoreCase)) then
                // Root config file: merge top-level keys directly into root
                match parsed with
                | ObjectNode (props, _) ->
                    match rootNode with
                    | ObjectNode (rootProps, src) ->
                        rootNode <- ObjectNode (mergeObjects rootProps props, src)
                    | _ -> ()
                | _ -> ()
            else
                let segments =
                    let pathWithoutExt =
                        if String.IsNullOrEmpty(dirName) then fileNameNoExt
                        else Path.Combine(dirName, fileNameNoExt).Replace('\\', '/')
                    pathWithoutExt.Split('/', StringSplitOptions.RemoveEmptyEntries)
                    |> Array.toList

                rootNode <- setAtPath segments parsed rootNode

        rootNode

    // Serializers
    let serializeYaml (node: ConfigNode) : string =
        let serializer = SerializerBuilder().ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull).Build()
        let raw = toRawObject node
        serializer.Serialize(raw)

    let serializeJson (node: ConfigNode) : string =
        let jn = toJsonNode node
        let options = JsonSerializerOptions(WriteIndented = true)
        if jn = null then "{}" else jn.ToJsonString(options)

    let serializeToml (node: ConfigNode) : string =
        let jn = toJsonNode node
        let rec tomlConvert (n: JsonNode) : obj =
            match n with
            | null -> null
            | :? JsonObject as jo ->
                let tt = TomlTable()
                for kv in jo do
                    let c = tomlConvert kv.Value
                    if c <> null then tt.[kv.Key] <- c
                box tt
            | :? JsonArray as ja ->
                let ta = TomlArray()
                for item in ja do
                    let c = tomlConvert item
                    if c <> null then ta.Add(c)
                box ta
            | :? JsonValue as jv ->
                let mutable b = false
                let mutable i = 0L
                let mutable d = 0.0
                if jv.TryGetValue<bool>(&b) then box b
                elif jv.TryGetValue<int64>(&i) then box i
                elif jv.TryGetValue<double>(&d) then box d
                else box (jv.ToString())
            | other -> box (other.ToString())

        let table =
            match tomlConvert jn with
            | :? TomlTable as tt -> tt
            | _ -> TomlTable()
        Toml.FromModel(table)

    let serializeFormat (format: string) (node: ConfigNode) : string =
        match format.ToLowerInvariant() with
        | "json" -> serializeJson node
        | "toml" -> serializeToml node
        | _ -> serializeYaml node

    // starry config pack
    let pack (root: ConfigNode) (format: string) : string =
        serializeFormat format root

    // starry config split
    let split (root: ConfigNode) (targetDir: string) (format: string) : unit =
        Directory.CreateDirectory(targetDir) |> ignore
        let ext =
            match format.ToLowerInvariant() with
            | "json" -> ".json"
            | "toml" -> ".toml"
            | _ -> ".yaml"

        match root with
        | ObjectNode (props, _) ->
            let mutable rootProperties = Map.empty

            props |> Map.iter (fun key node ->
                match key.ToLowerInvariant() with
                | "nodes" ->
                    match node with
                    | ObjectNode (nodesMap, _) ->
                        let nodesDir = Path.Combine(targetDir, "nodes")
                        Directory.CreateDirectory(nodesDir) |> ignore
                        nodesMap |> Map.iter (fun nodeName nodeVal ->
                            let filePath = Path.Combine(nodesDir, nodeName + ext)
                            File.WriteAllText(filePath, serializeFormat format nodeVal, Encoding.UTF8)
                        )
                    | _ -> rootProperties <- Map.add key node rootProperties
                | "services" ->
                    match node with
                    | ObjectNode (svcMap, _) ->
                        let svcDir = Path.Combine(targetDir, "services")
                        Directory.CreateDirectory(svcDir) |> ignore
                        svcMap |> Map.iter (fun svcName svcVal ->
                            let filePath = Path.Combine(svcDir, svcName + ext)
                            File.WriteAllText(filePath, serializeFormat format svcVal, Encoding.UTF8)
                        )
                    | _ -> rootProperties <- Map.add key node rootProperties
                | "domains" ->
                    let domDir = Path.Combine(targetDir, "domains")
                    Directory.CreateDirectory(domDir) |> ignore
                    let filePath = Path.Combine(domDir, "domains" + ext)
                    File.WriteAllText(filePath, serializeFormat format node, Encoding.UTF8)
                | "ports" ->
                    let filePath = Path.Combine(targetDir, "ports" + ext)
                    File.WriteAllText(filePath, serializeFormat format node, Encoding.UTF8)
                | "volumes" ->
                    let filePath = Path.Combine(targetDir, "volumes" + ext)
                    File.WriteAllText(filePath, serializeFormat format node, Encoding.UTF8)
                | "files" ->
                    let filesDir = Path.Combine(targetDir, "files")
                    Directory.CreateDirectory(filesDir) |> ignore
                    let rec dumpFiles (prefix: string) (fNode: ConfigNode) =
                        match fNode with
                        | StringNode (text, _) ->
                            let filePath = Path.Combine(filesDir, prefix)
                            let pDir = Path.GetDirectoryName(filePath)
                            if not (String.IsNullOrEmpty(pDir)) then Directory.CreateDirectory(pDir) |> ignore
                            File.WriteAllText(filePath, text, Encoding.UTF8)
                        | ObjectNode (fMap, _) ->
                            fMap |> Map.iter (fun subKey subVal ->
                                let subPath = if String.IsNullOrEmpty(prefix) then subKey else Path.Combine(prefix, subKey)
                                dumpFiles subPath subVal
                            )
                        | _ -> ()
                    dumpFiles "" node
                | _ ->
                    rootProperties <- Map.add key node rootProperties
            )

            let mainConfigFile = Path.Combine(targetDir, "starry" + ext)
            File.WriteAllText(mainConfigFile, serializeFormat format (ObjectNode (rootProperties, None)), Encoding.UTF8)
        | _ -> ()
