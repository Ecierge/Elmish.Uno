namespace Elmish.Uno

open Elmish
open Elmish.Collections
open System
open System.Collections.Generic
open System.Collections.ObjectModel
open System.Collections.Specialized
open System.Runtime.InteropServices
open System.Windows.Input
open Microsoft.UI.Xaml

[<AbstractClass; Sealed>]
type BindingT =

  /// <summary>
  ///   Creates a binding intended for use with <code>Selector.SelectedIndex</code>.
  /// </summary>
  /// <param name="get">Gets the selected index from the model.</param>
  /// <param name="set">Returns the message to dispatch.</param>
  static member selectedIndex:
    get: ('model -> int voption) * set: (int voption -> 'msg) -> (string -> Binding<'model, 'msg, int>)

  /// <summary>
  ///   Creates a binding intended for use with <code>Selector.SelectedIndex</code>.
  /// </summary>
  /// <param name="get">Gets the selected index from the model.</param>
  /// <param name="set">Returns the message to dispatch.</param>
  static member selectedIndex:
    get: ('model -> int option) * set: (int option -> 'msg) -> (string -> Binding<'model, 'msg, int>)

  /// <summary>Creates a one-way binding.</summary>
  /// <param name="get">Gets the value from the model.</param>
  static member oneWay: get: ('model -> 'a) -> (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a one-way binding to an optional value. The binding
  ///   automatically converts between the optional source value and an
  ///   unwrapped (possibly <c>null</c>) value on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  static member oneWayOpt: get: ('model -> 'a voption) -> (string -> Binding<'model, 'msg, Nullable<'a>>)
  /// <summary>
  ///   Creates a one-way binding to an optional value. The binding
  ///   automatically converts between the optional source value and an
  ///   unwrapped (possibly <c>null</c>) value on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  static member oneWayOpt: get: ('model -> 'a option) -> (string -> Binding<'model, 'msg, Nullable<'a>>)

  /// <summary>
  ///   Creates a one-way binding to an optional value. The binding
  ///   automatically converts between the optional source value and an
  ///   unwrapped (possibly <c>null</c>) value on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  static member oneWayOptObj: get: ('model -> 'a option) -> (string -> Binding<'model, 'msg, 'a>)
  /// <summary>
  ///   Creates a one-way binding to an optional value. The binding
  ///   automatically converts between the optional source value and an
  ///   unwrapped (possibly <c>null</c>) value on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  static member oneWayOptObj: get: ('model -> 'a voption) -> (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a lazily evaluated one-way binding. <paramref name="map" />
  ///   will be called only when the output of <paramref name="get" /> changes,
  ///   as determined by <paramref name="equals" />. This may have better
  ///   performance than <see cref="oneWay" /> for expensive computations (but
  ///   may be less performant for non-expensive functions due to additional
  ///   overhead).
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="equals">
  ///   Indicates whether two intermediate values are equal. Good candidates are
  ///   <c>elmEq</c> and <c>refEq</c>.
  /// </param>
  /// <param name="map">Transforms the value into the final type.</param>
  static member oneWayLazy:
    get: ('model -> 'a) * equals: ('a -> 'a -> bool) * map: ('a -> 'b) -> (string -> Binding<'model, 'msg, 'b>)

  /// <summary>
  ///   Creates a lazily evaluated one-way binding to an optional value. The
  ///   binding automatically converts between the optional source value and an
  ///   unwrapped (possibly <c>null</c>) value on the view side. <paramref
  ///   name="map" /> will be called only when the output of <paramref
  ///   name="get" /> changes, as determined by <paramref name="equals" />.
  ///
  ///   This may have better performance than a non-lazy binding for expensive
  ///   computations (but may be less performant for non-expensive functions due
  ///   to additional overhead).
  /// </summary>
  /// <param name="get">Gets the intermediate value from the model.</param>
  /// <param name="equals">
  ///   Indicates whether two intermediate values are equal. Good candidates are
  ///   <c>elmEq</c> and <c>refEq</c>.
  /// </param>
  /// <param name="map">Transforms the intermediate value into the final type.</param>
  static member oneWayOptLazy:
    get: ('model -> 'a) * equals: ('a -> 'a -> bool) * map: ('a -> 'b option) ->
      (string -> Binding<'model, 'msg, Nullable<'b>>)

  /// <summary>
  ///   Creates a lazily evaluated one-way binding to an optional value. The
  ///   binding automatically converts between the optional source value and an
  ///   unwrapped (possibly <c>null</c>) value on the view side. <paramref
  ///   name="map" /> will be called only when the output of <paramref
  ///   name="get" /> changes, as determined by <paramref name="equals" />.
  ///
  ///   This may have better performance than a non-lazy binding for expensive
  ///   computations (but may be less performant for non-expensive functions due
  ///   to additional overhead).
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="equals">
  ///   Indicates whether two intermediate values are equal. Good candidates are
  ///   <c>elmEq</c> and <c>refEq</c>.
  /// </param>
  /// <param name="map">Transforms the intermediate value into the final type.</param>
  static member oneWayOptLazy:
    get: ('model -> 'a) * equals: ('a -> 'a -> bool) * map: ('a -> 'b voption) ->
      (string -> Binding<'model, 'msg, Nullable<'b>>)

  /// <summary>
  ///   Creates a lazily evaluated one-way binding to an optional value. The
  ///   binding automatically converts between the optional source value and an
  ///   unwrapped (possibly <c>null</c>) value on the view side. <paramref
  ///   name="map" /> will be called only when the output of <paramref
  ///   name="get" /> changes, as determined by <paramref name="equals" />.
  ///
  ///   This may have better performance than a non-lazy binding for expensive
  ///   computations (but may be less performant for non-expensive functions due
  ///   to additional overhead).
  /// </summary>
  /// <param name="get">Gets the intermediate value from the model.</param>
  /// <param name="equals">
  ///   Indicates whether two intermediate values are equal. Good candidates are
  ///   <c>elmEq</c> and <c>refEq</c>.
  /// </param>
  /// <param name="map">Transforms the intermediate value into the final type.</param>
  static member oneWayOptObjLazy:
    get: ('model -> 'a) * equals: ('a -> 'a -> bool) * map: ('a -> 'b option) -> (string -> Binding<'model, 'msg, 'b>)

  /// <summary>
  ///   Creates a lazily evaluated one-way binding to an optional value. The
  ///   binding automatically converts between the optional source value and an
  ///   unwrapped (possibly <c>null</c>) value on the view side. <paramref
  ///   name="map" /> will be called only when the output of <paramref
  ///   name="get" /> changes, as determined by <paramref name="equals" />.
  ///
  ///   This may have better performance than a non-lazy binding for expensive
  ///   computations (but may be less performant for non-expensive functions due
  ///   to additional overhead).
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="equals">
  ///   Indicates whether two intermediate values are equal. Good candidates are
  ///   <c>elmEq</c> and <c>refEq</c>.
  /// </param>
  /// <param name="map">Transforms the intermediate value into the final type.</param>
  static member oneWayOptObjLazy:
    get: ('model -> 'a) * equals: ('a -> 'a -> bool) * map: ('a -> 'b voption) -> (string -> Binding<'model, 'msg, 'b>)

  /// <summary>
  ///   Creates a one-way binding to a sequence of items, each uniquely
  ///   identified by the value returned by <paramref name="getId"/>. The
  ///   binding will not be updated if the output of <paramref name="get"/>
  ///   does not change, as determined by <paramref name="equals"/>.
  ///   The binding is backed by a persistent <c>ObservableCollection</c>, so
  ///   only changed items (as determined by <paramref name="itemEquals"/>)
  ///   will be replaced. If the items are complex and you want them updated
  ///   instead of replaced, consider using <see cref="subModelSeq"/>.
  /// </summary>
  /// <param name="get">Gets the intermediate value from the model.</param>
  /// <param name="equals">
  ///   Indicates whether two intermediate values are equal. Good candidates are
  ///   <c>elmEq</c> and <c>refEq</c>.
  /// </param>
  /// <param name="map">Transforms the value into the final collection.</param>
  /// <param name="itemEquals">
  ///   Indicates whether two collection items are equal. Good candidates are
  ///   <c>elmEq</c>, <c>refEq</c>, or simply <c>(=)</c>.
  /// </param>
  /// <param name="getId">Gets a unique identifier for a collection item.</param>
  static member oneWaySeqLazy:
    get: ('model -> 'a) *
    equals: ('a -> 'a -> bool) *
    map: ('a -> seq<'b>) *
    itemEquals: ('b -> 'b -> bool) *
    getId: ('b -> 'id) ->
      (string -> Binding<'model, 'msg, ObservableCollection<'b>>)

  /// <summary>
  ///   Creates a one-way binding to a sequence of items, each uniquely
  ///   identified by the value returned by <paramref name="getId"/>. The
  ///   binding will not be updated if the output of <paramref name="get"/>
  ///   does not change, as determined by <paramref name="equals"/>.
  ///   The binding is backed by a persistent <c>ObservableCollection</c>, so
  ///   only changed items (as determined by <paramref name="itemEquals"/>)
  ///   will be replaced. If the items are complex and you want them updated
  ///   instead of replaced, consider using <see cref="subModelSeq"/>.
  /// </summary>
  /// <param name="get">Gets the intermediate value from the model.</param>
  /// <param name="equals">
  ///   Indicates whether two intermediate values are equal. Good candidates are
  ///   <c>elmEq</c> and <c>refEq</c>.
  /// </param>
  /// <param name="map">Transforms the value into the final collection.</param>
  /// <param name="itemEquals">
  ///   Indicates whether two collection items are equal. Good candidates are
  ///   <c>elmEq</c>, <c>refEq</c>, or simply <c>(=)</c>.
  /// </param>
  /// <param name="getId">Gets a unique identifier for a collection item.</param>
  /// <param name="getGrouppingKey">Gets a key used to group items.</param>
  /// <param name="compareKeys">Compares two keys.</param>
  static member oneWaySeqLazy:
    get: ('model -> 'a) *
    equals: ('a -> 'a -> bool) *
    map: ('a -> seq<'b>) *
    itemEquals: ('b -> 'b -> bool) *
    getId: ('b -> 'id) *
    getGroupingKey: ('b -> 'key) *
    [<Optional>] compareKeys: ('key -> 'key -> int) ->
      (string -> Binding<'model, 'msg, ObservableLookup<'key, 'b>>)

  /// <summary>
  ///   Creates a one-way binding to a sequence of items, each uniquely
  ///   identified by the value returned by <paramref name="getId"/>. The
  ///   binding will not be updated if the output of <paramref name="get"/>
  ///   is referentially equal. This is the same as calling
  ///   <see cref="oneWaySeqLazy"/> with <c>equals = refEq</c> and
  ///   <c>map = id</c>. The binding is backed by a persistent
  ///   <c>ObservableCollection</c>, so only changed items (as determined by
  ///   <paramref name="itemEquals"/>) will be replaced. If the items are
  ///   complex and you want them updated instead of replaced, consider using
  ///   <see cref="subModelSeq"/>.
  /// </summary>
  /// <param name="get">Gets the collection from the model.</param>
  /// <param name="itemEquals">
  ///   Indicates whether two collection items are equal. Good candidates are
  ///   <c>elmEq</c>, <c>refEq</c>, or simply <c>(=)</c>.
  /// </param>
  /// <param name="getId">Gets a unique identifier for a collection item.</param>
  static member oneWaySeq:
    get: ('model -> seq<'a>) * itemEquals: ('a -> 'a -> bool) * getId: ('a -> 'id) ->
      (string -> Binding<'model, 'msg, ObservableCollection<'a>>)

  /// <summary>
  ///   Creates a one-way binding to a sequence of items, each uniquely
  ///   identified by the value returned by <paramref name="getId"/>. The
  ///   binding will not be updated if the output of <paramref name="get"/>
  ///   is referentially equal. This is the same as calling
  ///   <see cref="oneWaySeqLazy"/> with <c>equals = refEq</c> and
  ///   <c>map = id</c>. The binding is backed by a persistent
  ///   <c>ObservableCollection</c>, so only changed items (as determined by
  ///   <paramref name="itemEquals"/>) will be replaced. If the items are
  ///   complex and you want them updated instead of replaced, consider using
  ///   <see cref="subModelSeq"/>.
  /// </summary>
  /// <param name="get">Gets the collection from the model.</param>
  /// <param name="itemEquals">
  ///   Indicates whether two collection items are equal. Good candidates are
  ///   <c>elmEq</c>, <c>refEq</c>, or simply <c>(=)</c>.
  /// </param>
  /// <param name="getId">Gets a unique identifier for a collection item.</param>
  /// <param name="getGrouppingKey">Gets a key used to group items.</param>
  /// <param name="compareKeys">Compares two keys.</param>
  static member oneWaySeq:
    get: ('model -> seq<'a>) *
    itemEquals: ('a -> 'a -> bool) *
    getId: ('a -> 'id) *
    getGroupingKey: ('a -> 'key) *
    [<Optional>] compareKeys: ('key -> 'key -> int) ->
      (string -> Binding<'model, 'msg, ObservableLookup<'key, 'a>>)

  /// <summary>Creates a two-way binding.</summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  static member twoWay:
    get: ('model -> 'a) * setWithModel: ('a -> 'model -> 'msg) -> (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding to an optional value. The binding
  ///   automatically converts between the optional source value and an
  ///   unwrapped (possibly <c>null</c>) value on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  static member twoWayOpt:
    get: ('model -> 'a option) * setWithModel: ('a option -> 'model -> 'msg) ->
      (string -> Binding<'model, 'msg, Nullable<'a>>)

  /// <summary>
  ///   Creates a two-way binding to an optional value. The binding
  ///   automatically converts between the optional source value and an
  ///   unwrapped (possibly <c>null</c>) value on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="set">Returns the message to dispatch.</param>
  static member twoWayOpt:
    get: ('model -> 'a voption) * set: ('a voption -> 'model -> 'msg) -> (string -> Binding<'model, 'msg, Nullable<'a>>)

  /// <summary>
  ///   Creates a two-way binding to an optional value. The binding
  ///   automatically converts between the optional source value and an
  ///   unwrapped (possibly <c>null</c>) value on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  static member twoWayOptObj:
    get: ('model -> 'a option) * setWithModel: ('a option -> 'model -> 'msg) -> (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding to an optional value. The binding
  ///   automatically converts between the optional source value and an
  ///   unwrapped (possibly <c>null</c>) value on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="set">Returns the message to dispatch.</param>
  static member twoWayOptObj:
    get: ('model -> 'a voption) * set: ('a voption -> 'model -> 'msg) -> (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding with validation using <c>INotifyDataErrorInfo</c>.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation messages from the updated model.</param>
  static member twoWayValidate:
    get: ('model -> 'a) * setWithModel: ('a -> 'model -> 'msg) * validate: ('model -> string list) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding with validation using <c>INotifyDataErrorInfo</c>.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayValidate:
    get: ('model -> 'a) * setWithModel: ('a -> 'model -> 'msg) * validate: ('model -> string voption) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding with validation using <c>INotifyDataErrorInfo</c>.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayValidate:
    get: ('model -> 'a) * setWithModel: ('a -> 'model -> 'msg) * validate: ('model -> string option) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding with validation using <c>INotifyDataErrorInfo</c>.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayValidate:
    get: ('model -> 'a) * setWithModel: ('a -> 'model -> 'msg) * validate: ('model -> Result<'ignored, string>) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation messages from the updated model.</param>
  static member twoWayOptValidate:
    get: ('model -> 'a voption) * setWithModel: ('a voption -> 'model -> 'msg) * validate: ('model -> string list) ->
      (string -> Binding<'model, 'msg, Nullable<'a>>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayOptValidate:
    get: ('model -> 'a voption) * setWithModel: ('a voption -> 'model -> 'msg) * validate: ('model -> string voption) ->
      (string -> Binding<'model, 'msg, Nullable<'a>>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayOptValidate:
    get: ('model -> 'a voption) * setWithModel: ('a voption -> 'model -> 'msg) * validate: ('model -> string option) ->
      (string -> Binding<'model, 'msg, Nullable<'a>>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayOptValidate:
    get: ('model -> 'a voption) *
    setWithModel: ('a voption -> 'model -> 'msg) *
    validate: ('model -> Result<'ignored, string>) ->
      (string -> Binding<'model, 'msg, Nullable<'a>>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation messages from the updated model.</param>
  static member twoWayOptValidate:
    get: ('model -> 'a option) * setWithModel: ('a option -> 'model -> 'msg) * validate: ('model -> string list) ->
      (string -> Binding<'model, 'msg, Nullable<'a>>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayOptValidate:
    get: ('model -> 'a option) * setWithModel: ('a option -> 'model -> 'msg) * validate: ('model -> string voption) ->
      (string -> Binding<'model, 'msg, Nullable<'a>>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayOptValidate:
    get: ('model -> 'a option) * setWithModel: ('a option -> 'model -> 'msg) * validate: ('model -> string option) ->
      (string -> Binding<'model, 'msg, Nullable<'a>>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayOptValidate:
    get: ('model -> 'a option) *
    setWithModel: ('a option -> 'model -> 'msg) *
    validate: ('model -> Result<'ignored, string>) ->
      (string -> Binding<'model, 'msg, Nullable<'a>>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation messages from the updated model.</param>
  static member twoWayOptObjValidate:
    get: ('model -> 'a voption) * setWithModel: ('a voption -> 'model -> 'msg) * validate: ('model -> string list) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayOptObjValidate:
    get: ('model -> 'a voption) * setWithModel: ('a voption -> 'model -> 'msg) * validate: ('model -> string voption) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayOptObjValidate:
    get: ('model -> 'a voption) * setWithModel: ('a voption -> 'model -> 'msg) * validate: ('model -> string option) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayOptObjValidate:
    get: ('model -> 'a voption) *
    setWithModel: ('a voption -> 'model -> 'msg) *
    validate: ('model -> Result<'ignored, string>) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation messages from the updated model.</param>
  static member twoWayOptObjValidate:
    get: ('model -> 'a option) * setWithModel: ('a option -> 'model -> 'msg) * validate: ('model -> string list) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayOptObjValidate:
    get: ('model -> 'a option) * setWithModel: ('a option -> 'model -> 'msg) * validate: ('model -> string voption) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayOptObjValidate:
    get: ('model -> 'a option) * setWithModel: ('a option -> 'model -> 'msg) * validate: ('model -> string option) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding to an optional value with validation using
  ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
  ///   the optional source value and an unwrapped (possibly <c>null</c>) value
  ///   on the view side.
  /// </summary>
  /// <param name="get">Gets the value from the model.</param>
  /// <param name="setWithModel">Returns the message to dispatch.</param>
  /// <param name="validate">Returns the validation message from the updated model.</param>
  static member twoWayOptObjValidate:
    get: ('model -> 'a option) *
    setWithModel: ('a option -> 'model -> 'msg) *
    validate: ('model -> Result<'ignored, string>) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a two-way binding to a sequence of items, each uniquely
  ///   identified by the value returned by <paramref name="getId"/>. The
  ///   binding will not be updated if the output of <paramref name="get"/>
  ///   is referentially equal. This is the same as calling
  ///   <see cref="twoWaySeqLazy"/> with <c>equals = refEq</c> and
  ///   <c>map = id</c>. The binding is backed by a persistent
  ///   <c>ObservableCollection</c>, so only changed items (as determined by
  ///   <paramref name="itemEquals"/>) will be replaced. If the items are
  ///   complex and you want them updated instead of replaced, consider using
  ///   <see cref="subModelSeq"/>.
  /// </summary>
  /// <param name="get">Gets the collection from the model.</param>
  /// <param name="itemEquals">
  ///   Indicates whether two collection items are equal. Good candidates are
  ///   <c>elmEq</c>, <c>refEq</c>, or simply <c>(=)</c>.
  /// </param>
  /// <param name="getId">Gets a unique identifier for a collection item.</param>
  /// <param name="update">Updates the collection from UI.</param>
  static member twoWaySeq:
    get: ('model -> seq<'T>) *
    itemEquals: ('T -> 'T -> bool) *
    getId: ('T -> 'id) *
    update: (NotifyCollectionChangedEventArgs -> seq<'T> -> 'msg) ->
      (string -> Binding<'model, 'msg, ObservableCollection<'T>>)

  /// <summary>
  ///   Creates a two-way binding to a sequence of items, each uniquely
  ///   identified by the value returned by <paramref name="getId"/>. The
  ///   binding will not be updated if the output of <paramref name="get"/>
  ///   is referentially equal. This is the same as calling
  ///   <see cref="twoWaySeqLazy"/> with <c>equals = refEq</c> and
  ///   <c>map = id</c>. The binding is backed by a persistent
  ///   <c>ObservableCollection</c>, so only changed items (as determined by
  ///   <paramref name="itemEquals"/>) will be replaced. If the items are
  ///   complex and you want them updated instead of replaced, consider using
  ///   <see cref="subModelSeq"/>.
  /// </summary>
  /// <param name="get">Gets the collection from the model.</param>
  /// <param name="itemEquals">
  ///   Indicates whether two collection items are equal. Good candidates are
  ///   <c>elmEq</c>, <c>refEq</c>, or simply <c>(=)</c>.
  /// </param>
  /// <param name="getId">Gets a unique identifier for a collection item.</param>
  /// <param name="update">Updates the collection from UI.</param>
  static member twoWaySeq:
    get: ('model -> seq<'T>) *
    itemEquals: ('T -> 'T -> bool) *
    getId: ('T -> 'id) *
    update: (NotifyCollectionChangedEventArgs -> 'msg) ->
      (string -> Binding<'model, 'msg, ObservableCollection<'T>>)

  /// <summary>
  ///   Creates a two-way binding to a sequence of items, each uniquely
  ///   identified by the value returned by <paramref name="getId"/>. The
  ///   binding will not be updated if the output of <paramref name="get"/>
  ///   does not change, as determined by <paramref name="equals"/>.
  ///   The binding is backed by a persistent <c>ObservableCollection</c>, so
  ///   only changed items (as determined by <paramref name="itemEquals"/>)
  ///   will be replaced. If the items are complex and you want them updated
  ///   instead of replaced, consider using <see cref="subModelSeq"/>.
  /// </summary>
  /// <param name="get">Gets the intermediate value from the model.</param>
  /// <param name="equals">
  ///   Indicates whether two intermediate values are equal. Good candidates are
  ///   <c>elmEq</c> and <c>refEq</c>.
  /// </param>
  /// <param name="map">Transforms the value into the final collection.</param>
  /// <param name="itemEquals">
  ///   Indicates whether two collection items are equal. Good candidates are
  ///   <c>elmEq</c>, <c>refEq</c>, or simply <c>(=)</c>.
  /// </param>
  /// <param name="getId">Gets a unique identifier for a collection item.</param>
  /// <param name="update">Updates the collection from UI.</param>
  static member twoWaySeqLazy:
    get: ('model -> 'T) *
    equals: ('T -> 'T -> bool) *
    map: ('T -> seq<'b>) *
    itemEquals: ('b -> 'b -> bool) *
    getId: ('b -> 'id) *
    update: (NotifyCollectionChangedEventArgs -> seq<'b> -> 'msg) ->
      (string -> Binding<'model, 'msg, ObservableCollection<'b>>)

  /// <summary>
  ///   Creates a two-way binding to a sequence of items, each uniquely
  ///   identified by the value returned by <paramref name="getId"/>. The
  ///   binding will not be updated if the output of <paramref name="get"/>
  ///   does not change, as determined by <paramref name="equals"/>.
  ///   The binding is backed by a persistent <c>ObservableCollection</c>, so
  ///   only changed items (as determined by <paramref name="itemEquals"/>)
  ///   will be replaced. If the items are complex and you want them updated
  ///   instead of replaced, consider using <see cref="subModelSeq"/>.
  /// </summary>
  /// <param name="get">Gets the intermediate value from the model.</param>
  /// <param name="equals">
  ///   Indicates whether two intermediate values are equal. Good candidates are
  ///   <c>elmEq</c> and <c>refEq</c>.
  /// </param>
  /// <param name="map">Transforms the value into the final collection.</param>
  /// <param name="itemEquals">
  ///   Indicates whether two collection items are equal. Good candidates are
  ///   <c>elmEq</c>, <c>refEq</c>, or simply <c>(=)</c>.
  /// </param>
  /// <param name="getId">Gets a unique identifier for a collection item.</param>
  /// <param name="update">Updates the collection from UI.</param>
  static member twoWaySeqLazy:
    get: ('model -> 'T) *
    equals: ('T -> 'T -> bool) *
    map: ('T -> seq<'b>) *
    itemEquals: ('b -> 'b -> bool) *
    getId: ('b -> 'id) *
    update: (NotifyCollectionChangedEventArgs -> 'msg) ->
      (string -> Binding<'model, 'msg, ObservableCollection<'b>>)

  /// <summary>
  ///   Creates a <c>Command</c> binding that depends only on the model (not the
  ///   <c>CommandParameter</c>) and can always execute.
  /// </summary>
  /// <param name="msg">The message to dispatch.</param>
  static member cmd: msg: 'msg -> (string -> Binding<'model, 'msg, ICommand>)
  /// <summary>
  ///   Creates a <c>Command</c> binding that depends only on the model (not the
  ///   <c>CommandParameter</c>) and can always execute.
  /// </summary>
  /// <param name="exec">Returns the message to dispatch.</param>
  static member cmd: exec: ('model -> 'msg) -> (string -> Binding<'model, 'msg, ICommand>)

  /// <summary>
  ///   Creates a conditional <c>Command</c> binding that depends only on the
  ///   model (not the <c>CommandParameter</c>) and can execute if <paramref
  ///   name="canExec" /> returns <c>true</c>.
  /// </summary>
  /// <param name="msg">The message to dispatch.</param>
  /// <param name="canExec">Indicates whether the command can execute.</param>
  static member cmdIf: msg: 'msg * canExec: ('model -> bool) -> (string -> Binding<'model, 'msg, ICommand>)
  /// <summary>
  ///   Creates a conditional <c>Command</c> binding that depends only on the
  ///   model (not the <c>CommandParameter</c>) and can execute if <paramref
  ///   name="canExec" /> returns <c>true</c>.
  /// </summary>
  /// <param name="exec">Returns the message to dispatch.</param>
  /// <param name="canExec">Indicates whether the command can execute.</param>
  static member cmdIf: exec: ('model -> 'msg) * canExec: ('model -> bool) -> (string -> Binding<'model, 'msg, ICommand>)

  /// <summary>
  ///   Creates a conditional <c>Command</c> binding that depends only on the
  ///   model (not the <c>CommandParameter</c>) and can execute if <paramref
  ///   name="exec" /> returns <c>ValueSome</c>.
  /// </summary>
  /// <param name="exec">Returns the message to dispatch.</param>
  static member cmdIf: exec: ('model -> 'msg voption) -> (string -> Binding<'model, 'msg, ICommand>)
  /// <summary>
  ///   Creates a conditional <c>Command</c> binding that depends only on the
  ///   model (not the <c>CommandParameter</c>) and can execute if <paramref
  ///   name="exec" /> returns <c>Some</c>.
  /// </summary>
  /// <param name="exec">Returns the message to dispatch.</param>
  static member cmdIf: exec: ('model -> 'msg option) -> (string -> Binding<'model, 'msg, ICommand>)
  /// <summary>
  ///   Creates a conditional <c>Command</c> binding that depends only on the
  ///   model (not the <c>CommandParameter</c>) and can execute if <paramref
  ///   name="exec" /> returns <c>Ok</c>.
  ///
  ///   This overload allows more easily re-using the same validation functions
  ///   for inputs and commands.
  /// </summary>
  /// <param name="exec">Returns the message to dispatch.</param>
  static member cmdIf: exec: ('model -> Result<'msg, 'ignored>) -> (string -> Binding<'model, 'msg, ICommand>)

  /// <summary>
  ///   Creates a <c>Command</c> binding that depends on the
  ///   <c>CommandParameter</c> and can always execute.
  /// </summary>
  /// <param name="execWithModel">Returns the message to dispatch.</param>
  static member cmdParam: execWithModel: ('param -> 'model -> 'msg) -> (string -> Binding<'model, 'msg, ICommand>)

  /// <summary>
  ///   Creates a <c>Command</c> binding that depends on the
  ///   <c>CommandParameter</c> and can execute if <paramref name="canExec" />
  ///   returns <c>true</c>.
  /// </summary>
  /// <param name="exec">Returns the message to dispatch.</param>
  /// <param name="execWithModel">Indicates whether the command can execute.</param>
  static member cmdParamIf:
    exec: ('param -> 'model -> 'msg) * execWithModel: ('param -> 'model -> bool) ->
      (string -> Binding<'model, 'msg, ICommand>)

  /// <summary>
  ///   Creates a conditional <c>Command</c> binding that depends on the
  ///   <c>CommandParameter</c> and can execute if <paramref name="exec" />
  ///   returns <c>ValueSome</c>.
  /// </summary>
  /// <param name="execWithModel">Returns the message to dispatch.</param>
  static member cmdParamIf:
    execWithModel: ('param -> 'model -> 'msg voption) -> (string -> Binding<'model, 'msg, ICommand>)

  /// <summary>
  ///   Creates a conditional <c>Command</c> binding that depends on the
  ///   <c>CommandParameter</c> and can execute if <paramref name="exec" />
  ///   returns <c>Some</c>.
  /// </summary>
  /// <param name="execWithModel">Returns the message to dispatch.</param>
  static member cmdParamIf:
    execWithModel: ('param -> 'model -> 'msg option) -> (string -> Binding<'model, 'msg, ICommand>)

  /// <summary>
  ///   Creates a conditional <c>Command</c> binding that depends on the
  ///   <c>CommandParameter</c> and can execute if <paramref name="exec" />
  ///   returns <c>Ok</c>.
  ///
  ///   This overload allows more easily re-using the same validation functions
  ///   for inputs and commands.
  /// </summary>
  /// <param name="execWithModel">Returns the message to dispatch.</param>
  static member cmdParamIf:
    execWithModel: ('param -> 'model -> Result<'msg, 'ignored>) -> (string -> Binding<'model, 'msg, ICommand>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings and
  ///   message type. You typically bind this to the <c>DataContext</c> of a
  ///   <c>UserControl</c> or similar.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  /// <param name="toBindingModel">
  ///   Converts the models to the model used by the bindings.
  /// </param>
  /// <param name="toMsg">
  ///   Converts the messages used in the bindings to parent model messages
  ///   (e.g. a parent message union case that wraps the child message type).
  /// </param>
  static member subModel:
    createVm: 'createVm *
    getSubModel: ('model -> 'subModel) *
    toBindingModel: ('model * 'subModel -> 'bindingModel) *
    toMsg: ('bindingMsg -> 'msg) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings and
  ///   message type. You typically bind this to the <c>DataContext</c> of a
  ///   <c>UserControl</c> or similar.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  /// <param name="toMsg">
  ///   Converts the messages used in the bindings to parent model messages
  ///   (e.g. a parent message union case that wraps the child message type).
  /// </param>
  static member subModelWithModel:
    createVm: 'createVm * getSubModel: ('model -> 'subModel) * toMsg: ('subMsg -> 'msg) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings and
  ///   message type. You typically bind this to the <c>DataContext</c> of a
  ///   <c>UserControl</c> or similar.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  /// <param name="toMsg">
  ///   Converts the messages used in the bindings to parent model messages
  ///   (e.g. a parent message union case that wraps the child message type).
  /// </param>
  static member subModel:
    createVm: 'createVm * getSubModel: ('model -> 'subModel) * toMsg: ('subMsg -> 'msg) ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings.
  ///   You typically bind this to the <c>DataContext</c> of a
  ///   <c>UserControl</c> or similar.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  static member subModelWithModel:
    createVm: 'createVm * getSubModel: ('model -> 'subModel) -> (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings.
  ///   You typically bind this to the <c>DataContext</c> of a
  ///   <c>UserControl</c> or similar.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  static member subModel:
    createVm: 'createVm * getSubModel: ('model -> 'subModel) -> (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings and
  ///   message type, and may not exist. If it does not exist, bindings to this
  ///   model will return <c>null</c> unless <paramref name="sticky" /> is
  ///   <c>true</c>, in which case the last non-<c>null</c> model will be
  ///   returned. You typically bind this to the <c>DataContext</c> of a
  ///   <c>UserControl</c> or similar.
  ///
  ///   The 'sticky' part is useful if you want to e.g. animate away a
  ///   <c>UserControl</c> when the model is missing, but don't want the data
  ///   used by that control to be cleared once the animation starts. (The
  ///   animation must be triggered using another binding since this will never
  ///   return <c>null</c>.)
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  /// <param name="toBindingModel">
  ///   Converts the models to the model used by the bindings.
  /// </param>
  /// <param name="toMsg">
  ///   Converts the messages used in the bindings to parent model messages
  ///   (e.g. a parent message union case that wraps the child message type).
  /// </param>
  /// <param name="sticky">
  ///   If <c>true</c>, when the model is missing, the last non-<c>null</c>
  ///   model will be returned instead of <c>null</c>.
  /// </param>
  static member subModelOptWithModel:
    createVm: 'createVm *
    getSubModel: ('model -> 'subModel voption) *
    toBindingModel: ('model * 'subModel -> 'bindingModel) *
    toMsg: ('bindingMsg -> 'msg) *
    [<Optional>] sticky: bool ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings and
  ///   message type, and may not exist. If it does not exist, bindings to this
  ///   model will return <c>null</c> unless <paramref name="sticky" /> is
  ///   <c>true</c>, in which case the last non-<c>null</c> model will be
  ///   returned. You typically bind this to the <c>DataContext</c> of a
  ///   <c>UserControl</c> or similar.
  ///
  ///   The 'sticky' part is useful if you want to e.g. animate away a
  ///   <c>UserControl</c> when the model is missing, but don't want the data
  ///   used by that control to be cleared once the animation starts. (The
  ///   animation must be triggered using another binding since this will never
  ///   return <c>null</c>.)
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  /// <param name="toBindingModel">
  ///   Converts the models to the model used by the bindings.
  /// </param>
  /// <param name="toMsg">
  ///   Converts the messages used in the bindings to parent model messages
  ///   (e.g. a parent message union case that wraps the child message type).
  /// </param>
  /// <param name="sticky">
  ///   If <c>true</c>, when the model is missing, the last non-<c>null</c>
  ///   model will be returned instead of <c>null</c>.
  /// </param>
  static member subModelOptWithModel:
    createVm: 'createVm *
    getSubModel: ('model -> 'subModel option) *
    toBindingModel: ('model * 'subModel -> 'bindingModel) *
    toMsg: ('bindingMsg -> 'msg) *
    [<Optional>] sticky: bool ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings and
  ///   message type, and may not exist. If it does not exist, bindings to this
  ///   model will return <c>null</c> unless <paramref name="sticky" /> is
  ///   <c>true</c>, in which case the last non-<c>null</c> model will be
  ///   returned. You typically bind this to the <c>DataContext</c> of a
  ///   <c>UserControl</c> or similar.
  ///
  ///   The 'sticky' part is useful if you want to e.g. animate away a
  ///   <c>UserControl</c> when the model is missing, but don't want the data
  ///   used by that control to be cleared once the animation starts. (The
  ///   animation must be triggered using another binding since this will never
  ///   return <c>null</c>.)
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  /// <param name="toMsg">
  ///   Converts the messages used in the bindings to parent model messages
  ///   (e.g. a parent message union case that wraps the child message type).
  /// </param>
  /// <param name="sticky">
  ///   If <c>true</c>, when the model is missing, the last non-<c>null</c>
  ///   model will be returned instead of <c>null</c>.
  /// </param>
  static member subModelOptWithModel:
    createVm: 'createVm *
    getSubModel: ('model -> 'subModel voption) *
    toMsg: ('subMsg -> 'msg) *
    [<Optional>] sticky: bool ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings and
  ///   message type, and may not exist. If it does not exist, bindings to this
  ///   model will return <c>null</c> unless <paramref name="sticky" /> is
  ///   <c>true</c>, in which case the last non-<c>null</c> model will be
  ///   returned. You typically bind this to the <c>DataContext</c> of a
  ///   <c>UserControl</c> or similar.
  ///
  ///   The 'sticky' part is useful if you want to e.g. animate away a
  ///   <c>UserControl</c> when the model is missing, but don't want the data
  ///   used by that control to be cleared once the animation starts. (The
  ///   animation must be triggered using another binding since this will never
  ///   return <c>null</c>.)
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  /// <param name="toMsg">
  ///   Converts the messages used in the bindings to parent model messages
  ///   (e.g. a parent message union case that wraps the child message type).
  /// </param>
  /// <param name="sticky">
  ///   If <c>true</c>, when the model is missing, the last non-<c>null</c>
  ///   model will be returned instead of <c>null</c>.
  /// </param>
  static member subModelOpt:
    createVm: 'createVm *
    getSubModel: ('model -> 'subModel voption) *
    toMsg: ('subMsg -> 'msg) *
    [<Optional>] sticky: bool ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings and
  ///   message type, and may not exist. If it does not exist, bindings to this
  ///   model will return <c>null</c> unless <paramref name="sticky" /> is
  ///   <c>true</c>, in which case the last non-<c>null</c> model will be
  ///   returned. You typically bind this to the <c>DataContext</c> of a
  ///   <c>UserControl</c> or similar.
  ///
  ///   The 'sticky' part is useful if you want to e.g. animate away a
  ///   <c>UserControl</c> when the model is missing, but don't want the data
  ///   used by that control to be cleared once the animation starts. (The
  ///   animation must be triggered using another binding since this will never
  ///   return <c>null</c>.)
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  /// <param name="toMsg">
  ///   Converts the messages used in the bindings to parent model messages
  ///   (e.g. a parent message union case that wraps the child message type).
  /// </param>
  /// <param name="sticky">
  ///   If <c>true</c>, when the model is missing, the last non-<c>null</c>
  ///   model will be returned instead of <c>null</c>.
  /// </param>
  static member subModelOptWithModel:
    createVm: 'createVm *
    getSubModel: ('model -> 'subModel option) *
    toMsg: ('subMsg -> 'msg) *
    [<Optional>] sticky: bool ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings and
  ///   message type, and may not exist. If it does not exist, bindings to this
  ///   model will return <c>null</c> unless <paramref name="sticky" /> is
  ///   <c>true</c>, in which case the last non-<c>null</c> model will be
  ///   returned. You typically bind this to the <c>DataContext</c> of a
  ///   <c>UserControl</c> or similar.
  ///
  ///   The 'sticky' part is useful if you want to e.g. animate away a
  ///   <c>UserControl</c> when the model is missing, but don't want the data
  ///   used by that control to be cleared once the animation starts. (The
  ///   animation must be triggered using another binding since this will never
  ///   return <c>null</c>.)
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  /// <param name="toMsg">
  ///   Converts the messages used in the bindings to parent model messages
  ///   (e.g. a parent message union case that wraps the child message type).
  /// </param>
  /// <param name="sticky">
  ///   If <c>true</c>, when the model is missing, the last non-<c>null</c>
  ///   model will be returned instead of <c>null</c>.
  /// </param>
  static member subModelOpt:
    createVm: ('createVm) *
    getSubModel: ('model -> 'subModel option) *
    toMsg: ('subMsg -> 'msg) *
    [<Optional>] sticky: bool ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings,
  ///   and may not exist. If it does not exist, bindings to this model will
  ///   return <c>null</c> unless <paramref name="sticky" /> is <c>true</c>, in
  ///   which case the last non-<c>null</c> model will be returned. You
  ///   typically bind this to the <c>DataContext</c> of a <c>UserControl</c> or
  ///   similar.
  ///
  ///   The 'sticky' part is useful if you want to e.g. animate away a
  ///   <c>UserControl</c> when the model is missing, but don't want the data
  ///   used by that control to be cleared once the animation starts. (The
  ///   animation must be triggered using another binding since this will never
  ///   return <c>null</c>.)
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  /// <param name="sticky">
  ///   If <c>true</c>, when the model is missing, the last non-<c>null</c>
  ///   model will be returned instead of <c>null</c>.
  /// </param>
  static member subModelOptWithModel:
    createVm: ('createVm) * getSubModel: ('model -> 'subModel voption) * [<Optional>] sticky: bool ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings,
  ///   and may not exist. If it does not exist, bindings to this model will
  ///   return <c>null</c> unless <paramref name="sticky" /> is <c>true</c>, in
  ///   which case the last non-<c>null</c> model will be returned. You
  ///   typically bind this to the <c>DataContext</c> of a <c>UserControl</c> or
  ///   similar.
  ///
  ///   The 'sticky' part is useful if you want to e.g. animate away a
  ///   <c>UserControl</c> when the model is missing, but don't want the data
  ///   used by that control to be cleared once the animation starts. (The
  ///   animation must be triggered using another binding since this will never
  ///   return <c>null</c>.)
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  /// <param name="sticky">
  ///   If <c>true</c>, when the model is missing, the last non-<c>null</c>
  ///   model will be returned instead of <c>null</c>.
  /// </param>
  static member subModelOpt:
    createVm: ('createVm) * getSubModel: ('model -> 'subModel voption) * [<Optional>] sticky: bool ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings,
  ///   and may not exist. If it does not exist, bindings to this model will
  ///   return <c>null</c> unless <paramref name="sticky" /> is <c>true</c>, in
  ///   which case the last non-<c>null</c> model will be returned. You
  ///   typically bind this to the <c>DataContext</c> of a <c>UserControl</c> or
  ///   similar.
  ///
  ///   The 'sticky' part is useful if you want to e.g. animate away a
  ///   <c>UserControl</c> when the model is missing, but don't want the data
  ///   used by that control to be cleared once the animation starts. (The
  ///   animation must be triggered using another binding since this will never
  ///   return <c>null</c>.)
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  /// <param name="sticky">
  ///   If <c>true</c>, when the model is missing, the last non-<c>null</c>
  ///   model will be returned instead of <c>null</c>.
  /// </param>
  static member subModelOptWithModel:
    createVm: ('createVm) * getSubModel: ('model -> 'subModel option) * [<Optional>] sticky: bool ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sub-model/component that has its own bindings,
  ///   and may not exist. If it does not exist, bindings to this model will
  ///   return <c>null</c> unless <paramref name="sticky" /> is <c>true</c>, in
  ///   which case the last non-<c>null</c> model will be returned. You
  ///   typically bind this to the <c>DataContext</c> of a <c>UserControl</c> or
  ///   similar.
  ///
  ///   The 'sticky' part is useful if you want to e.g. animate away a
  ///   <c>UserControl</c> when the model is missing, but don't want the data
  ///   used by that control to be cleared once the animation starts. (The
  ///   animation must be triggered using another binding since this will never
  ///   return <c>null</c>.)
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModel">Gets the sub-model from the model.</param>
  /// <param name="sticky">
  ///   If <c>true</c>, when the model is missing, the last non-<c>null</c>
  ///   model will be returned instead of <c>null</c>.
  /// </param>
  static member subModelOpt:
    createVm: ('createVm) * getSubModel: ('model -> 'subModel option) * [<Optional>] sticky: bool ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Like <see cref="subModelOpt" />, but uses the <c>WindowState</c> wrapper
  ///   to show/hide/close a new window that will have the specified bindings as
  ///   its <c>DataContext</c>.
  ///
  ///   You do not need to set the <c>DataContext</c> yourself (neither in code
  ///   nor XAML).
  ///
  ///   The window can only be closed/hidden by changing the return value of
  ///   <paramref name="getState" />, and can not be directly closed by the
  ///   user. External close attempts (the Close/X button, Alt+F4, or System
  ///   Menu -> Close) will cause the message specified by
  ///   <paramref name="onCloseRequested" /> to be dispatched. You should supply
  ///   <paramref name="onCloseRequested" /> and react to this in a manner that
  ///   will not confuse a user trying to close the window (e.g. by closing it,
  ///   or displaying relevant feedback to the user.)
  ///
  ///   If you don't need a sub-model, you can use
  ///   <c>WindowState&lt;unit&gt;</c> to just control the Window visibility,
  ///   and pass <c>fst</c> to <paramref name="toBindingModel" />.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getState">Gets the window state and a sub-model.</param>
  /// <param name="toBindingModel">
  ///   Converts the models to the model used by the bindings.
  /// </param>
  /// <param name="toMsg">
  ///   Converts the messages used in the bindings to parent model messages
  ///   (e.g. a parent message union case that wraps the child message type).
  /// </param>
  /// <param name="getWindow">
  ///   The function used to get and configure the window.
  /// </param>
  /// <param name="onCloseRequested">
  ///   The message to be dispatched on external close attempts (the Close/X
  ///   button, Alt+F4, or System Menu -> Close).
  /// </param>
  static member subModelWin:
    createVm: 'createVm *
    getState: ('model -> WindowState<'subModel>) *
    toBindingModel: ('model * 'subModel -> 'bindingModel) *
    toMsg: ('bindingMsg -> 'msg) *
    getWindow: ('model -> Dispatch<'msg> -> Window) *
    ?onCloseRequested: 'msg ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Like <see cref="subModelOpt" />, but uses the <c>WindowState</c> wrapper
  ///   to show/hide/close a new window that will have the specified bindings as
  ///   its <c>DataContext</c>.
  ///
  ///   You do not need to set the <c>DataContext</c> yourself (neither in code
  ///   nor XAML).
  ///
  ///   The window can only be closed/hidden by changing the return value of
  ///   <paramref name="getState" />, and can not be directly closed by the
  ///   user. External close attempts (the Close/X button, Alt+F4, or System
  ///   Menu -> Close) will cause the message specified by
  ///   <paramref name="onCloseRequested" /> to be dispatched. You should supply
  ///   <paramref name="onCloseRequested" /> and react to this in a manner that
  ///   will not confuse a user trying to close the window (e.g. by closing it,
  ///   or displaying relevant feedback to the user.)
  ///
  ///   If you don't need a sub-model, you can use
  ///   <c>WindowState&lt;unit&gt;</c> to just control the Window visibility,
  ///   and pass <c>fst</c> to <paramref name="toBindingModel" />.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getState">Gets the window state and a sub-model.</param>
  /// <param name="toBindingModel">
  ///   Converts the models to the model used by the bindings.
  /// </param>
  /// <param name="toMsg">
  ///   Converts the messages used in the bindings to parent model messages
  ///   (e.g. a parent message union case that wraps the child message type).
  /// </param>
  /// <param name="getWindow">
  ///   The function used to get and configure the window.
  /// </param>
  /// <param name="onCloseRequested">
  ///   The message to be dispatched on external close attempts (the Close/X
  ///   button, Alt+F4, or System Menu -> Close).
  /// </param>
  static member subModelWin:
    createVm: 'createVm *
    getState: ('model -> WindowState<'subModel>) *
    toBindingModel: ('model * 'subModel -> 'bindingModel) *
    toMsg: ('bindingMsg -> 'msg) *
    getWindow: (unit -> Window) *
    ?onCloseRequested: 'msg ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Like <see cref="subModelOpt" />, but uses the <c>WindowState</c> wrapper
  ///   to show/hide/close a new window that will have the specified bindings as
  ///   its <c>DataContext</c>.
  ///
  ///   You do not need to set the <c>DataContext</c> yourself (neither in code
  ///   nor XAML).
  ///
  ///   The window can only be closed/hidden by changing the return value of
  ///   <paramref name="getState" />, and can not be directly closed by the
  ///   user. External close attempts (the Close/X button, Alt+F4, or System
  ///   Menu -> Close) will cause the message specified by
  ///   <paramref name="onCloseRequested" /> to be dispatched. You should supply
  ///   <paramref name="onCloseRequested" /> and react to this in a manner that
  ///   will not confuse a user trying to close the window (e.g. by closing it,
  ///   or displaying relevant feedback to the user.)
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getState">Gets the window state and a sub-model.</param>
  /// <param name="toMsg">
  ///   Converts the messages used in the bindings to parent model messages
  ///   (e.g. a parent message union case that wraps the child message type).
  /// </param>
  /// <param name="getWindow">
  ///   The function used to get and configure the window.
  /// </param>
  /// <param name="onCloseRequested">
  ///   The message to be dispatched on external close attempts (the Close/X
  ///   button, Alt+F4, or System Menu -> Close).
  /// </param>
  static member subModelWin:
    createVm: 'createVm *
    getState: ('model -> WindowState<'subModel>) *
    toMsg: ('subMsg -> 'msg) *
    getWindow: ('model -> Dispatch<'msg> -> Window) *
    ?onCloseRequested: 'msg ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Like <see cref="subModelOpt" />, but uses the <c>WindowState</c> wrapper
  ///   to show/hide/close a new window that will have the specified bindings as
  ///   its <c>DataContext</c>.
  ///
  ///   You do not need to set the <c>DataContext</c> yourself (neither in code
  ///   nor XAML).
  ///
  ///   The window can only be closed/hidden by changing the return value of
  ///   <paramref name="getState" />, and can not be directly closed by the
  ///   user. External close attempts (the Close/X button, Alt+F4, or System
  ///   Menu -> Close) will cause the message specified by
  ///   <paramref name="onCloseRequested" /> to be dispatched. You should supply
  ///   <paramref name="onCloseRequested" /> and react to this in a manner that
  ///   will not confuse a user trying to close the window (e.g. by closing it,
  ///   or displaying relevant feedback to the user.)
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getState">Gets the window state and a sub-model.</param>
  /// <param name="toMsg">
  ///   Converts the messages used in the bindings to parent model messages
  ///   (e.g. a parent message union case that wraps the child message type).
  /// </param>
  /// <param name="getWindow">
  ///   The function used to get and configure the window.
  /// </param>
  /// <param name="onCloseRequested">
  ///   The message to be dispatched on external close attempts (the Close/X
  ///   button, Alt+F4, or System Menu -> Close).
  /// </param>
  static member subModelWin:
    createVm: 'createVm *
    getState: ('model -> WindowState<'subModel>) *
    toMsg: ('subMsg -> 'msg) *
    getWindow: (unit -> Window) *
    ?onCloseRequested: 'msg ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Like <see cref="subModelOpt" />, but uses the <c>WindowState</c> wrapper
  ///   to show/hide/close a new window that will have the specified bindings as
  ///   its <c>DataContext</c>.
  ///
  ///   You do not need to set the <c>DataContext</c> yourself (neither in code
  ///   nor XAML).
  ///
  ///   The window can only be closed/hidden by changing the return value of
  ///   <paramref name="getState" />, and can not be directly closed by the
  ///   user. External close attempts (the Close/X button, Alt+F4, or System
  ///   Menu -> Close) will cause the message specified by
  ///   <paramref name="onCloseRequested" /> to be dispatched. You should supply
  ///   <paramref name="onCloseRequested" /> and react to this in a manner that
  ///   will not confuse a user trying to close the window (e.g. by closing it,
  ///   or displaying relevant feedback to the user.)
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getState">Gets the window state and a sub-model.</param>
  /// <param name="getWindow">
  ///   The function used to get and configure the window.
  /// </param>
  /// <param name="onCloseRequested">
  ///   The message to be dispatched on external close attempts (the Close/X
  ///   button, Alt+F4, or System Menu -> Close).
  /// </param>
  static member subModelWin:
    createVm: 'createVm *
    getState: ('model -> WindowState<'subModel>) *
    getWindow: ('model -> Dispatch<'msg> -> Window) *
    ?onCloseRequested: 'msg ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Like <see cref="subModelOpt" />, but uses the <c>WindowState</c> wrapper
  ///   to show/hide/close a new window that will have the specified bindings as
  ///   its <c>DataContext</c>.
  ///
  ///   You do not need to set the <c>DataContext</c> yourself (neither in code
  ///   nor XAML).
  ///
  ///   The window can only be closed/hidden by changing the return value of
  ///   <paramref name="getState" />, and can not be directly closed by the
  ///   user. External close attempts (the Close/X button, Alt+F4, or System
  ///   Menu -> Close) will cause the message specified by
  ///   <paramref name="onCloseRequested" /> to be dispatched. You should supply
  ///   <paramref name="onCloseRequested" /> and react to this in a manner that
  ///   will not confuse a user trying to close the window (e.g. by closing it,
  ///   or displaying relevant feedback to the user.)
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getState">Gets the window state and a sub-model.</param>
  /// <param name="getWindow">
  ///   The function used to get and configure the window.
  /// </param>
  /// <param name="onCloseRequested">
  ///   The message to be dispatched on external close attempts (the Close/X
  ///   button, Alt+F4, or System Menu -> Close).
  /// </param>
  static member subModelWin:
    createVm: 'createVm *
    getState: ('model -> WindowState<'subModel>) *
    getWindow: (unit -> Window) *
    ?onCloseRequested: 'msg ->
      (string -> Binding<'model, 'msg, 'a>)

  /// <summary>
  ///   Creates a binding to a sequence of sub-models, each uniquely identified
  ///   by the value returned by <paramref name="getId"/>. The sub-models have
  ///   their own bindings and message type. You typically bind this to the
  ///   <c>ItemsSource</c> of an <c>ItemsControl</c>, <c>ListView</c>,
  ///   <c>TreeView</c>, etc.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModels">Gets the sub-models from the model.</param>
  /// <param name="toBindingModel">
  ///   Converts the models to the model used by the bindings.
  /// </param>
  /// <param name="getId">Gets a unique identifier for a sub-model.</param>
  /// <param name="toMsg">
  ///   Converts the sub-model ID and messages used in the bindings to parent
  ///   model messages (e.g. a parent message union case that wraps the
  ///   sub-model ID and message type).
  /// </param>
  static member subModelSeqWithModel:
    createVm: 'createVm *
    getSubModels: ('model -> #seq<'subModel>) *
    toBindingModel: ('model * 'subModel -> 'bindingModel) *
    getId: ('bindingModel -> 'id) *
    toMsg: ('id * 'bindingMsg -> 'msg) ->
      (string -> Binding<'model, 'msg, ObservableCollection<'a>>)

  /// <summary>
  ///   Creates a binding to a sequence of sub-models, each uniquely identified
  ///   by the value returned by <paramref name="getId"/>. The sub-models have
  ///   their own bindings and message type. You typically bind this to the
  ///   <c>ItemsSource</c> of an <c>ItemsControl</c>, <c>ListView</c>,
  ///   <c>TreeView</c>, etc.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModels">Gets the sub-models from the model.</param>
  /// <param name="getId">Gets a unique identifier for a sub-model.</param>
  /// <param name="toMsg">
  ///   Converts the sub-model ID and messages used in the bindings to parent
  ///   model messages (e.g. a parent message union case that wraps the
  ///   sub-model ID and message type).
  /// </param>
  static member subModelSeqWithModel:
    createVm: 'createVm *
    getSubModels: ('model -> #seq<'subModel>) *
    getId: ('subModel -> 'id) *
    toMsg: ('id * 'subMsg -> 'msg) ->
      (string -> Binding<'model, 'msg, ObservableCollection<'a>>)

  /// <summary>
  ///   Creates a binding to a sequence of sub-models, each uniquely identified
  ///   by the value returned by <paramref name="getId"/>. The sub-models have
  ///   their own bindings and message type. You typically bind this to the
  ///   <c>ItemsSource</c> of an <c>ItemsControl</c>, <c>ListView</c>,
  ///   <c>TreeView</c>, etc.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModels">Gets the sub-models from the model.</param>
  /// <param name="getId">Gets a unique identifier for a sub-model.</param>
  /// <param name="toMsg">
  ///   Converts the sub-model ID and messages used in the bindings to parent
  ///   model messages (e.g. a parent message union case that wraps the
  ///   sub-model ID and message type).
  /// </param>
  static member subModelSeq:
    createVm: 'createVm *
    getSubModels: ('model -> #seq<'subModel>) *
    getId: ('subModel -> 'id) *
    toMsg: ('id * 'subMsg -> 'msg) ->
      (string -> Binding<'model, 'msg, ObservableCollection<'a>>)

  /// <summary>
  ///   Creates a binding to a sequence of sub-models, each uniquely identified
  ///   by the value returned by <paramref name="getId"/>. The sub-models have
  ///   their own bindings. You typically bind this to the <c>ItemsSource</c> of
  ///   an
  ///   <c>ItemsControl</c>, <c>ListView</c>, <c>TreeView</c>, etc.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModels">Gets the sub-models from the model.</param>
  /// <param name="getId">Gets a unique identifier for a sub-model.</param>
  static member subModelSeqWithModel:
    createVm: 'createVm * getSubModels: ('model -> #seq<'subModel>) * getId: ('subModel -> 'id) ->
      (string -> Binding<'model, 'msg, ObservableCollection<'a>>)

  /// <summary>
  ///   Creates a binding to a sequence of sub-models, each uniquely identified
  ///   by the value returned by <paramref name="getId"/>. The sub-models have
  ///   their own bindings. You typically bind this to the <c>ItemsSource</c> of
  ///   an
  ///   <c>ItemsControl</c>, <c>ListView</c>, <c>TreeView</c>, etc.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModels">Gets the sub-models from the model.</param>
  /// <param name="getId">Gets a unique identifier for a sub-model.</param>
  static member subModelSeq:
    createVm: 'createVm * getSubModels: ('model -> #seq<'subModel>) * getId: ('subModel -> 'id) ->
      (string -> Binding<'model, 'msg, ObservableCollection<'a>>)

  /// <summary>
  ///   Creates a binding to a sequence of sub-models, each uniquely identified
  ///   by order number. The sub-models have their own bindings.
  ///   You typically bind this to the <c>ItemsSource</c> of an
  ///   <c>ItemsControl</c>, <c>ListView</c>, <c>TreeView</c>, etc.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getSubModels">Gets the sub-models from the model.</param>
  static member subModelSeq:
    createVm: 'createVm * getSubModels: ('model -> #seq<'subModel>) ->
      (string -> Binding<'model, 'msg, ObservableCollection<'a>>)

  /// <summary>
  ///   Creates a binding to a sequence of sub-models, each uniquely identified
  ///   by the value returned by <paramref name="getId"/>. The sub-models have
  ///   their own bindings. You typically bind this to the <c>ItemsSource</c> of
  ///   an
  ///   <c>ItemsControl</c>, <c>ListView</c>, <c>TreeView</c>, etc.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getId">Gets a unique identifier for a sub-model.</param>
  static member subModelSeqWithModel:
    createVm: 'createVm * getId: ('subModel -> 'id) -> (string -> Binding<'model, 'msg, ObservableCollection<'a>>)

  /// <summary>
  ///   Creates a binding to a sequence of sub-models, each uniquely identified
  ///   by the value returned by <paramref name="getId"/>. The sub-models have
  ///   their own bindings. You typically bind this to the <c>ItemsSource</c> of
  ///   an
  ///   <c>ItemsControl</c>, <c>ListView</c>, <c>TreeView</c>, etc.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  /// <param name="getId">Gets a unique identifier for a sub-model.</param>
  static member subModelSeq:
    createVm: 'createVm * getId: ('subModel -> 'id) -> (string -> Binding<'model, 'id * 'msg, ObservableCollection<'a>>)

  /// <summary>
  ///   Creates a binding to a sequence of sub-models, each uniquely identified
  ///   by order number. The sub-models have their own bindings.
  ///   You typically bind this to the <c>ItemsSource</c> of an
  ///   <c>ItemsControl</c>, <c>ListView</c>, <c>TreeView</c>, etc.
  /// </summary>
  /// <param name="createVm">Creates the view model for the sub-model.</param>
  static member subModelSeq: createVm: 'createVm -> (string -> Binding<'model, int * 'msg, ObservableCollection<'a>>)

  /// <summary>
  ///   Creates a two-way binding to a sequence of sub-models, each uniquely
  ///   identified by order number. The sub-models have their own bindings. You
  ///   typically bind this to the <c>ItemsSource</c> of an <c>ItemsControl</c>,
  ///   <c>ListView</c>, <c>TreeView</c>, etc.
  /// </summary>
  /// <param name="subModelSeqBindingName">
  ///   The name of the <see cref="subModelSeq" /> binding used as the items
  ///   source.
  /// </param>
  /// <param name="get">Gets the selected sub-model/sub-binding ID from the
  /// model.</param>
  /// <param name="set">
  ///   Returns the message to dispatch on selections/de-selections.
  /// </param>
  static member subModelSelectedItem:
    subModelSeqBindingName: string * get: ('model -> 'id voption) * set: ('id voption -> 'model -> 'msg) ->
      (string -> Binding<'model, 'msg, 'id>)

  /// <summary>
  ///   Creates a two-way binding to a sequence of sub-models, each uniquely
  ///   identified by order number. The sub-models have their own bindings. You
  ///   typically bind this to the <c>ItemsSource</c> of an <c>ItemsControl</c>,
  ///   <c>ListView</c>, <c>TreeView</c>, etc.
  /// </summary>
  /// <param name="subModelSeqBindingName">
  ///   The name of the <see cref="subModelSeq" /> binding used as the items
  ///   source.
  /// </param>
  /// <param name="get">Gets the selected sub-model/sub-binding ID from the
  /// model.</param>
  /// <param name="set">
  ///   Returns the message to dispatch on selections/de-selections.
  /// </param>
  static member subModelSelectedItem:
    subModelSeqBindingName: string * get: ('model -> 'id option) * set: ('id option -> 'model -> 'msg) ->
      (string -> Binding<'model, 'msg, 'id>)

[<AutoOpen>]
module ExtensionsT =

  type BindingT with

    /// <summary>Creates a two-way binding.</summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    static member twoWay: get: ('model -> 'a) * set: ('a -> 'msg) -> (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a two-way binding to an optional value. The binding
    ///   automatically converts between the optional source value and an
    ///   unwrapped (possibly <c>null</c>) value on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    static member twoWayOpt:
      get: ('model -> 'a option) * set: ('a option -> 'msg) -> (string -> Binding<'model, 'msg, Nullable<'a>>)

    /// <summary>
    ///   Creates a two-way binding to an optional value. The binding
    ///   automatically converts between the optional source value and an
    ///   unwrapped (possibly <c>null</c>) value on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    static member twoWayOpt:
      get: ('model -> 'a voption) * set: ('a voption -> 'msg) -> (string -> Binding<'model, 'msg, Nullable<'a>>)

    /// <summary>
    ///   Creates a two-way binding to an optional value. The binding
    ///   automatically converts between the optional source value and an
    ///   unwrapped (possibly <c>null</c>) value on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    static member twoWayOptObj:
      get: ('model -> 'a option) * set: ('a option -> 'msg) -> (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a two-way binding to an optional value. The binding
    ///   automatically converts between the optional source value and an
    ///   unwrapped (possibly <c>null</c>) value on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    static member twoWayOptObj:
      get: ('model -> 'a voption) * set: ('a voption -> 'msg) -> (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a two-way binding with validation using
    ///   <c>INotifyDataErrorInfo</c>.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation messages from the updated model.
    /// </param>
    static member twoWayValidate:
      get: ('model -> 'a) * set: ('a -> 'msg) * validate: ('model -> string list) ->
        (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a two-way binding with validation using
    ///   <c>INotifyDataErrorInfo</c>.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation message from the updated model.
    /// </param>
    static member twoWayValidate:
      get: ('model -> 'a) * set: ('a -> 'msg) * validate: ('model -> string voption) ->
        (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a two-way binding with validation using
    ///   <c>INotifyDataErrorInfo</c>.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation message from the updated model.
    /// </param>
    static member twoWayValidate:
      get: ('model -> 'a) * set: ('a -> 'msg) * validate: ('model -> string option) ->
        (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a two-way binding with validation using
    ///   <c>INotifyDataErrorInfo</c>.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation message from the updated model.
    /// </param>
    static member twoWayValidate:
      get: ('model -> 'a) * set: ('a -> 'msg) * validate: ('model -> Result<'ignored, string>) ->
        (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation messages from the updated model.
    /// </param>
    static member twoWayOptValidate:
      get: ('model -> 'a voption) * set: ('a voption -> 'msg) * validate: ('model -> string list) ->
        (string -> Binding<'model, 'msg, Nullable<'a>>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation message from the updated model.
    /// </param>
    static member twoWayOptValidate:
      get: ('model -> 'a voption) * set: ('a voption -> 'msg) * validate: ('model -> string voption) ->
        (string -> Binding<'model, 'msg, Nullable<'a>>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation message from the updated model.
    /// </param>
    static member twoWayOptValidate:
      get: ('model -> 'a voption) * set: ('a voption -> 'msg) * validate: ('model -> string option) ->
        (string -> Binding<'model, 'msg, Nullable<'a>>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation message from the updated model.
    /// </param>
    static member twoWayOptValidate:
      get: ('model -> 'a voption) * set: ('a voption -> 'msg) * validate: ('model -> Result<'ignored, string>) ->
        (string -> Binding<'model, 'msg, Nullable<'a>>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation messages from the updated model.
    /// </param>
    static member twoWayOptValidate:
      get: ('model -> 'a option) * set: ('a option -> 'msg) * validate: ('model -> string list) ->
        (string -> Binding<'model, 'msg, Nullable<'a>>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation message from the updated model.
    /// </param>
    static member twoWayOptValidate:
      get: ('model -> 'a option) * set: ('a option -> 'msg) * validate: ('model -> string voption) ->
        (string -> Binding<'model, 'msg, Nullable<'a>>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation message from the updated model.
    /// </param>
    static member twoWayOptValidate:
      get: ('model -> 'a option) * set: ('a option -> 'msg) * validate: ('model -> string option) ->
        (string -> Binding<'model, 'msg, Nullable<'a>>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation message from the updated model.
    /// </param>
    static member twoWayOptValidate:
      get: ('model -> 'a option) * set: ('a option -> 'msg) * validate: ('model -> Result<'ignored, string>) ->
        (string -> Binding<'model, 'msg, Nullable<'a>>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation messages from the updated model.
    /// </param>
    static member twoWayOptObjValidate:
      get: ('model -> 'a voption) * set: ('a voption -> 'msg) * validate: ('model -> string list) ->
        (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation message from the updated model.
    /// </summary>
    static member twoWayOptObjValidate:
      get: ('model -> 'a voption) * set: ('a voption -> 'msg) * validate: ('model -> string voption) ->
        (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation message from the updated model.
    /// </param>
    static member twoWayOptObjValidate:
      get: ('model -> 'a voption) * set: ('a voption -> 'msg) * validate: ('model -> string option) ->
        (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation message from the updated model.
    /// </param>
    static member twoWayOptObjValidate:
      get: ('model -> 'a voption) * set: ('a voption -> 'msg) * validate: ('model -> Result<'ignored, string>) ->
        (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation messages from the updated model.
    /// </param>
    static member twoWayOptObjValidate:
      get: ('model -> 'a option) * set: ('a option -> 'msg) * validate: ('model -> string list) ->
        (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation messages from the updated model.
    /// </param>
    static member twoWayOptObjValidate:
      get: ('model -> 'a option) * set: ('a option -> 'msg) * validate: ('model -> string voption) ->
        (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation message from the updated model.
    /// </param>
    static member twoWayOptObjValidate:
      get: ('model -> 'a option) * set: ('a option -> 'msg) * validate: ('model -> string option) ->
        (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a two-way binding to an optional value with validation using
    ///   <c>INotifyDataErrorInfo</c>. The binding automatically converts between
    ///   the optional source value and an unwrapped (possibly <c>null</c>) value
    ///   on the view side.
    /// </summary>
    /// <param name="get">Gets the value from the model.</param>
    /// <param name="set">Returns the message to dispatch.</param>
    /// <param name="validate">
    ///   Returns the validation message from the updated model.
    /// </param>
    static member twoWayOptObjValidate:
      get: ('model -> 'a option) * set: ('a option -> 'msg) * validate: ('model -> Result<'ignored, string>) ->
        (string -> Binding<'model, 'msg, 'a>)

    /// <summary>
    ///   Creates a <c>Command</c> binding that dispatches the specified message
    ///   and can always execute.
    /// </summary>
    /// <param name="exec">Returns the message to dispatch.</param>
    static member cmd: exec: 'msg -> (string -> Binding<'model, 'msg, ICommand>)

    /// <summary>
    ///   Creates a <c>Command</c> binding that depends on the
    ///   <c>CommandParameter</c> and can always execute.
    /// </summary>
    /// <param name="exec">Returns the message to dispatch.</param>
    static member cmdParam: exec: ('param -> 'msg) -> (string -> Binding<'model, 'msg, ICommand>)

    /// <summary>
    ///   Creates a conditional <c>Command</c> binding that depends on the
    ///   <c>CommandParameter</c> and can execute if <paramref name="exec" />
    ///   returns <c>ValueSome</c>.
    /// </summary>
    /// <param name="exec">Returns the message to dispatch.</param>
    static member cmdParamIf: exec: ('param -> 'msg voption) -> (string -> Binding<'model, 'msg, ICommand>)
    /// <summary>
    ///   Creates a conditional <c>Command</c> binding that depends on the
    ///   <c>CommandParameter</c> and can execute if <paramref name="exec" />
    ///   returns <c>Some</c>.
    /// </summary>
    /// <param name="exec">Returns the message to dispatch.</param>
    static member cmdParamIf: exec: ('param -> 'msg option) -> (string -> Binding<'model, 'msg, ICommand>)
    /// <summary>
    ///   Creates a conditional <c>Command</c> binding that depends on the
    ///   <c>CommandParameter</c> and can execute if <paramref name="exec" />
    ///   returns <c>Ok</c>.
    ///
    ///   This overload allows more easily re-using the same validation
    ///   functions for inputs and commands.
    /// </summary>
    /// <param name="exec">Returns the message to dispatch.</param>
    static member cmdParamIf: exec: ('param -> Result<'msg, 'ignored>) -> (string -> Binding<'model, 'msg, ICommand>)

    /// <summary>
    ///   Creates a <c>Command</c> binding that depends on the
    ///   <c>CommandParameter</c> and can execute if <paramref name="canExec" />
    ///   returns <c>true</c>.
    /// </summary>
    /// <param name="exec">Returns the message to dispatch.</param>
    /// <param name="canExec">Indicates whether the command can execute.</param>
    static member cmdParamIf:
      exec: ('param -> 'msg) * canExec: ('param -> 'model -> bool) -> (string -> Binding<'model, 'msg, ICommand>)

    /// <summary>
    ///   Creates a <c>Command</c> binding that depends on the
    ///   <c>CommandParameter</c> and can execute if <paramref name="canExec" />
    ///   returns <c>true</c>.
    /// </summary>
    /// <param name="exec">Returns the message to dispatch.</param>
    /// <param name="canExec">Indicates whether the command can execute.</param>
    static member cmdParamIf:
      exec: ('param -> 'model -> 'msg) * canExec: ('param -> bool) -> (string -> Binding<'model, 'msg, ICommand>)

    /// <summary>
    ///   Creates a <c>Command</c> binding that depends on the
    ///   <c>CommandParameter</c> and can execute if <paramref name="canExec" />
    ///   returns <c>true</c>.
    /// </summary>
    /// <param name="exec">Returns the message to dispatch.</param>
    /// <param name="canExec">Indicates whether the command can execute.</param>
    static member cmdParamIf:
      exec: ('param -> 'msg) * canExec: ('param -> bool) -> (string -> Binding<'model, 'msg, ICommand>)

    /// <summary>
    ///   Creates a two-way binding to a <c>SelectedItem</c>-like property where
    ///   the <c>ItemsSource</c>-like property is a <see cref="subModelSeq" />
    ///   binding. Automatically converts the dynamically created Elmish.Uno
    ///   view models to/from their corresponding IDs, so the Elmish user code
    ///   only has to work with the IDs.
    ///
    ///   Only use this if you are unable to use some kind of
    ///   <c>SelectedValue</c> or <c>SelectedIndex</c> property with a normal
    ///   <see cref="twoWay" /> binding. This binding is less type-safe. It will
    ///   throw when initializing the bindings if <paramref name="subModelSeqBindingName" />
    ///   does not correspond to a <see cref="subModelSeq" /> binding, and it
    ///   will throw at runtime if the inferred <c>'id</c> type does not
    ///   match the actual ID type used in that binding.
    /// </summary>
    /// <param name="subModelSeqBindingName">
    ///   The name of the <see cref="subModelSeq" /> binding used as the items
    ///   source.
    /// </param>
    /// <param name="get">Gets the selected sub-model/sub-binding ID from the model.</param>
    /// <param name="set">Returns the message to dispatch on selections/de-selections.</param>
    static member subModelSelectedItem:
      subModelSeqBindingName: string * get: ('model -> 'id voption) * set: ('id voption -> 'msg) ->
        (string -> Binding<'model, 'msg, 'id>)

    /// <summary>
    ///   Creates a two-way binding to a <c>SelectedItem</c>-like property where
    ///   the <c>ItemsSource</c>-like property is a <see cref="subModelSeq" />
    ///   binding. Automatically converts the dynamically created Elmish.Uno
    ///   view models to/from their corresponding IDs, so the Elmish user code
    ///   only has to work with the IDs.
    ///
    ///   Only use this if you are unable to use some kind of
    ///   <c>SelectedValue</c> or <c>SelectedIndex</c> property with a normal
    ///   <see cref="twoWay" /> binding. This binding is less type-safe. It will
    ///   throw when initializing the bindings if <paramref name="subModelSeqBindingName" />
    ///   does not correspond to a <see cref="subModelSeq" /> binding, and it
    ///   will throw at runtime if the inferred <c>'id</c> type does not
    ///   match the actual ID type used in that binding.
    /// </summary>
    /// <param name="subModelSeqBindingName">
    ///   The name of the <see cref="subModelSeq" /> binding used as the items
    ///   source.
    /// </param>
    /// <param name="get">Gets the selected sub-model/sub-binding ID from the model.</param>
    /// <param name="set">Returns the message to dispatch on selections/de-selections.</param>
    static member subModelSelectedItem:
      subModelSeqBindingName: string * get: ('model -> 'id option) * set: ('id option -> 'msg) ->
        (string -> Binding<'model, 'msg, 'id>)
