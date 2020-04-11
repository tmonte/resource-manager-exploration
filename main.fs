namespace ResourceManagerExploration

open Logic

module rec Main =
      
    type Levels<'QueryProperty> =
      { Self: 'QueryProperty
        Parent: 'QueryProperty
        GrandParent: 'QueryProperty
        Child: 'QueryProperty
        GrandChild: 'QueryProperty }

    type FilterProperties =
      | PlanId of FieldOperation<Value>
      | ProjectId of FieldOperation<Value>
      | Metadata of FieldOperation<Pair<string, Value>>
      | Relation of string * FieldOperation<Pair<string, Value>>
    
    type FieldFilter =
      | Filter of Logic<FilterProperties>
      | Nothing

    type Filter = Levels<FieldFilter>

    let filter: Filter =
      { Self = FieldFilter.Nothing
        Parent = FieldFilter.Nothing
        GrandParent = FieldFilter.Nothing
        Child = FieldFilter.Nothing
        GrandChild = FieldFilter.Nothing }

    type FieldInclusion =
      | All
      | Select of string list
      | Nothing

    type Inclusion = Levels<FieldInclusion>

    let inclusion: Inclusion =
      { Self = FieldInclusion.Nothing
        Parent = FieldInclusion.Nothing
        GrandParent = FieldInclusion.Nothing
        Child = FieldInclusion.Nothing
        GrandChild = FieldInclusion.Nothing }

    type FieldSort =
      | Ascending of string
      | Descending of string

    type Sort = Levels<FieldSort list>

    let sort: Sort =
      { Self = []
        Parent = []
        GrandParent = []
        Child = []
        GrandChild = [] }

    type Query =
      { Filter: Filter
        Inclusion: Inclusion
        Sort: Sort }

    printfn "*** All Priorities for a 1800contacts Plan ***" 

    let priorities1800contacts: Query =
      { Filter =
          { filter with
              Self = Filter
                (And
                  (And
                    (Field
                      (PlanId (Equals(String "1800contacts"))),
                     Field
                      (Metadata (Equals("Tag", String "Priority")))),
                   Field
                    (Relation ("Technologies", (StartsWith("Name", String "Cloud")))))) }
        Inclusion =
          { inclusion with Self = All }
        Sort =
          { sort with Self = [Ascending("Name")] } }

    printfn "%A" priorities1800contacts

    // [<Interface>]
    // type IProjectAccess =
    //   abstract GetProject : Query -> Result<Project, Error>