module UserSchema

open Femto
open Model

[<Meta.Table(Name = "users", Pk = "id")>]
type User =
    { [<Meta.ID(Name = "user_id")>]
      id: int
      name: string
      age: int }

let changeset model parameters =
    model
    |> Changeset.cast parameters [ "name" ]
    |> Changeset.addValidation (fun u -> u.age < 20) "age" "should be greater than 20"
    |> Changeset.addValidation (fun u -> u.name <> "") "name" "should not be empty"
    |> Changeset.validate

type Column(name) =
    member this.Name = name

[<AbstractClass>]
type Schema<'T>(name: string) =
    member this.Name = name
    member this.Column(name) = Column(name)
    abstract Struct: unit -> 'T

// type NewUser =
//     { ID: string
//       Name: string }

type NewUser = NewUser of string * string

type NewUserSchema() =
    inherit Schema<NewUser>("demo")
    member this.ID = this.Column("ID")
    override __.Struct() = NewUser("1", "2")
