namespace ResourceManagerExploration

open Logic
open Query

module rec Process =

  let private processValue value =
    match value with
    | String stringValue -> stringValue
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

  let rec private processFieldFilter (fieldFilter: FieldFilter) =
    match fieldFilter with
    | PlanId planId -> "planId = " + processPlanId planId
    | Metadatum metadatum -> processMetadum metadatum
    | Relation (table, fieldFilter) -> table + "." + (processFieldFilter fieldFilter)
    | _ -> ""

  let rec private processFilter (filter: Logic<int * FieldFilter>) =
    match filter with
    | Field field -> string (fst field) + "." + processFieldFilter (snd field)
    | And (first, second) -> processFilter first + " and " + processFilter second
    | _ -> ""

  let private processFilterOption (filterOption: Filter) =
    match filterOption with
    | Some filter -> "where " + (processFilter filter)
    | None -> "where 1 = 1"
    
  let transform (query: Query) =
    processFilterOption query.Filter