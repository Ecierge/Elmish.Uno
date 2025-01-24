module Elmish.Uno.Samples.OneWaySeq.Program

open System
open Serilog
open Serilog.Extensions.Logging
open Elmish
open Elmish.Uno
open FSharp.Collections.Immutable
open Microsoft.Extensions.Logging


type Model =
  { OneWaySeqNumbers: int list
    OneWayNumbers: int list
    IncrementalLoadingNumbers: int FlatList
    IsLoading: bool }

type Msg =
  | AddOneWaySeqNumber
  | AddOneWayNumber
  | LoadMore of count: uint * complete: (uint -> unit)
  | LoadedMore of allItems: FlatList<int>
  | ErrorLoadingMore

let asyncLoadItems (complete: uint -> unit) (items: FlatList<int>) count = async {
  try
    try
      let intCount = int count
      let builder = items.ToBuilder()
      let max = FlatList.last items
      for i = max + 1 to max + intCount do
        builder.Add(i)
      return LoadedMore <| builder.ToImmutable ()
    with _ ->
      return ErrorLoadingMore
  finally
    complete count
}

let initial =
  { OneWaySeqNumbers = [ 1000..-1..1 ]
    OneWayNumbers = [ 1000..-1..1 ]
    IncrementalLoadingNumbers = [ 1..1..10 ] |> FlatList.ofSeq
    IsLoading = false  }

let init () = initial, Cmd.ofMsg AddOneWaySeqNumber

let update msg m =
  match msg with
  | AddOneWaySeqNumber -> { m with OneWaySeqNumbers = m.OneWaySeqNumbers.Head + 1 :: m.OneWaySeqNumbers }, Cmd.none
  | AddOneWayNumber -> { m with OneWayNumbers = m.OneWayNumbers.Head + 1 :: m.OneWayNumbers }, Cmd.none
  | LoadMore (count, complete) ->
                        // If error hapened earlier it may be worth to do someting with that instead
                        { m with IsLoading = true }, // Display some hint (indetermined progress bar)
                        Cmd.OfAsync.perform (asyncLoadItems complete m.IncrementalLoadingNumbers) count id
  | LoadedMore items -> { m with
                            IsLoading = false // Hide loading indicator
                            IncrementalLoadingNumbers = items }, Cmd.none
  | ErrorLoadingMore -> m, Cmd.none // add error to model that will appear on UI

[<CompiledName "Bindings">]
let bindings : Binding<Model, Msg> list = [
  "OneWaySeqNumbers" |> Binding.oneWaySeq (_.OneWaySeqNumbers, (=), id)
  "IncrementalLoadingNumbers" |> Binding.oneWaySeq ((fun m -> m.IncrementalLoadingNumbers : _ seq), (=), id, (fun _ -> true), LoadMore)
  "OneWayNumbers" |> Binding.oneWay _.OneWayNumbers
  "AddOneWaySeqNumber" |> Binding.cmd AddOneWaySeqNumber
  "AddOneWayNumber" |> Binding.cmd AddOneWayNumber
]

[<CompiledName("DesignInstance")>]
let designInstance = ViewModel.designInstance initial bindings

[<CompiledName("Program")>]
let program =
  UnoProgram.mkProgram init update bindings
  |> UnoProgram.withLogger (new SerilogLoggerFactory(logger))

type ViewModel(dispatcher) as vm =
  inherit DynamicViewModel<Model, Msg>(
    UnoProgram.createVmArgs dispatcher (Func<_>(fun () -> vm)) program,
    bindings
  )
