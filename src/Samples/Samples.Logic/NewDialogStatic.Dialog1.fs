namespace rec Elmish.Uno.Samples.NewDialogStatic.Dialog1

open Elmish.Uno
open FSharp.Core

type Dialog1 = string

type Dialog1Msg =
    | SetInput of string

module Program =

    let init () = ""

    let update msg (m: Dialog1) =
        match msg with
        | SetInput s -> s

module Bindings =

    let private viewModel = Unchecked.defaultof<Dialog1ViewModel>

    let inputBinding =
        BindingT.twoWay (id, SetInput) (nameof viewModel.Input)

#nowarn 3261 // Nullness warning
type Dialog1ViewModel(args) =
    inherit ViewModelBase<Dialog1, Dialog1Msg>(args)

    member _.Input = base.Get(Bindings.inputBinding)

    static member DesignInstance = Dialog1ViewModel (Unchecked.defaultof<_>)
