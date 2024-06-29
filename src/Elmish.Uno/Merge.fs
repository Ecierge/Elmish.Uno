﻿[<AutoOpen>]
module internal Elmish.Uno.Merge

open System.Collections.Generic
open System.Collections.ObjectModel
open Elmish.ObservableLookup

type SourceOrTarget =
  | Source
  | Target

type DuplicateIdException (sourceOrTarget: SourceOrTarget, index1: int, index2: int, id: string) =
  inherit System.Exception(sprintf "In the %A sequence, the elements at indices %d and %d have the same ID %s" sourceOrTarget index1 index2 id)
  member this.SourceOrTarget = sourceOrTarget
  member this.Index1 = index1
  member this.Index2 = index2
  member this.Id = id

type CollectionTarget<'T, 'aCollection> =
  { GetLength: unit -> int
    GetAt: int -> 'T
    Append: 'T -> unit
    InsertAt: int * 'T -> unit
    SetAt: int * 'T -> unit
    RemoveAt: int -> unit
    Move: int * int -> unit
    Clear: unit -> unit
    Enumerate: unit -> 'T seq
    GetCollection: unit -> 'aCollection }

type GrouppedCollectionTarget<'T, 'aCollection, 'key> =
  { GetLength: unit -> int
    GetKey: 'T -> 'key
    GetAt: int -> 'T
    Append: 'key -> 'T -> unit
    InsertAt: 'key -> int * 'T -> unit
    SetAt: int * 'T -> unit
    RemoveAt: int -> unit
    Move: 'key -> int * int -> unit
    Clear: unit -> unit
    Enumerate: unit -> 'T seq
    GetCollection: unit -> 'aCollection }

module CollectionTarget =

  let create (oc: ObservableCollection<'T>) =
    { GetLength = fun () -> oc.Count
      GetAt = fun i -> oc.[i]
      Append = oc.Add
      InsertAt = oc.Insert
      SetAt = fun (i, a) -> oc.[i] <- a
      RemoveAt = oc.RemoveAt
      Move = oc.Move
      Clear = oc.Clear
      Enumerate = fun () -> upcast oc
      GetCollection = fun () -> oc }

  let mapA (fOut: 'a0 -> 'a1) (fIn: 'a1 -> 'a0) (ct: CollectionTarget<'a0, 'aCollection>) : CollectionTarget<'a1, 'aCollection> =
    { GetLength = ct.GetLength
      GetAt = ct.GetAt >> fOut
      Append = fIn >> ct.Append
      InsertAt = Pair.map2 fIn >> ct.InsertAt
      SetAt = Pair.map2 fIn >> ct.SetAt
      RemoveAt = ct.RemoveAt
      Move = ct.Move
      Clear = ct.Clear
      Enumerate = ct.Enumerate >> Seq.map fOut
      GetCollection = ct.GetCollection }

  let mapCollection (fOut: 'aCollection0 -> 'aCollection1) (ct: CollectionTarget<'T, 'aCollection0>) : CollectionTarget<'T, 'aCollection1> =
    { GetLength = ct.GetLength
      GetAt = ct.GetAt
      Append = ct.Append
      InsertAt = ct.InsertAt
      SetAt = ct.SetAt
      RemoveAt = ct.RemoveAt
      Move = ct.Move
      Clear = ct.Clear
      Enumerate = ct.Enumerate
      GetCollection = ct.GetCollection >> fOut }


module GrouppedCollectionTarget =

  let create (oc: ObservableLookup<'TKey, 'TValue>) =
    { GetLength = fun () -> oc.Count
      GetAt = fun i -> oc.[i]
      Append = oc.Add
      InsertAt = oc.Insert
      SetAt = fun (i, a) -> oc.[i] <- a
      RemoveAt = oc.RemoveAt
      Move = oc.Move
      Clear = oc.Clear
      Enumerate = fun () -> upcast oc
      GetCollection = fun () -> oc }

  let mapA (fOut: 'a0 -> 'a1) (fIn: 'a1 -> 'a0) (kOut: 'key0 -> 'key1) (ct: GrouppedCollectionTarget<'a0, 'aCollection, 'key0>) : GrouppedCollectionTarget<'a1, 'aCollection, 'key1> =
    { GetLength = ct.GetLength
      GetKey = fIn >> ct.GetKey >> kOut
      GetAt = ct.GetAt >> fOut
      Append = fIn >> ct.Append
      InsertAt = Pair.map2 fIn >> ct.InsertAt
      SetAt = Pair.map2 fIn >> ct.SetAt
      RemoveAt = ct.RemoveAt
      Move = ct.Move
      Clear = ct.Clear
      Enumerate = ct.Enumerate >> Seq.map fOut
      GetCollection = ct.GetCollection }

  let mapCollection (fOut: 'aCollection0 -> 'aCollection1) (ct: GrouppedCollectionTarget<'T, 'aCollection0, 'key0>) : GrouppedCollectionTarget<'T, 'aCollection1, 'key0> =
    { GetLength = ct.GetLength
      GetKey = ct.GetKey
      GetAt = ct.GetAt
      Append = ct.Append
      InsertAt = ct.InsertAt
      SetAt = ct.SetAt
      RemoveAt = ct.RemoveAt
      Move = ct.Move
      Clear = ct.Clear
      Enumerate = ct.Enumerate
      GetCollection = ct.GetCollection >> fOut }

  let mapKey (fOut: 'key0 -> 'key1) (ct: GrouppedCollectionTarget<'T, 'aCollection0, 'key0>) : GrouppedCollectionTarget<'T, 'aCollection0, 'key1> =
    { GetLength = ct.GetLength
      GetKey = ct.GetKey >> fOut
      GetAt = ct.GetAt
      Append = ct.Append
      InsertAt = ct.InsertAt
      SetAt = ct.SetAt
      RemoveAt = ct.RemoveAt
      Move = ct.Move
      Clear = ct.Clear
      Enumerate = ct.Enumerate
      GetCollection = ct.GetCollection }



module Merge =

  let unkeyed
      (create: 's -> int -> 't)
      (update: 't -> 's -> unit)
      (target: CollectionTarget<'t, 'tCollection>)
      (source: 's seq) =
    let mutable lastIdx = -1
    for (idx, s) in source |> Seq.indexed do
      lastIdx <- idx
      if idx < target.GetLength() then
        update (target.GetAt idx) s
      else // source is longer than target
        create s idx |> target.Append
    let mutable idx = target.GetLength() - 1
    while idx > lastIdx do // target is longer than source
      target.RemoveAt idx
      idx <- idx - 1


  let keyed
      (getSourceId: 's -> 'id)
      (getTargetId: 't -> 'id)
      (create: 's -> 'id -> 't)
      (update: 't -> 's -> int -> unit)
      (target: CollectionTarget<'t, 'tCollection>)
      (source: 's array) =
    (*
     * Based on Elm's HTML.keyed
     * https://guide.elm-lang.org/optimization/keyed.html
     * https://github.com/elm/virtual-dom/blob/5a5bcf48720bc7d53461b3cd42a9f19f119c5503/src/Elm/Kernel/VirtualDom.js#L980-L1226
     *)
    let removals = Dictionary<_, _> ()
    let additions = Dictionary<_, _> ()

    let recordRemoval curTargetIdx curTarget curTargetId =
      if removals.ContainsKey curTargetId then
        let struct (firstIdx, _) = removals.[curTargetId]
        raise (DuplicateIdException (Target, firstIdx, curTargetIdx, curTargetId.ToString()))
      else
        removals.Add(curTargetId, (curTargetIdx, curTarget))
    let recordAddition curSourceIdx curSource curSourceId =
      if additions.ContainsKey curSourceId then
        let struct (firstIdx, _) = additions.[curSourceId]
        raise (DuplicateIdException (Source, firstIdx, curSourceIdx, curSourceId.ToString()))
      else
        additions.Add(curSourceId, (curSourceIdx, curSource))

    let mutable curSourceIdx = 0
    let mutable curTargetIdx = 0

    let mutable shouldContinue = true

    let sourceCount = source.Length
    let targetCount = target.GetLength()

    while (shouldContinue && curSourceIdx < sourceCount && curTargetIdx < targetCount) do
      let curSource = source.[curSourceIdx]
      let curTarget = target.GetAt curTargetIdx

      let curSourceId = getSourceId curSource
      let curTargetId = getTargetId curTarget

      if curSourceId = curTargetId then
        update curTarget curSource curTargetIdx

        curSourceIdx <- curSourceIdx + 1
        curTargetIdx <- curTargetIdx + 1
      else
        let mNextSource =
          source
          |> Array.tryItem (curSourceIdx + 1)
          |> Option.map (fun s ->
            let id = getSourceId s
            s, id, id = curTargetId) // true => need to add

        let mNextTarget =
          if curTargetIdx + 1 < targetCount then target.GetAt (curTargetIdx + 1) |> Some else None
          |> Option.map (fun t ->
            let id = getTargetId t
            t, id, id = curSourceId) // true => need to remove

        match mNextSource, mNextTarget with
        | Some (nextSource, _, true), Some (nextTarget, _, true) -> // swap adjacent
            target.SetAt (curTargetIdx, nextTarget)
            target.SetAt (curTargetIdx + 1, curTarget)

            update curTarget nextSource (curTargetIdx + 1)
            update nextTarget curSource curTargetIdx

            curSourceIdx <- curSourceIdx + 2
            curTargetIdx <- curTargetIdx + 2
        |               None, Some (nextTarget, _, true)
        | Some (_, _, false), Some (nextTarget, _, true) -> // remove
            recordRemoval curTargetIdx curTarget curTargetId

            update nextTarget curSource curTargetIdx

            curSourceIdx <- curSourceIdx + 1
            curTargetIdx <- curTargetIdx + 2
        | Some (nextSource, _, true), None
        | Some (nextSource, _, true), Some (_, _, false) -> // add
            recordAddition curSourceIdx curSource curSourceId

            update curTarget nextSource (curTargetIdx + 1)

            curSourceIdx <- curSourceIdx + 2
            curTargetIdx <- curTargetIdx + 1
        | Some (_, _, false),               None
        |               None, Some (_, _, false)
        |               None,               None -> // source and target have different lengths and we have reached the end of one
            shouldContinue <- false
        | Some (nextSource, nextSourceId, false), Some (nextTarget, nextTargetId, false) ->
            if nextSourceId = nextTargetId then // replace
              recordRemoval curTargetIdx curTarget curTargetId
              recordAddition curSourceIdx curSource curSourceId

              update nextTarget nextSource (curTargetIdx + 1)

              curSourceIdx <- curSourceIdx + 2
              curTargetIdx <- curTargetIdx + 2
            else // collections very different
              shouldContinue <- false

    // replace many
    while (curSourceIdx < sourceCount && curTargetIdx < targetCount) do
      let curSource = source.[curSourceIdx]
      let curTarget = target.GetAt curTargetIdx

      let curSourceId = getSourceId curSource
      let curTargetId = getTargetId curTarget

      recordRemoval curTargetIdx curTarget curTargetId
      recordAddition curSourceIdx curSource curSourceId

      curSourceIdx <- curSourceIdx + 1
      curTargetIdx <- curTargetIdx + 1

    // remove many
    for i in targetCount - 1..-1..curTargetIdx do
      let t = target.GetAt i
      let id = getTargetId t
      recordRemoval i t id

    // add many
    for i in curSourceIdx..sourceCount - 1 do
      let s = source.[i]
      let id = getSourceId s
      recordAddition i s id

    let moves =
      additions
      |> Seq.toList // make copy of additions so that calling Remove doesn't happen on the same data structure while enumerating
      |> List.collect (fun (Kvp (id, struct (sIdx, s))) ->
        removals
        |> Dictionary.tryFind id
        |> ValueOption.map (fun struct (tIdx, t) ->
            removals.Remove id |> ignore
            additions.Remove id |> ignore
            (tIdx, sIdx, t, s) |> List.singleton)
        |> ValueOption.defaultValue [])

    let actuallyRemove () =
      Seq.empty
      |> Seq.append (removals |> Seq.map (Kvp.value >> fstv))
      |> Seq.append (moves |> Seq.map (fun (tIdx, _, _, _) -> tIdx))
      |> Seq.sortDescending // remove by index from largest to smallest
      |> Seq.iter target.RemoveAt

    let actuallyAdd () =
      Seq.empty
      |> Seq.append (additions |> Seq.map (fun (Kvp (id, (idx, s))) -> idx, create s id))
      |> Seq.append (moves |> Seq.map (fun (_, sIdx, t, _) -> sIdx, t))
      |> Seq.sortBy fst // insert by index from smallest to largest
      |> Seq.iter target.InsertAt

    match moves, removals.Count, additions.Count with
    | [ (tIdx, sIdx, _, _) ], 0, 0 -> // single move
        target.Move(tIdx, sIdx)
    | [ (t1Idx, s1Idx, _, _); (t2Idx, s2Idx, _, _) ], 0, 0 when t1Idx = s2Idx && t2Idx = s1Idx-> // single swap
        let temp = target.GetAt t1Idx
        target.SetAt (t1Idx, target.GetAt t2Idx)
        target.SetAt (t2Idx, temp)
    | _, rc, _ when rc = targetCount && rc > 0 -> // remove everything (implies moves = [])
        target.Clear ()
        actuallyAdd ()
    | _ ->
        actuallyRemove ()
        actuallyAdd ()

    // update moved elements
    moves |> Seq.iter (fun (_, sIdx, t, s) -> update t s sIdx)

  let groupped
      (getSourceKey: 's -> 'key)
      (getTargetKey: 't -> 'key)
      (getSourceId: 's -> 'id)
      (getTargetId: 't -> 'id)
      (create: 's -> 'id -> 't)
      (update: 't -> 's -> int -> unit)
      (target: GrouppedCollectionTarget<'t, 'tCollection, 'key>)
      (source: 's array) =

    let removals = Dictionary<_, _> ()
    let additions = Dictionary<_, _> ()

    let recordRemoval curTargetIdx curTarget curTargetId =
      if removals.ContainsKey curTargetId then
        let struct (firstIdx, _) = removals.[curTargetId]
        raise (DuplicateIdException (Target, firstIdx, curTargetIdx, curTargetId.ToString()))
      else
        removals.Add(curTargetId, (curTargetIdx, curTarget))
    let recordAddition curSourceIdx curSource curSourceId =
      if additions.ContainsKey curSourceId then
        let struct (firstIdx, _) = additions.[curSourceId]
        raise (DuplicateIdException (Source, firstIdx, curSourceIdx, curSourceId.ToString()))
      else
        additions.Add(curSourceId, (curSourceIdx, curSource))

    let mutable curSourceIdx = 0
    let mutable curTargetIdx = 0

    let mutable shouldContinue = true

    let sourceCount = source.Length
    let targetCount = target.GetLength()

    while (shouldContinue && curSourceIdx < sourceCount && curTargetIdx < targetCount) do
      let curSource = source.[curSourceIdx]
      let curTarget = target.GetAt curTargetIdx

      let curSourceId = getSourceId curSource
      let curTargetId = getTargetId curTarget

      if curSourceId = curTargetId then
        update curTarget curSource curTargetIdx

        curSourceIdx <- curSourceIdx + 1
        curTargetIdx <- curTargetIdx + 1
      else
        let mNextSource =
          source
          |> Array.tryItem (curSourceIdx + 1)
          |> Option.map (fun s ->
            let id = getSourceId s
            s, id, id = curTargetId) // true => need to add

        let mNextTarget =
          if curTargetIdx + 1 < targetCount then target.GetAt (curTargetIdx + 1) |> Some else None
          |> Option.map (fun t ->
            let id = getTargetId t
            t, id, id = curSourceId) // true => need to remove

        match mNextSource, mNextTarget with
        | Some (nextSource, _, true), Some (nextTarget, _, true) -> // swap adjacent
            target.SetAt (curTargetIdx, nextTarget)
            target.SetAt (curTargetIdx + 1, curTarget)

            update curTarget nextSource (curTargetIdx + 1)
            update nextTarget curSource curTargetIdx

            curSourceIdx <- curSourceIdx + 2
            curTargetIdx <- curTargetIdx + 2
        |               None, Some (nextTarget, _, true)
        | Some (_, _, false), Some (nextTarget, _, true) -> // remove
            recordRemoval curTargetIdx curTarget curTargetId

            update nextTarget curSource curTargetIdx

            curSourceIdx <- curSourceIdx + 1
            curTargetIdx <- curTargetIdx + 2
        | Some (nextSource, _, true), None
        | Some (nextSource, _, true), Some (_, _, false) -> // add
            recordAddition curSourceIdx curSource curSourceId

            update curTarget nextSource (curTargetIdx + 1)

            curSourceIdx <- curSourceIdx + 2
            curTargetIdx <- curTargetIdx + 1
        | Some (_, _, false),               None
        |               None, Some (_, _, false)
        |               None,               None -> // source and target have different lengths and we have reached the end of one
            shouldContinue <- false
        | Some (nextSource, nextSourceId, false), Some (nextTarget, nextTargetId, false) ->
            if nextSourceId = nextTargetId then // replace
              recordRemoval curTargetIdx curTarget curTargetId
              recordAddition curSourceIdx curSource curSourceId

              update nextTarget nextSource (curTargetIdx + 1)

              curSourceIdx <- curSourceIdx + 2
              curTargetIdx <- curTargetIdx + 2
            else // collections very different
              shouldContinue <- false

    // replace many
    while (curSourceIdx < sourceCount && curTargetIdx < targetCount) do
      let curSource = source.[curSourceIdx]
      let curTarget = target.GetAt curTargetIdx

      let curSourceId = getSourceId curSource
      let curTargetId = getTargetId curTarget

      recordRemoval curTargetIdx curTarget curTargetId
      recordAddition curSourceIdx curSource curSourceId

      curSourceIdx <- curSourceIdx + 1
      curTargetIdx <- curTargetIdx + 1

    // remove many
    for i in targetCount - 1..-1..curTargetIdx do
      let t = target.GetAt i
      let id = getTargetId t
      recordRemoval i t id

    // add many
    for i in curSourceIdx..sourceCount - 1 do
      let s = source.[i]
      let id = getSourceId s
      recordAddition i s id

    let moves =
      additions
      |> Seq.toList // make copy of additions so that calling Remove doesn't happen on the same data structure while enumerating
      |> List.collect (fun (Kvp (id, (sIdx, s))) ->
        removals
        |> Dictionary.tryFind id
        |> ValueOption.map (fun struct (tIdx, t) ->
            removals.Remove id |> ignore
            additions.Remove id |> ignore
            (tIdx, sIdx, t, s) |> List.singleton)
        |> ValueOption.defaultValue [])

    let actuallyRemove () =
      Seq.empty
      |> Seq.append (removals |> Seq.map (Kvp.value >> fstv))
      |> Seq.append (moves |> Seq.map (fun (tIdx, _, _, _) -> tIdx))
      |> Seq.sortDescending // remove by index from largest to smallest
      |> Seq.iter target.RemoveAt

    let actuallyAdd () =
      Seq.empty
      |> Seq.append (additions |> Seq.map (fun (Kvp (id, struct (idx, s))) -> idx, create s id))
      |> Seq.append (moves |> Seq.map (fun (_, sIdx, t, _) -> sIdx, t))
      |> Seq.sortBy fst // insert by index from smallest to largest
      |> Seq.iter target.InsertAt

    match moves, removals.Count, additions.Count with
    | [ (tIdx, sIdx, _, _) ], 0, 0 -> // single move
        target.Move(tIdx, sIdx)
    | [ (t1Idx, s1Idx, _, _); (t2Idx, s2Idx, _, _) ], 0, 0 when t1Idx = s2Idx && t2Idx = s1Idx-> // single swap
        let temp = target.GetAt t1Idx
        target.SetAt (t1Idx, target.GetAt t2Idx)
        target.SetAt (t2Idx, temp)
    | _, rc, _ when rc = targetCount && rc > 0 -> // remove everything (implies moves = [])
        target.Clear ()
        actuallyAdd ()
    | _ ->
        actuallyRemove ()
        actuallyAdd ()

    // update moved elements
    moves |> Seq.iter (fun (_, sIdx, t, s) -> update t s sIdx)
