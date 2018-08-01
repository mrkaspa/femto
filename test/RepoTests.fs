module RepoTests

open Expecto
open UserSchema
open Femto

[<Tests>]
let tests =
    testList "Changeset" [
        testCase " " <| fun _ ->
            let query = Repo.getQuery<User> ()
            Expect.isTrue (query = "select * from users where user_id = @Id") "Query generated"
    ]
