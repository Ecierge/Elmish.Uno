module Elmish.Uno.Samples.NewDialog.Dialog2Module

open Elmish.Uno


[<RequireQualifiedAccess>]
type ConfirmState =
  | Submit
  | Cancel
  | Close

type Dialog2 =
  { Input: string
    IsChecked: bool
    ConfirmState: ConfirmState option }

type Dialog2Msg =
  | SetInput of string
  | SetChecked of bool
  | Submit
  | Cancel
  | Close

[<RequireQualifiedAccess>]
type Dialog2OutMsg =
  | Close


module Dialog2 =
  module Input =
    let get m = m.Input
    let set v m = { m with Input = v }
  module IsChecked =
    let get m = m.IsChecked
    let set v m = { m with IsChecked = v }
  module ConfirmState =
    let set v m = { m with ConfirmState = v }

  let init =
    { Input = ""
      IsChecked = false
      ConfirmState = None }

  let update msg =
    match msg with
    | SetInput s -> s |> Input.set
    | SetChecked b -> b |> IsChecked.set
    | Submit -> ConfirmState.Submit |> Some |> ConfirmState.set
    | Cancel -> ConfirmState.Cancel |> Some |> ConfirmState.set
    | Close  -> ConfirmState.Close  |> Some |> ConfirmState.set

  let private confirmStateVisibilityBinding confirmState =
    fun m -> m.ConfirmState = Some confirmState
    >> Bool.toVisibilityCollapsed
    |> Binding.oneWay

  let private confirmStateToMsg confirmState msg m =
    if m.ConfirmState = Some confirmState
    then InOut.Out Dialog2OutMsg.Close
    else InOut.In msg

  let bindings =
    let inBindings =
      [ "Input" |> Binding.twoWay (Input.get, SetInput)
        "IsChecked" |> Binding.twoWay (IsChecked.get, SetChecked)
        "SubmitMsgVisible" |> confirmStateVisibilityBinding ConfirmState.Submit
        "CancelMsgVisible" |> confirmStateVisibilityBinding ConfirmState.Cancel
        "CloseMsgVisible"  |> confirmStateVisibilityBinding ConfirmState.Close ]
      |> Bindings.mapMsg InOut.In
    let inOutBindings =
      [ "Submit" |> Binding.cmd (confirmStateToMsg ConfirmState.Submit Submit)
        "Cancel" |> Binding.cmd (confirmStateToMsg ConfirmState.Cancel Cancel)
        "Close"  |> Binding.cmd (confirmStateToMsg ConfirmState.Close  Close) ]
    inBindings @ inOutBindings

[<CompiledName("DesignInstance")>]
let designInstance = ViewModel.designInstance Dialog2.init (Dialog2.bindings)
