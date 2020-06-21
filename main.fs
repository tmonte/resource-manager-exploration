open ResourceManagerExploration.Logic
open ResourceManagerExploration.Query
open ResourceManagerExploration.Process

let priorities1800contacts: Query =
  { Filter =
      Some
        (And
          (And
            (Field(0, PlanId(Equals(String "1800contacts"))),
             Field(0, Metadatum(Equals("Tag", String "Priority")))),
            Field(0, Relation("Technologies", Metadatum(StartsWith("Name", String "Cloud"))))))
    Inclusion =
      [ (0, Everything)
        (0, Related("Technologies", Everything)) ]
    Sort = [ (0, [ Ascending("Name") ]) ] }

let getPrioritiesById planId: Query =
  { Filter = Some(Field(0, PlanId(Equals(String planId))))
    Inclusion =
      [ (0,
          Select
            [ "Id"
              "Title"
              "Description"
              "Deadline"
              "TimeRangeStart"
              "TimeRangeEnd"
              "CreatedBy"
              "CreatedDate" ])
        (0, Related("PriorityPlans", Select [ "PlanId" ]))
        (0, Related("Technologies", Everything))
        (0, Related("Templates", Select [ "Id"; "Name" ]))
        (0, Related("ChannelGroups", Select [ "Id"; "Name" ])) ]
    Sort = [] }

let getChannelsByChannelGroupId myId planId: Query =
  { Filter =
      Some
        (All
          [ (-1, ProjectId(Equals(String myId)))
            (0, PlanId(Equals(String planId)))
            (0, Metadatum(Equals("PrivacyLevel", String "org")))
            (0, PlanId(NotEquals(Null)))
            (0, Metadatum(NotEquals("CreatedDate", Null))) ])
    Inclusion =
      [ (0,
          Select
            [ "Id"; "Name"; "Description"; "PlanId"; "Objective"; "TotalDuration"; "CreatedDate" ]) ]
    Sort = [] }

let getChannelsByPlanId myId planId: Query =
  { Filter =
      Some
        (All
          [ (0, PlanId(Equals(String planId)))
            (0, Relation("Plan", Metadatum(Equals("HasPriorities", Boolean true))))
            (0, Metadatum(NotEquals("CreatedDate", Null))) ])
    Inclusion =
      [ (0, Select [ "ChannelId"; "UserHandle" ])
        (0, Related("ChannelProgressV1", Select [ "PercentComplete"; "WatchedDuration" ])) ]
    Sort = [] }

let res = transform priorities1800contacts

printfn "%A" res
