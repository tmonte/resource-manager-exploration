namespace ResourceManagerExploration

open Logic

module rec Main =
      
    type LevelNumber = int

    // what is popularity?relevance? - aggregation?
    type LevelFilter =
      | PlanId of string
      | Metadata of Logic<Pair<string, string>>

    type LevelMetadataFilter = (LevelNumber *  LevelFilter)

    type Filter =
      { Level: LevelFilter list option
        Parent: LevelMetadataFilter list option
        Child: LevelMetadataFilter list option }

    type LevelInclusion =
      | All
      | Fields of string list

    type LevelMetadataInclusion = (LevelNumber * LevelInclusion)

    type Inclusion =
      { Level: LevelInclusion list option
        Parent: LevelMetadataInclusion list option
        Child: LevelMetadataInclusion list option }

    type SortDirection =
      | Ascending
      | Descending

    type Sort =
      { Direction: SortDirection
        Current: string list
        Levels: string option }

    type Query =
      { Filter: Filter option
        Inclusion: Inclusion option
        Sort: Sort option }

    let query: Query =
      { Filter =
          Some
            { Level =
                Some
                  [ Metadata (Field(Equals("Name", "Bob"))) ]
              Parent = None
              Child = None }
        Inclusion =
          Some
            { Level = None // TODO: we want this to be all?
              Parent =
                Some
                  [ (2, All)
                    (1, Fields [ "Name"; "Description" ])]
              Child =
                Some
                  [(1, Fields [ "Name"; "Technologies" ])]}
        Sort = None }

    printfn "%A" query