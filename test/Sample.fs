module Tests

open Expecto
open Femto

type User = {
    name: string
    age: int
}

[<Tests>]
let tests =
    testList "Changeset" [
        testCase "Updates the data on cast" <| fun _ ->
            let user = { name = ""; age = 20}
            let parameters = Map.ofList ["name", "michel" :> obj]
            let ch = Changeset.cast parameters ["name"] user
            Expect.isTrue (ch.data.name = "michel") "changes name"
            Expect.isTrue (user.name = "") "does not change name"
    ]
