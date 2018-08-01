module RepoTests

open Expecto
open UserSchema
open Femto.Repo

[<Tests>]
let tests =
    testList "Changeset" [
        testCase "getQuery" <| fun _ ->
            let query = Queries.getQuery<User> ()
            Expect.isTrue (query = "select * from users where user_id = @Id") "Query generated"

        testCase "deleteQuery" <| fun _ ->
            let query = Queries.deleteQuery<User> ()
            Expect.isTrue (query = "delete from users where user_id = @Id") "Query generated"
    ]
