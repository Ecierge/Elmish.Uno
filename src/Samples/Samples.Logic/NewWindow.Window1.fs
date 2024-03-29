module Elmish.Uno.Samples.NewWindow.Window1Module

open Elmish.Uno


module Window1 =
  let init = ""

  let bindings = [
    "Input" |> Binding.twoWay (id, id)
  ]

[<CompiledName("DesignInstance")>]
let designInstance = ViewModel.designInstance Window1.init Window1.bindings