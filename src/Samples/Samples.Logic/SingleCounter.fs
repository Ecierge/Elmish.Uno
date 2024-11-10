﻿module Elmish.Uno.Samples.SingleCounter.Program

open System
open Serilog
open Serilog.Extensions.Logging
open Elmish
open Elmish.Uno

type Model =
  { Count: int
    StepSize: int }

type Msg =
  | Increment
  | Decrement
  | SetStepSize of int
  | Reset

let initial =
  { Count = 0
    StepSize = 1 }

let init parameter = { initial with Count = parameter }

let canReset = (<>) initial

let update msg m =
  match msg with
  | Increment -> { m with Count = m.Count + m.StepSize }
  | Decrement -> { m with Count = m.Count - m.StepSize }
  | SetStepSize x -> { m with StepSize = x }
  | Reset -> initial

[<CompiledName "Bindings">]
let bindings : Binding<Model, Msg> list = [
  "CounterValue" |> Binding.oneWay (fun m -> m.Count)
  "Increment" |> Binding.cmd Increment
  "Decrement" |> Binding.cmd Decrement
  "StepSize" |> Binding.twoWay(
    (fun m -> float m.StepSize),
    int >> SetStepSize)
  "Reset" |> Binding.cmdIf(Reset, canReset)
]

[<CompiledName("DesignInstance")>]
let designInstance = ViewModel.designInstance initial bindings

[<CompiledName("Program")>]
let program =
  UnoProgram.mkSimple init update bindings
  |> UnoProgram.withLogger (new SerilogLoggerFactory(logger))

