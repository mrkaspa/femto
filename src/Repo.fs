namespace Femto

open DBUtils
open Model

module Repo =
    module Queries =
        let insertQuery<'T> () =
            let tableName = getTableName<'T> ()
            sprintf "insert into %s () values ()" tableName

        let updateQuery<'T> () =
            let tableName = getTableName<'T> ()
            let idName = getIdName<'T> ()
            sprintf "update %s set () values () where %s = @Id" tableName idName

        let deleteQuery<'T> () =
            let tableName = getTableName<'T> ()
            let idName = getIdName<'T> ()
            sprintf "delete from %s where %s = @Id" tableName idName

        let getQuery<'T> () =
            let tableName = getTableName<'T> ()
            let idName = getIdName<'T> ()
            sprintf "select * from %s where %s = @Id" tableName idName

    open Changeset
    open Queries

    let insert changeset =
        ()

    let update changeset =
        ()

    let remove id =
        ()

    let get<'T> conn id =
        let macthRes = function
            | fst::_ -> Some fst
            | _ -> None
        let query = getQuery<'T> ()
        ["Id" => id]
        |> buildArgs
        |> dbQuery<'T> conn query
        |> List.ofSeq
        |> macthRes
