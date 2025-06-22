namespace rec Elmish.Uno.Samples.NewDialogStatic.Dialog2

open Elmish
open Elmish.Uno
open Microsoft.UI.Xaml
open FSharp.Core

[<RequireQualifiedAccess>]
type ConfirmState =
    | Submit
    | Cancel
    | Close

type Dialog2 = {
    Input: string
    IsChecked: bool
    ConfirmState: ConfirmState option
}

type Dialog2Msg =
    | SetInput of string
    | SetChecked of bool
    | Submit
    | Cancel
    | Close

[<RequireQualifiedAccess>]
type Dialog2OutMsg =
    | Close

module Input =
    let get (m: Dialog2) = m.Input
    let set v (m: Dialog2) = { m with Input = v }

module IsChecked =
    let get (m: Dialog2) = m.IsChecked
    let set v (m: Dialog2) = { m with IsChecked = v }

module ConfirmState =
    let get (m: Dialog2) = m.ConfirmState
    let set v (m: Dialog2) = { m with ConfirmState = v }

module Program =

    let init () = { Input = ""; IsChecked = false; ConfirmState = None }

    let update msg (m: Dialog2) =
        match msg with
        | SetInput s -> { m with Input = s }, Cmd.none
        | SetChecked b -> { m with IsChecked = b }, Cmd.none
        | Submit -> { m with ConfirmState = Some ConfirmState.Submit }, Cmd.ofMsg Dialog2OutMsg.Close
        | Cancel -> { m with ConfirmState = Some ConfirmState.Cancel }, Cmd.ofMsg Dialog2OutMsg.Close
        | Close -> { m with ConfirmState = Some ConfirmState.Close }, Cmd.ofMsg Dialog2OutMsg.Close

module Bindings =

    let private viewModel = Unchecked.defaultof<Dialog2ViewModel>

    let inputBinding =
        BindingT.twoWay(_.Input, SetInput) (nameof viewModel.Input)

    let isCheckedBinding =
        BindingT.twoWay (_.IsChecked, SetChecked) (nameof viewModel.IsChecked)

    let submitMsgVisibleBinding =
        BindingT.oneWay (fun m -> m.ConfirmState = Some ConfirmState.Submit)
        (nameof viewModel.SubmitMsgVisible)

    let cancelMsgVisibleBinding =
        BindingT.oneWay (fun m -> m.ConfirmState = Some ConfirmState.Cancel)
        (nameof viewModel.CancelMsgVisible)

    let closeMsgVisibleBinding =
        BindingT.oneWay (fun m -> m.ConfirmState = Some ConfirmState.Close)
        (nameof viewModel.CloseMsgVisible)

    let submitCommandBinding =
        BindingT.cmd Submit (nameof viewModel.SubmitCommand)

    let cancelCommandBinding =
        BindingT.cmd Cancel (nameof viewModel.CancelCommand)

    let closeCommandBinding =
        BindingT.cmd Close (nameof viewModel.CloseCommand)

type Dialog2ViewModel(args) =
    inherit ViewModelBase<Dialog2, Dialog2Msg>(args)

    member _.Input = base.Get(Bindings.inputBinding)
    member _.IsChecked = base.Get(Bindings.isCheckedBinding)
    member _.SubmitMsgVisible = base.Get(Bindings.submitMsgVisibleBinding)
    member _.CancelMsgVisible = base.Get(Bindings.cancelMsgVisibleBinding)
    member _.CloseMsgVisible = base.Get(Bindings.closeMsgVisibleBinding)
    member _.SubmitCommand = base.Get(Bindings.submitCommandBinding)
    member _.CancelCommand = base.Get(Bindings.cancelCommandBinding)
    member _.CloseCommand = base.Get(Bindings.closeCommandBinding)

    static member DesignInstance = Dialog2ViewModel (Unchecked.defaultof<_>)
