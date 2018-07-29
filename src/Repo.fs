namespace Femto

open DBUtils
open Model

module Repo =
    open Changeset

    // let insert changeset =
    //     ()

    // let update changeset =
    //     ()

    // let remove id =
    //     ()

    let get<'T> conn id =
        let tableName = getTableName<'T> ()
        let query = sprintf "select * from %s where id = @Id" tableName
        let res =
            ["Id" => id]
            |> buildArgs
            |> dbQuery<'T> conn query
            |> List.ofSeq
        match res with
        | fst::_ -> Some fst
        | _ -> None
