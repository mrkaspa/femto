namespace Femto


module Repo =
    open Model

    module Queries =
        let insertQuery<'T>() =
            let genFields fields =
                let strFields = String.concat ", " fields

                let strFieldsPlaceholders =
                    fields
                    |> List.map (fun str -> "@" + str)
                    |> String.concat ", "
                (strFields, strFieldsPlaceholders)

            let tableName = getTableName<'T>()
            let fields = getFields<'T>()
            let (strFields, strFieldsPlaceholders) = genFields fields
            sprintf "insert into %s (%s) values (%s) returning *" tableName strFields strFieldsPlaceholders

        let updateQuery<'T>() =
            let tableName = getTableName<'T>()
            let idName = getIdName<'T>()
            let fields = getFields<'T>()

            let strFieldsWithPlaceholders =
                fields
                |> List.map (fun str -> str + " = @" + str)
                |> String.concat ", "
            sprintf "update %s set (%s) where %s = @%s" tableName strFieldsWithPlaceholders idName idName

        let deleteQuery<'T>() =
            let tableName = getTableName<'T>()
            let idName = getIdName<'T>()
            sprintf "delete from %s where %s = @%s" tableName idName idName

        let getQuery<'T>() =
            let tableName = getTableName<'T>()
            let idName = getIdName<'T>()
            sprintf "select * from %s where %s = @%s" tableName idName idName

    open DBUtils
    open Changeset
    open Queries

    let insert changeset = ()

    let update changeset = ()

    let remove conn id = ()

    let get<'T> conn id =
        let macthRes =
            function
            | fst :: _ -> Some fst
            | _ -> None

        let query = getQuery<'T>()
        let idName = getIdName<'T>()

        [ idName => id ]
        |> buildArgs
        |> dbQuery<'T> conn query
        |> Result.bind
            (List.ofSeq
             >> macthRes
             >> Ok)
