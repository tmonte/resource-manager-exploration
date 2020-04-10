namespace ResourceManagerExploration

module rec Logic =

  type Pair<'Key, 'Value> = ('Key * 'Value)

  type FieldOperation<'Field> =
    | StartsWith of 'Field
    | Contains of 'Field
    | Equals of 'Field

  type Logic<'Field> =
    | And of Logic<'Field> * Logic<'Field>
    | Or of Logic<'Field> * Logic<'Field>
    | Field of FieldOperation<'Field>

  type private Field = Pair<string, string>

  let private a: Logic<Field> =
    Field
      (Equals ("Name", "Bob"))  

  let private b: Logic<Field> =
    And
      (Field
        (Equals("Name", "Bob")),
       Field
        (StartsWith("Description", "Hello")))

  let private c: Logic<Field> =
    And
      (Or
        (Field
          (Equals("Name", "Bob")),
         Field
          (Equals("Name", "John"))),
       Field
        (StartsWith("Description", "Hello")))