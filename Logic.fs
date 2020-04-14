namespace ResourceManagerExploration

open System

module rec Logic =

  type Value =
    | String of string
    | Int of int
    | Float of float
    | Date of DateTimeOffset
    | Boolean of bool
    | Null

  type Pair<'Key, 'Value> = ('Key * 'Value)

  type FieldOperation<'Field> =
    | StartsWith of 'Field
    | Contains of 'Field
    | Equals of 'Field
    | NotEquals of 'Field

  type Logic<'Field> =
    | And of Logic<'Field> * Logic<'Field>
    | Or of Logic<'Field> * Logic<'Field>
    | All of 'Field list
    | Any of 'Field list
    | Field of 'Field

  type private Operation = FieldOperation<Pair<string, Value>>

  // Examples
  let private a: Logic<Operation> =
    Field
      (Equals ("Name", String "Bob"))

  let private b: Logic<Operation> =
    And
      (Field
        (Equals("Name", String "Bob")),
       Field
        (StartsWith("Description", String "Hello")))

  let private c: Logic<Operation> =
    And
      (Or
        (Field
          (Equals("Name", String "Bob")),
         Field
          (Equals("Name", String "John"))),
       Field
        (StartsWith("Description", Int 2)))