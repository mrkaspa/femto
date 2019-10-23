module ModelTests

open Expecto
open UserSchema
open Femto.Model

[<Tests>]
let tests =
    testList "Model"
        [ testCase "Gets table name" <| fun _ ->
            let tableName = getTableName<User>()
            Expect.isTrue (tableName = "users") "same table name"
          testCase "Gets table id" <| fun _ ->
              let idName = getIdName<User>()
              Expect.isTrue (idName = "user_id") "same id name"
          testCase "" <| fun _ ->
              let fields = getFields<User>()
              Expect.isTrue (fields = [ "age"; "name" ]) "same fields" ]
