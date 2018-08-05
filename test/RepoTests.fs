module RepoTests

open Expecto
open UserSchema
open Femto.Repo

[<Tests>]
let tests =
    testList "Changeset" [
        testCase "getQuery" <| fun _ ->
            let query = Queries.getQuery<User> ()
            Expect.isTrue (query = "select * from users where user_id = @user_id") "Query generated"

        testCase "insertQuery" <| fun _ ->
            let query = Queries.insertQuery<User> ()
            Expect.isTrue (query = "insert into users (age, name) values (@age, @name)") "Query generated"

        testCase "updateQuery" <| fun _ ->
            let query = Queries.updateQuery<User> ()
            Expect.isTrue (query = "update users set (age = @age, name = @name) where user_id = @user_id") "Query generated"

        testCase "deleteQuery" <| fun _ ->
            let query = Queries.deleteQuery<User> ()
            Expect.isTrue (query = "delete from users where user_id = @user_id") "Query generated"
    ]
