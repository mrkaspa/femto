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

    let getQuery<'T> () =
        let tableName = getTableName<'T> ()
        let idName = getIdName<'T> ()
        sprintf "select * from %s where %s = @Id" tableName idName

    let get<'T> conn id =
        let query = getQuery<'T> ()
        let res =
            ["Id" => id]
            |> buildArgs
            |> dbQuery<'T> conn query
            |> List.ofSeq
        match res with
        | fst::_ -> Some fst
        | _ -> None
