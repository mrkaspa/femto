module RepoTests

open Expecto
open UserSchema
open Femto.Repo
open Femto.DBUtils


let getDbURL() =
    try
        let connString =
            System.Environment.GetEnvironmentVariable("DATABASE_URL")
        Ok connString
    with :? System.ArgumentNullException as e ->
        let err = e :> System.Exception
        Error err

[<Tests>]
let tests =
    testList "Changeset" [
        testCase "getQuery" <| fun _ ->
            let query = Queries.getQuery<User> ()
            Expect.isTrue (query = "select * from users where user_id = @user_id") "Query generated"

        testCase "insertQuery" <| fun _ ->
            let query = Queries.insertQuery<User> ()
            printfn "%s" query
            Expect.isTrue (query = "insert into users (age, name) values (@age, @name) returning *") "Insert generated"

        testCase "updateQuery" <| fun _ ->
            let query = Queries.updateQuery<User> ()
            Expect.isTrue (query = "update users set (age = @age, name = @name) where user_id = @user_id") "Update generated"

        testCase "deleteQuery" <| fun _ ->
            let query = Queries.deleteQuery<User> ()
            Expect.isTrue (query = "delete from users where user_id = @user_id") "Delete generated"

        testCase "Inserting an user" <| fun _ ->
            getDbURL ()
            |> Result.bind (fun connURL ->
                withConn connURL (fun conn ->
                    let query = Queries.insertQuery<User> ()
                    let res = dbQuery<obj> conn query (buildArgs ["name" => "Michel"; "age" => 21])
                    let query = Queries.getQuery<User> ()
                    let res = dbQuery<obj> conn query (buildArgs ["user_id" => 3])
                    Ok res
                )
            )
            |> ignore
    ]
