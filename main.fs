namespace ResourceManagerExploration

open Logic

module rec Main =
      
    type LevelNumber = int

    type AvailableFilters =
      | PlanId of string
      | ProjectId of string
      | Metadata of Logic<Pair<string, Value>>
      |

    // what is popularity?relevance? - aggregation?
    type LevelFilter =
      | Filter of Logic<Pair<string, Value>>
      | Nothing

    type LevelMetadataFilter = (LevelNumber *  LevelFilter)

    type Filter =
      { Self: LevelFilter list option
        Parent: LevelMetadataFilter list option
        Child: LevelMetadataFilter list option }

    type LevelInclusion =
      | All
      | Select of string list
      | Nothing

    type Inclusion =
      { Self: LevelInclusion
        Parent: LevelInclusion
        Child: LevelInclusion
        GrandChild: LevelInclusion }

    type SortDirection =
      | Ascending
      | Descending

    type Sort =
      { Direction: SortDirection
        Current: string list
        Levels: string option }

    type Query =
      { Filter: Filter option
        Inclusion: Inclusion
        Sort: Sort option }

    let inclusion =
      { Self = Nothing
        Parent = Nothing
        GrandParent = Nothing
        Child = Nothing
        GrandChild = Nothing
        GreatGrandChild = Nothing }

    let query: Query =
      { Filter =
          Some
            { Self =
                Filter
                  (And
                    (Field
                      (Equals("Name", String "Bob"))),
                     Field
                      (Equals("PlanId", String "abcd")))
              Parent = None
              Child = None }
        Inclusion = 
          {inclusion with Self = Select["Name", "Technologies"]; Parent = All}
        Sort = None }

    printfn "%A" query