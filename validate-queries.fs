namespace ResourceManagerExploration

open Logic
open Query

module Validate =

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
             [ "Id"
               "Name"
               "Description"
               "PlanId"
               "Objective"
               "TotalDuration"
               "CreatedDate" ]) ]
      Sort = [] }
      
// TODO -----------

// SELECT
//     channel_id as ChannelId, user_handle as userHandle, percent_complete as PercentComplete, watched_duration as WatchedDuration
// FROM
//     dvs_replication.channel_progress_v1
// WHERE
//     channel_id in (
//                     select
//                         c.id
//                     from
//                         dvs_replication.channel_v1 c
//                           join dvs_replication.plan p on p.id = c.plan_id and p.has_priorities = true
//                     );