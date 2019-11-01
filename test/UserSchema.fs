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
type Schema<'T, 'F>(name: string) =
    member __.Name = name
    abstract Struct: List<Column>
    abstract Parser: 'F

type Repository<'T>(schema: Schema<'T, 'F`>) =
    member this.Insert<'T>(t: 'T) = None

type NewUser = NewUser of string * string

type NewUserFn = string -> string -> NewUser

type NewUserSchema private () =
    inherit Schema<NewUser, NewUserFn>("demo")
    static let instance = NewUserSchema()
    static member Instance = instance
    member __.ID = Column("ID")
    override this.Struct = [ this.ID ]
    override this.Parser = fun str0 str1 -> NewUser(str0, str1)

let newUserRepo = Repository(NewUserSchema.Instance)

newUserRepo.Insert(NewUser("", ""))
