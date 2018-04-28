namespace Femto

module TypeUtils =
    let updateModel (parameters : Map<string, obj>) (any : 'T) : 'T =
        let typ = any.GetType()
        let props = typ.GetProperties()
        let attrList =
            props |>
            Array.map (fun prop ->
                if Map.containsKey prop.Name parameters then
                    Map.find prop.Name parameters
                else
                    prop.GetValue(any)
            )
        let newModel = FSharp.Reflection.FSharpValue.MakeRecord(typ, attrList) :?> 'T
        newModel
