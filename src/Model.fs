namespace Femto

module Model =
    module ReflectionUtils =
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

        let hasAttr
            (attrs : System.Collections.Generic.IEnumerator<System.Reflection.CustomAttributeData>)
            (tag : System.Type) =
            let mutable breakCond = false
            while attrs.MoveNext() && not breakCond do
                let attr = attrs.Current
                if attr.AttributeType = tag then
                    breakCond <- true
            breakCond

    module Meta =
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

        type Column() =
            inherit System.Attribute()

            let mutable nameMut = ""

            member __.Name
                with get() = nameMut
                and set(value) = nameMut <- value

        type Virtual() =
            inherit System.Attribute()

        let tableType = typeof<Table>

        let idType = typeof<ID>

        let virtualType = typeof<Virtual>

    open ReflectionUtils
    open Meta

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

    let getFields<'T> () =
        let ttype = typeof<'T>
        let fields = ttype.GetProperties().GetEnumerator()
        let mutable fieldsArr = List.empty<string>
        while fields.MoveNext() do
            let field = fields.Current :?> System.Reflection.PropertyInfo
            let attrs1 = field.CustomAttributes.GetEnumerator()
            let attrs2 = field.CustomAttributes.GetEnumerator()
            if not (hasAttr attrs1 virtualType) && not (hasAttr attrs2 idType) then
                fieldsArr <- field.Name :: fieldsArr
        fieldsArr
