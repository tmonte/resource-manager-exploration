namespace ResourceManagerExploration

open Logic
open Query

module Main =

  printfn "*** All Priorities for a 1800contacts Plan ***"

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

  // Alternative syntax
  let priorities1800contactsBuilder: Query =
    QueryBuilder()
    |> (Where((0, PlanId(Equals(String "1800contacts"))))
        |> And(0, Metadatum(Equals("Tag", String "Priority")))
        |> Or(0, Relation("Technologies", Metadatum(StartsWith("Name", String "Cloud")))))
    |> Include(0, Everything)
    |> Include(0, Related("Technologies", Everything))
    |> SortBy(0, Ascending("Name"))


  printfn "%A" priorities1800contacts
