namespace Femto

module DBUtils =
    open Dapper
    open Npgsql
    open System.Dynamic

    type Dict<'K, 'V> = System.Collections.Generic.IDictionary<'K, 'V>

    let inline ($) f x = f x

    let inline (=>) k v = (k, box v)

    let inline buildArgs args = Some $ dict args

    let envVars =
        System.Environment.GetEnvironmentVariables()
        |> Seq.cast<System.Collections.DictionaryEntry>
        |> Seq.map (fun d -> (d.Key :?> string, d.Value :?> string))
        |> dict

    let dbQuery<'T> (connection: NpgsqlConnection) sql (parameters: Option<Dict<string, obj>>) =
        try
            match parameters with
            | Some p ->
                let expando = ExpandoObject()
                let expandoDictionary = expando :> Dict<string, obj>
                for paramValue in p do
                    expandoDictionary.Add(paramValue.Key, paramValue.Value)
                Ok(connection.Query<'T>(sql, expandoDictionary))
            | None -> Ok(connection.Query<'T>(sql))
        with err -> Error err

    let withConn connString doFunc =
        use conn = new NpgsqlConnection(connString)
        conn.Open()
        doFunc conn
