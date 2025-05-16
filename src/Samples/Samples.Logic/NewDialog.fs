module Elmish.Uno.Samples.NewDialog.AppModule

open System
open Elmish
open Elmish.Uno
open Microsoft.UI.Xaml
open Microsoft.UI.Xaml.Controls

open Dialog1Module
open Dialog2Module


type App =
  { Dialog1: WindowState<string>
    Dialog2: Dialog2 option }

type AppMsg =
  | Dialog1Show
  | Dialog1Close
  | Dialog1SetInput of string
  | Dialog2Show
  | Dialog2Close
  | Dialog2Msg of Dialog2Msg


module App =
  module Dialog1 =
    let get m = m.Dialog1
    let set v m = { m with Dialog1 = v }
    let map = map get set
  module Dialog2 =
    let get m = m.Dialog2
    let set v m = { m with Dialog2 = v }
    let map = map get set
    let mapOutMsg msg =
      match msg with
      | Dialog2OutMsg.Close -> Dialog2Close
    let mapInOutMsg = InOut.cata Dialog2Msg mapOutMsg

  let initial =
    { Dialog1 = WindowState.Closed
      Dialog2 = None }

  let update msg =
    match msg with
    | Dialog1Show -> "" |> WindowState.toVisible |> Dialog1.map
    | Dialog1Close -> WindowState.Closed |> Dialog1.set
    | Dialog1SetInput s -> s |> WindowState.set |> Dialog1.map
    | Dialog2Show -> Dialog2.init |> Some |> Dialog2.set
    | Dialog2Close -> None |> Dialog2.set
    | Dialog2Msg msg -> msg |> Dialog2.update |> Option.map |> Dialog2.map

  let bindings (createDialog1: unit -> ContentDialog, createDialog2: unit -> ContentDialog) = [
    "Dialog1Show" |> Binding.cmd Dialog1Show
    "Dialog1Close" |> Binding.cmd Dialog1Close
    "Dialog2Show" |> Binding.cmd Dialog2Show
    "Dialog1" |> Binding.subModelDialog(
      (fun m -> m.Dialog1),
      snd,
      id,
      (Dialog1.bindings |> Bindings.mapMsg Dialog1SetInput),
      createDialog1,
      Dialog1Close)
    "Dialog2" |> Binding.subModelDialog(
      (Dialog2.get >> WindowState.ofOption),
      snd,
      Dialog2.mapInOutMsg,
      Dialog2.bindings,
      createDialog2)
  ]

open App

let private fail _ = failwith "never called"
[<CompiledName("DesignInstance")>]
let designInstance = ViewModel.designInstance initial (bindings (fail, fail))

[<CompiledName("CreateProgram")>]
let createProgram (createDialog1 : Func<ContentDialog>, createDialog2 : Func<ContentDialog>) =
  let bindings = bindings ((fun () -> createDialog1.Invoke ()), (fun () -> createDialog2.Invoke ()))
  UnoProgram.mkSimple (fun () -> initial) update bindings
