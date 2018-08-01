module RepoTests

open Expecto
open UserSchema
open Femto.Repo

[<Tests>]
let tests =
    testList "Changeset" [
        testCase " " <| fun _ ->
            let query = Queries.getQuery<User> ()
            Expect.isTrue (query = "select * from users where user_id = @Id") "Query generated"
    ]
