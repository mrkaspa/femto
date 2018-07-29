namespace Femto

module Model =
    type Table() =
        inherit System.Attribute()

        let mutable nameMut = ""
        let mutable pkMut = ""

        member __.Name
            with get() = nameMut
            and set(value) = nameMut <- value

        member __.Pk
            with get() = pkMut
            and set(value) = pkMut <- value

    type ID() =
        inherit System.Attribute()

        let mutable nameMut = ""

        member __.Name
            with get() = nameMut
            and set(value) = nameMut <- value

    let tableType = typeof<Table>

    let idType = typeof<ID>

    let findValuesInAttr
        (attrs : System.Collections.Generic.IEnumerator<System.Reflection.CustomAttributeData>)
        (tag : System.Type)
        (values : List<string>) =
        let mutable res = Map.empty
        while attrs.MoveNext() do
            let attr = attrs.Current
            if attr.AttributeType = tag then
                let args = attr.NamedArguments.GetEnumerator()
                while args.MoveNext() do
                    let arg = args.Current
                    if List.contains arg.MemberName values then
                        let value = arg.TypedValue.Value :?> string
                        res <- res.Add(arg.MemberName, value)
        res

    let getTableName<'T> () =
        let ttype = typeof<'T>
        let attrs = ttype.CustomAttributes.GetEnumerator()
        let res = findValuesInAttr attrs tableType ["Name"]
        match res.TryFind("Name") with
        | Some value -> value
        | None -> ttype.Name

    let getIdName<'T> () =
        let ttype = typeof<'T>
        let fields = ttype.GetProperties().GetEnumerator()
        let mutable idColumn = "id"
        let mutable foundCond = false
        while fields.MoveNext() && not foundCond do
            let field = fields.Current :?> System.Reflection.PropertyInfo
            let attrs = field.CustomAttributes.GetEnumerator()
            let res = findValuesInAttr attrs idType ["Name"]
            match res.TryFind("Name") with
            | Some value ->
                foundCond <- true
                idColumn <- value
            | None -> ()
        idColumn
