module UserSchema

open Femto

type User = {
    [<Model.ID>]
    id: int
    name: string
    age: int
}

let changeset model parameters  =
    model
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
