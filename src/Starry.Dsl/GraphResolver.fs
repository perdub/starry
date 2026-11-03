namespace Starry.Dsl

open System
open System.Text.RegularExpressions

module GraphResolver =

    type ResolutionError =
        | CircularReference of cycleChain: string list
        | NodeNotFound of path: string
        | InvalidInheritanceTarget of path: string

    let private referenceRegex = Regex(@"\$\{?(?<path>[a-zA-Z0-9_\.]+)\}?", RegexOptions.Compiled)

    let parsePath (pathStr: string) : string list =
        let trimmed =
            let s = pathStr.Trim()
            if s.StartsWith("$") then s.Substring(1) else s
        trimmed.Split('.', StringSplitOptions.RemoveEmptyEntries) |> Array.toList

    let rec resolvePath (pathSegments: string list) (root: ConfigNode) : ConfigNode option =
        ConfigAst.getAtPath pathSegments root

    let scalarToString (node: ConfigNode) : string option =
        match node with
        | StringNode (s, _) -> Some s
        | IntNode (i, _) -> Some (string i)
        | FloatNode (f, _) -> Some (string f)
        | BoolNode (b, _) -> Some (if b then "true" else "false")
        | NullNode _ -> Some ""
        | _ -> None

    let rec resolveNode
        (decryptSecret: (string -> string) option)
        (activeStack: string list)
        (root: ConfigNode)
        (currentNode: ConfigNode) : Result<ConfigNode, ResolutionError> =

        match currentNode with
        | StringNode (text, src) ->
            let trimmed = text.Trim()
            // 1. Whole-subtree substitution (e.g. "$presets.default_env")
            if (trimmed.StartsWith("$") || trimmed.StartsWith("${")) &&
               not (trimmed.Contains(" ")) &&
               (trimmed.StartsWith("$") && not (trimmed.Substring(1).Contains("$"))) then
                let pathStr =
                    if trimmed.StartsWith("${") && trimmed.EndsWith("}") then
                        trimmed.Substring(2, trimmed.Length - 3)
                    elif trimmed.StartsWith("$") then
                        trimmed.Substring(1)
                    else trimmed

                if List.contains pathStr activeStack then
                    Error (CircularReference (List.rev (pathStr :: activeStack)))
                else
                    match resolvePath (parsePath pathStr) root with
                    | Some targetNode ->
                        // Recursively resolve target subtree
                        resolveNode decryptSecret (pathStr :: activeStack) root targetNode
                    | None ->
                        // If path not in tree, leave as literal string
                        Ok (StringNode (text, src))
            else
                // 3. String & file content interpolation ($path.to.value and ${path.to.value})
                let mutable hasError: ResolutionError option = None
                let interpolated =
                    referenceRegex.Replace(text, (fun m ->
                        let p = m.Groups.["path"].Value
                        if List.contains p activeStack then
                            hasError <- Some (CircularReference (List.rev (p :: activeStack)))
                            m.Value
                        else
                            match resolvePath (parsePath p) root with
                            | Some target ->
                                match resolveNode decryptSecret (p :: activeStack) root target with
                                | Ok resolvedTarget ->
                                    match scalarToString resolvedTarget with
                                    | Some s ->
                                        let finalStr =
                                            match decryptSecret with
                                            | Some decrypt -> decrypt s
                                            | None -> s
                                        finalStr
                                    | None -> m.Value
                                | Error err ->
                                    hasError <- Some err
                                    m.Value
                            | None -> m.Value
                    ))

                match hasError with
                | Some err -> Error err
                | None ->
                    let finalText =
                        match decryptSecret with
                        | Some decrypt -> decrypt interpolated
                        | None -> interpolated
                    Ok (StringNode (finalText, src))

        | ArrayNode (items, src) ->
            let resolvedItems =
                items
                |> List.map (resolveNode decryptSecret activeStack root)

            let rec collect (acc: ConfigNode list) (list: Result<ConfigNode, ResolutionError> list) =
                match list with
                | [] -> Ok (ArrayNode (List.rev acc, src))
                | (Ok n) :: rest -> collect (n :: acc) rest
                | (Error e) :: _ -> Error e

            collect [] resolvedItems

        | ObjectNode (props, src) ->
            // 2. Object Inheritance & Merging ($: key)
            let inheritanceRefs =
                match props.TryFind("$") with
                | Some (StringNode (refPath, _)) -> [ refPath ]
                | Some (ArrayNode (refList, _)) ->
                    refList
                    |> List.choose (function
                        | StringNode (r, _) -> Some r
                        | _ -> None)
                | _ -> []

            let mutable baseMap = Map.empty
            let mutable inheritError: ResolutionError option = None

            for refPath in inheritanceRefs do
                if inheritError.IsNone then
                    let cleanPath = if refPath.StartsWith("$") then refPath.Substring(1) else refPath
                    if List.contains cleanPath activeStack then
                        inheritError <- Some (CircularReference (List.rev (cleanPath :: activeStack)))
                    else
                        match resolvePath (parsePath cleanPath) root with
                        | Some (ObjectNode _ as baseNode) ->
                            match resolveNode decryptSecret (cleanPath :: activeStack) root baseNode with
                            | Ok (ObjectNode (resolvedBaseProps, _)) ->
                                baseMap <- ConfigAst.mergeObjects baseMap resolvedBaseProps
                            | Ok _ ->
                                inheritError <- Some (InvalidInheritanceTarget cleanPath)
                            | Error err ->
                                inheritError <- Some err
                        | Some _ ->
                            inheritError <- Some (InvalidInheritanceTarget cleanPath)
                        | None ->
                            inheritError <- Some (NodeNotFound cleanPath)

            match inheritError with
            | Some err -> Error err
            | None ->
                // Remove the special "$" inheritance key from explicitly defined properties
                let explicitProps = Map.remove "$" props

                // Merge baseMap with explicitProps (explicit overrides inherited)
                let combinedProps = ConfigAst.mergeObjects baseMap explicitProps

                let resolvedPropsResult =
                    combinedProps
                    |> Map.fold (fun accResult k v ->
                        match accResult with
                        | Error e -> Error e
                        | Ok accMap ->
                            match resolveNode decryptSecret activeStack root v with
                            | Ok rv -> Ok (Map.add k rv accMap)
                            | Error e -> Error e
                    ) (Ok Map.empty)

                match resolvedPropsResult with
                | Ok rm -> Ok (ObjectNode (rm, src))
                | Error e -> Error e

        | leaf -> Ok leaf

    let resolve (root: ConfigNode) (decryptSecret: (string -> string) option) : Result<ConfigNode, ResolutionError> =
        resolveNode decryptSecret [] root root
