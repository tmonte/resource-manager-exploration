namespace ResourceManagerExploration

open Logic

module rec Query =

  type FieldFilter =
    | PlanId of FieldOperation<Value>
    | ProjectId of FieldOperation<Value>
    | Metadatum of FieldOperation<Pair<string, Value>>
    | Relation of string * FieldFilter

  type Filter = Logic<int * FieldFilter> option

  type FieldInclusion =
    | Everything
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

  
  
