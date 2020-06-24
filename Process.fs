namespace ResourceManagerExploration

open Logic
open Query

module rec Process =

  let private processValue value =
    match value with
    | String stringValue -> "'" + stringValue + "'"
    | _ -> ""

  let private processMetadum metadatum =
    match metadatum with
    | Equals (key, value) -> "metadata." + key + " = " + (processValue value)
    | StartsWith (key, value) -> "metadata." + key + " = " + (processValue value)
    | _ -> ""

  let private processPlanId planId =
    match planId with
    | Equals value -> processValue value
    | _ -> ""

  let rec private processFieldFilter fieldFilter =
    match fieldFilter with
    | ProjectId projectId -> "id = " + processPlanId projectId
    | PlanId planId -> "plan_id = " + processPlanId planId
    | Metadatum metadatum -> processMetadum metadatum
    | Relation (table, fieldFilter) -> table + "." + (processFieldFilter fieldFilter)

  let private processField field =
    let level = fst field
    let value = snd field
    match level with
    | 0 -> "p." + processFieldFilter value
    | _ -> string level + "." + processFieldFilter value

  let rec private processFilter filter =
    match filter with
    | Field field -> processField field
    | And (first, second) -> processFilter first + " and " + processFilter second
    | _ -> ""

  let private processFilterOption filterOption =
    match filterOption with
    | Some filter -> "where " + (processFilter filter)
    | None -> "where 1 = 1"

  let private processFieldInclusion fieldInclusion =
    match fieldInclusion with
    | Select fields -> String.concat ", " fields
    | _ -> ""

  let private processFieldInclusionLevel level =
    match level with
    | 0 -> "p."
    | _ -> string level + "."

  let private processInclusion inclusion =
    (inclusion
     |> List.map
      (function
        | (level, fieldInclusion) -> processFieldInclusionLevel level + processFieldInclusion fieldInclusion
      )
     |> String.concat " ") + " from projects as p"


  let transform (query: Query) =
    "select " + processInclusion query.Inclusion + " " +
    processFilterOption query.Filter