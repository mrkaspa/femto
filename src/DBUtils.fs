namespace Femto

module DBUtils =
    open Dapper
    open Npgsql
    open System.Dynamic

    type Dict<'K, 'V> = System.Collections.Generic.IDictionary<'K, 'V>

    let inline ($) f x = f x
    let inline (=>) k v = (k, box v)
    let buildArgs args = Some $ dict args

    let envVars =
        System.Environment.GetEnvironmentVariables()
        |> Seq.cast<System.Collections.DictionaryEntry>
        |> Seq.map (fun d -> (d.Key :?> string, d.Value :?> string))
        |> dict

    let dbQuery<'T> (connection : NpgsqlConnection) sql
        (parameters : Option<Dict<string, obj>>) =
        match parameters with
        | Some p ->
            let expando = ExpandoObject()
            let expandoDictionary = expando :> Dict<string, obj>
            for paramValue in p do
                expandoDictionary.Add(paramValue.Key, paramValue.Value)
            connection.Query<'T>(sql, expandoDictionary)
        | None -> connection.Query<'T>(sql)

    let withConn doFunc connString =
        use conn = new NpgsqlConnection(connString)
        conn.Open()
        doFunc conn

    let getFirstResult conn query args =
        let res = dbQuery<int> conn query args
        res
        |> List.ofSeq
        |> List.tryHead
