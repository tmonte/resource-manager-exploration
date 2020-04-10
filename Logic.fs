namespace ResourceManagerExploration

open System

module rec Logic =

  type Value =
    | String of string
    | Int of int
    | Float of float
    | Date of DateTimeOffset

  type Pair<'Key, 'Value> = ('Key * 'Value)

  type FieldOperation<'Field> =
    | StartsWith of 'Field
    | Contains of 'Field
    | Equals of 'Field

  type Logic<'Field> =
    | And of Logic<'Field> * Logic<'Field>
    | Or of Logic<'Field> * Logic<'Field>
    | Field of FieldOperation<'Field>

  type private Field = Pair<string, Value>

  // Examples
  let private a: Logic<Field> =
    Field
      (Equals ("Name", String "Bob"))

  let private b: Logic<Field> =
    And
      (Field
        (Equals("Name", String "Bob")),
       Field
        (StartsWith("Description", String "Hello")))

  let private c: Logic<Field> =
    And
      (Or
        (Field
          (Equals("Name", String "Bob")),
         Field
          (Equals("Name", String "John"))),
       Field
        (StartsWith("Description", Int 2)))