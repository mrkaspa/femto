module UserSchema

open Femto

[<Model.Table(Name="users", Pk="id")>]
type User = {
    [<Model.ID(Name="user_id")>]
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
