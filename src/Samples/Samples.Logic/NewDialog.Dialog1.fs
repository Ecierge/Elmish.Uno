module Elmish.Uno.Samples.NewDialog.Dialog1Module

open Elmish.Uno

module Dialog1 =
  let init = ""

  let bindings = [
    "Input" |> Binding.twoWay (id, id)
  ]

[<CompiledName("DesignInstance")>]
let designInstance = ViewModel.designInstance Dialog1.init Dialog1.bindings
