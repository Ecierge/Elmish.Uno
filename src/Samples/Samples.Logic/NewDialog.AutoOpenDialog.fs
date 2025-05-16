[<AutoOpen>]
module AutoOpenDialog

open Microsoft.UI.Xaml

let flip f b a = f a b

let map get set f a =
  a |> get |> f |> flip set a

[<RequireQualifiedAccess>]
module Bool =

  let toVisibilityCollapsed isVisible =
    match isVisible with
    | true  -> Visibility.Visible
    | false -> Visibility.Collapsed


[<AutoOpen>]
module InOutModule =

  [<RequireQualifiedAccess>]
  type InOut<'a, 'b> =
    | In of 'a
    | Out of 'b

  [<RequireQualifiedAccess>]
  module InOut =

    let cata f g msg =
      match msg with
      | InOut.In  msg -> msg |> f
      | InOut.Out msg -> msg |> g
