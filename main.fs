namespace ResourceManagerExploration

open Logic

module rec Main =

  type FieldFilter =
    | PlanId of FieldOperation<Value>
    | ProjectId of FieldOperation<Value>
    | Metadatum of FieldOperation<Pair<string, Value>>
    | Relation of string * FieldFilter

  type Filter = Logic<int * FieldFilter>

  type FieldInclusion =
    | All
    | Select of string list
    | Related of string * FieldInclusion
    | Nothing

  type Inclusion = (int * FieldInclusion) list

  type FieldSort =
    | Ascending of string
    | Descending of string

  type Sort = (int * FieldSort list) list

  type Query =
    { Filter: Filter
      Inclusion: Inclusion
      Sort: Sort }

  printfn "*** All Priorities for a 1800contacts Plan ***"

  let priorities1800contacts: Query =
    { Filter =
        And
          (And
            (Field(0, PlanId(Equals(String "1800contacts"))),
             Field(0, Metadatum(Equals("Tag", String "Priority")))),
           Field(0, Relation("Technologies", Metadatum(StartsWith("Name", String "Cloud")))))
      Inclusion =
        [ (0, All)
          (0, Related("Technologies", All)) ]
      Sort = [ (0, [ Ascending("Name") ]) ] }

  printfn "%A" priorities1800contacts
