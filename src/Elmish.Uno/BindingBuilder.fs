namespace Elmish.Uno

open System.Reflection
open Microsoft.FSharp.Core

type BindingBuilder<'model, 'msg> () =

  static let bindingType = typeof<Binding<'model, 'msg>>
  static let bindingCreatorType = typeof<string -> Binding<'model, 'msg>>

  member this.GetBindings () =
    let getBinding (propertyInfo : PropertyInfo) =
      match propertyInfo.PropertyType with
      | propertyType when bindingType.IsAssignableFrom (propertyType) ->
        Some (propertyInfo.GetValue this |> nonNull :?> Binding<'model, 'msg>)
      | propertyType when propertyType = bindingCreatorType ->
        let bindingCreator = propertyInfo.GetValue this |> nonNull :?> (string -> Binding<'model, 'msg>)
        Some (bindingCreator propertyInfo.Name)
      | _ -> None

    let t = this.GetType ()
    t.GetProperties (BindingFlags.Instance ||| BindingFlags.Public)
    |> Seq.filter (fun p -> p.CanRead && not p.CanWrite)
    |> Seq.map getBinding
    |> Seq.choose id
    |> Seq.toList
