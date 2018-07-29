namespace Femto

module TypeUtils =
    let updateModel (parameters : Map<string, obj>) (any : 'T) : 'T =
        let ttype = typeof<'T>
        let props = ttype.GetProperties()
        let attrList =
            props |>
            Array.map (fun prop ->
                if Map.containsKey prop.Name parameters then
                    Map.find prop.Name parameters
                else
                    prop.GetValue(any)
            )
        let newModel = FSharp.Reflection.FSharpValue.MakeRecord(ttype, attrList) :?> 'T
        newModel
