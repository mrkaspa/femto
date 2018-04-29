module ChangesetTests

open Expecto
open Femto

type User = {
    name: string
    age: int
}

let tests =
    testList "Changeset" [
        testCase "Updates the data on cast" <| fun _ ->
            let user = { name = ""; age = 20 }
            let parameters = Map.ofList ["name", "michel" :> obj]
            let ch = Changeset.cast parameters ["name"] user
            Expect.isTrue (ch.data.name = "michel") "changes name"
            Expect.isTrue (user.name = "") "does not change name"
        testCase "Validates a changeset" <| fun _ ->
            let user = { name = ""; age = 18 }
            let parameters = Map.ofList ["name", "michel" :> obj]
            let ch =
                user
                |> Changeset.cast parameters ["name"]
                |> Changeset.addValidation
                    (fun u -> u.age < 20)
                    "age"
                    "should be greater than 20"
                |> Changeset.addValidation
                    (fun u -> u.name <> "")
                    "name"
                    "should not be empty"
                |> Changeset.validate
            Expect.isFalse ch.valid.Value "invalid changeset"
            Expect.isNonEmpty ch.errors "must have errors"
            // printfn "Map = %A" ch.errors
            Expect.isTrue (Map.count ch.errors = 2) "has 2 errors"
    ]