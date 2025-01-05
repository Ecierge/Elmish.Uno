﻿[<AutoOpen>]
module internal Elmish.Uno.BindingData

open System
open System.Collections
open System.Collections.Generic
open System.Collections.ObjectModel
open System.Collections.Specialized
open System.Windows.Input
open Microsoft.UI.Xaml

open Elmish
open Elmish.Collections

#nowarn "1204"

module Helper =

  let mapDispatch
      (getCurrentModel: unit -> 'model)
      (set: 'bindingMsg -> 'model -> 'msg)
      (dispatch: 'msg -> unit)
      : 'bindingMsg -> unit =
    fun bMsg -> getCurrentModel () |> set bMsg |> dispatch


type OneWayData<'model, 'T> =
  { Get: 'model -> 'T }


type OneWaySeqData<'model, 'T, 'aCollection, 'id when 'id : equality and 'id : not null> =
  { Get: 'model -> 'T seq
    CreateCollection: 'T seq -> CollectionTarget<'T, 'aCollection>
    GetId: 'T -> 'id
    ItemEquals: 'T -> 'T -> bool }

  member d.Merge(values: CollectionTarget<'T, 'aCollection>, newModel: 'model) =
    let create v _ = v
    let update oldVal newVal oldIdx =
      if not (d.ItemEquals newVal oldVal) then
        values.SetAt (oldIdx, newVal)
    let newVals = newModel |> d.Get |> Seq.toArray
    Merge.keyed d.GetId d.GetId create update values newVals


type OneWaySeqGroupedData<'model, 'T, 'aCollection, 'id, 'key when 'id : equality and 'id : not null and 'key : equality and 'key : not null> =
  { Get: 'model -> 'T seq
    CreateCollection: 'T seq -> GroupedCollectionTarget<'T, 'aCollection, 'key>
    GetId: 'T -> 'id
    GetKey: 'T -> 'key
    KeyComparer: IComparer<'key>
    ItemEquals: 'T -> 'T -> bool }

  member d.Merge(values: GroupedCollectionTarget<'T, 'aCollection, 'key>, newModel: 'model) =
    let create v _ = v
    let update (values : CollectionTarget<'T, IList>) oldVal newVal oldIdx =
      if not (d.ItemEquals newVal oldVal) then
          values.SetAt (oldIdx, newVal)
    let newVals = newModel |> d.Get |> Seq.toArray
    Merge.grouped d.KeyComparer d.GetKey d.GetId d.GetId create update values newVals


type TwoWayData<'model, 'msg, 'T> =
  { Get: 'model -> 'T
    Set: 'T -> 'model -> 'msg }


type TwoWaySeqData<'model, 'msg, 'T, 'aCollection, 'id when 'id : equality and 'id : not null> =
  { Get: 'model -> 'T seq
    CreateCollection: 'T seq -> CollectionTarget<'T, 'aCollection>
    GetId: 'T -> 'id
    ItemEquals: 'T -> 'T -> bool
    Update: NotifyCollectionChangedEventArgs -> 'model -> 'msg }

  member d.Merge(values: CollectionTarget<'T, 'aCollection>, newModel: 'model) =
    let create v _ = v
    let update oldVal newVal oldIdx =
      if not (d.ItemEquals newVal oldVal) then
        values.SetAt (oldIdx, newVal)
    let newVals = newModel |> d.Get |> Seq.toArray
    Merge.keyed d.GetId d.GetId create update values newVals


type CmdData<'model, 'msg> = {
  Exec: obj -> 'model -> 'msg voption
  CanExec: obj -> 'model -> bool
}


type SubModelSelectedItemData<'model, 'msg, 'id> =
  { Get: 'model -> 'id voption
    Set: 'id voption -> 'model -> 'msg
    SubModelSeqBindingName: string }


type SubModelData<'model, 'msg, 'bindingModel, 'bindingMsg, 'vm> = {
  GetModel: 'model -> 'bindingModel voption
  CreateViewModel: ViewModelArgs<'bindingModel, 'bindingMsg> -> 'vm
  UpdateViewModel: 'vm * 'bindingModel -> unit
  ToMsg: 'model -> 'bindingMsg -> 'msg
}


and SubModelWinData<'model, 'msg, 'bindingModel, 'bindingMsg, 'vm> = {
  GetState: 'model -> WindowState<'bindingModel>
  CreateViewModel: ViewModelArgs<'bindingModel, 'bindingMsg> -> 'vm
  UpdateViewModel: 'vm * 'bindingModel -> unit
  ToMsg: 'model -> 'bindingMsg -> 'msg
  GetWindow: 'model -> Dispatch<'msg> -> Window
  OnCloseRequested: 'model -> 'msg voption
}


and SubModelSeqUnkeyedData<'model, 'msg, 'bindingModel, 'bindingMsg, 'vm, 'vmCollection> =
  { GetModels: 'model -> 'bindingModel seq
    CreateViewModel: ViewModelArgs<'bindingModel, 'bindingMsg> -> 'vm
    CreateCollection: 'vm seq -> CollectionTarget<'vm, 'vmCollection>
    UpdateViewModel: 'vm * 'bindingModel -> unit
    ToMsg: 'model -> int * 'bindingMsg -> 'msg }


and SubModelSeqKeyedData<'model, 'msg, 'bindingModel, 'bindingMsg, 'vm, 'vmCollection, 'id when 'id : equality and 'id : not null> =
  { GetSubModels: 'model -> 'bindingModel seq
    CreateViewModel: ViewModelArgs<'bindingModel, 'bindingMsg> -> 'vm
    CreateCollection: 'vm seq -> CollectionTarget<'vm, 'vmCollection>
    UpdateViewModel: 'vm * 'bindingModel -> unit
    ToMsg: 'model -> 'id * 'bindingMsg -> 'msg
    BmToId: 'bindingModel -> 'id
    VmToId: 'vm -> 'id }

  member d.MergeKeyed
      (create: 'bindingModel -> 'id -> 'vm,
       update: 'vm -> 'bindingModel -> unit,
       values: CollectionTarget<'vm, 'vmCollection>,
       newSubModels: 'bindingModel []) =
    let update vm bm _ = update vm bm
    Merge.keyed d.BmToId d.VmToId create update values newSubModels


and ValidationData<'model, 'msg, 't> =
  { BindingData: BindingData<'model, 'msg, 't>
    Validate: 'model -> string list }


and LazyData<'model, 'msg, 'bindingModel, 'bindingMsg, 't> =
  { BindingData: BindingData<'bindingModel, 'bindingMsg, 't>
    Get: 'model -> 'bindingModel
    Set: 'bindingMsg -> 'model -> 'msg
    Equals: 'bindingModel -> 'bindingModel -> bool }

  member this.MapDispatch
      (getCurrentModel: unit -> 'model,
       dispatch: 'msg -> unit)
       : 'bindingMsg -> unit =
    Helper.mapDispatch getCurrentModel this.Set dispatch


and AlterMsgStreamData<'model, 'msg, 'bindingModel, 'bindingMsg, 'dispatchMsg, 't> =
 { BindingData: BindingData<'bindingModel, 'bindingMsg, 't>
   Get: 'model -> 'bindingModel
   Set: 'dispatchMsg -> 'model -> 'msg
   AlterMsgStream: ('dispatchMsg -> unit) -> 'bindingMsg -> unit }

  member this.MapDispatch
      (getCurrentModel: unit -> 'model,
       dispatch: 'msg -> unit)
       : 'bindingMsg -> unit =
    Helper.mapDispatch getCurrentModel this.Set dispatch
    |> this.AlterMsgStream


and BaseBindingData<'model, 'msg, 't> =
  | OneWayData of OneWayData<'model, 't>
  | OneWaySeqData of OneWaySeqData<'model, obj, 't, obj>
  | OneWaySeqGroupedData of OneWaySeqGroupedData<'model, obj, 't, obj, obj>
  | TwoWayData of TwoWayData<'model, 'msg, 't>
  | TwoWaySeqData of TwoWaySeqData<'model, 'msg, obj, 't, obj>
  | CmdData of CmdData<'model, 'msg>
  | SubModelData of SubModelData<'model, 'msg, obj, obj, 't>
  | SubModelWinData of SubModelWinData<'model, 'msg, obj, obj, 't>
  | SubModelSeqUnkeyedData of SubModelSeqUnkeyedData<'model, 'msg, obj, obj, obj, 't>
  | SubModelSeqKeyedData of SubModelSeqKeyedData<'model, 'msg, obj, obj, obj, 't, obj>
  | SubModelSelectedItemData of SubModelSelectedItemData<'model, 'msg, obj>


and BindingData<'model, 'msg, 't> =
  | BaseBindingData of BaseBindingData<'model, 'msg, 't>
  | CachingData of BindingData<'model, 'msg, 't>
  | ValidationData of ValidationData<'model, 'msg, 't>
  | LazyData of LazyData<'model, 'msg, obj, obj, 't>
  | AlterMsgStreamData of AlterMsgStreamData<'model, 'msg, obj, obj, obj, 't>



module BindingData =

  module private MapT =

    let baseCase (fOut: 't0 -> 't1) (fIn: 't1 -> 't0) =
      function
      | OneWayData d -> OneWayData {
          Get = d.Get >> fOut
        }
      | OneWaySeqData d -> OneWaySeqData {
          Get = d.Get
          CreateCollection = d.CreateCollection >> CollectionTarget.mapCollection fOut
          GetId = d.GetId
          ItemEquals = d.ItemEquals
        }
      | OneWaySeqGroupedData d -> OneWaySeqGroupedData {
          Get = d.Get
          CreateCollection = d.CreateCollection >> GroupedCollectionTarget.mapCollection fOut
          GetId = d.GetId
          GetKey = d.GetKey
          KeyComparer = d.KeyComparer
          ItemEquals = d.ItemEquals
        }
      | TwoWayData d -> TwoWayData {
          Get = d.Get >> fOut
          Set = fIn >> d.Set
        }
      | TwoWaySeqData d -> TwoWaySeqData {
          Get = d.Get
          CreateCollection = d.CreateCollection >> CollectionTarget.mapCollection fOut
          GetId = d.GetId
          ItemEquals = d.ItemEquals
          Update = d.Update
        }
      | CmdData d -> CmdData {
          Exec = d.Exec
          CanExec = d.CanExec
        }
      | SubModelData d -> SubModelData {
          GetModel = d.GetModel
          CreateViewModel = d.CreateViewModel >> fOut
          UpdateViewModel = (fun (vm,m) -> d.UpdateViewModel (fIn vm, m))
          ToMsg = d.ToMsg
        }
      | SubModelWinData d -> SubModelWinData {
          GetState = d.GetState
          CreateViewModel = d.CreateViewModel >> fOut
          UpdateViewModel = (fun (vm,m) -> d.UpdateViewModel (fIn vm, m))
          ToMsg = d.ToMsg
          GetWindow = d.GetWindow
          OnCloseRequested = d.OnCloseRequested
        }
      | SubModelSeqUnkeyedData d -> SubModelSeqUnkeyedData {
          GetModels = d.GetModels
          CreateViewModel = d.CreateViewModel
          CreateCollection = d.CreateCollection >> CollectionTarget.mapCollection fOut
          UpdateViewModel = d.UpdateViewModel
          ToMsg = d.ToMsg
        }
      | SubModelSeqKeyedData d -> SubModelSeqKeyedData {
          GetSubModels = d.GetSubModels
          CreateViewModel = d.CreateViewModel
          CreateCollection = d.CreateCollection >> CollectionTarget.mapCollection fOut
          UpdateViewModel = d.UpdateViewModel
          ToMsg = d.ToMsg
          VmToId = d.VmToId
          BmToId = d.BmToId
        }
      | SubModelSelectedItemData d -> SubModelSelectedItemData {
          Get = d.Get
          Set = d.Set
          SubModelSeqBindingName = d.SubModelSeqBindingName
        }

    let rec recursiveCase<'model, 'msg, 't0, 't1> (fOut: 't0 -> 't1) (fIn: 't1 -> 't0)
      : BindingData<'model, 'msg, 't0> -> BindingData<'model, 'msg, 't1> =
      function
      | BaseBindingData d -> d |> baseCase fOut fIn |> BaseBindingData
      | CachingData d -> d |> recursiveCase<'model, 'msg, 't0, 't1> fOut fIn |> CachingData
      | ValidationData d -> ValidationData {
          BindingData = recursiveCase<'model, 'msg, 't0, 't1> fOut fIn d.BindingData
          Validate = d.Validate
        }
      | LazyData d -> LazyData {
          Get = d.Get
          Set = d.Set
          BindingData = recursiveCase<obj, obj, 't0, 't1> fOut fIn d.BindingData
          Equals = d.Equals
        }
      | AlterMsgStreamData d -> AlterMsgStreamData {
          BindingData = recursiveCase<obj, obj, 't0, 't1> fOut fIn d.BindingData
          AlterMsgStream = d.AlterMsgStream
          Get = d.Get
          Set = d.Set
        }

  let boxT b = MapT.recursiveCase (box >> nonNull) unbox b
  let unboxT b = MapT.recursiveCase unbox box b

  let mapModel f =
    let binaryHelper binary x m = binary x (f m)
    let baseCase = function
      | OneWayData d -> OneWayData {
          Get = f >> d.Get
        }
      | OneWaySeqData d -> OneWaySeqData {
          Get = f >> d.Get
          CreateCollection = d.CreateCollection
          GetId = d.GetId
          ItemEquals = d.ItemEquals
        }
      | OneWaySeqGroupedData d -> OneWaySeqGroupedData {
          Get = f >> d.Get
          CreateCollection = d.CreateCollection
          GetId = d.GetId
          GetKey = d.GetKey
          KeyComparer = d.KeyComparer
          ItemEquals = d.ItemEquals
        }
      | TwoWayData d -> TwoWayData {
          Get = f >> d.Get
          Set = binaryHelper d.Set
        }
      | TwoWaySeqData d -> TwoWaySeqData {
          Get = f >> d.Get
          CreateCollection = d.CreateCollection
          GetId = d.GetId
          ItemEquals = d.ItemEquals
          Update = fun args m -> d.Update args (f m)
        }
      | CmdData d -> CmdData {
          Exec = binaryHelper d.Exec
          CanExec = binaryHelper d.CanExec
        }
      | SubModelData d -> SubModelData {
          GetModel = f >> d.GetModel
          CreateViewModel = d.CreateViewModel
          UpdateViewModel = d.UpdateViewModel
          ToMsg = f >> d.ToMsg
        }
      | SubModelWinData d -> SubModelWinData {
          GetState = f >> d.GetState
          CreateViewModel = d.CreateViewModel
          UpdateViewModel = d.UpdateViewModel
          ToMsg = f >> d.ToMsg
          GetWindow = f >> d.GetWindow
          OnCloseRequested = f >> d.OnCloseRequested
        }
      | SubModelSeqUnkeyedData d -> SubModelSeqUnkeyedData {
          GetModels = f >> d.GetModels
          CreateViewModel = d.CreateViewModel
          CreateCollection = d.CreateCollection
          UpdateViewModel = d.UpdateViewModel
          ToMsg = f >> d.ToMsg
        }
      | SubModelSeqKeyedData d -> SubModelSeqKeyedData {
          GetSubModels = f >> d.GetSubModels
          CreateViewModel = d.CreateViewModel
          CreateCollection = d.CreateCollection
          UpdateViewModel = d.UpdateViewModel
          ToMsg = f >> d.ToMsg
          BmToId = d.BmToId
          VmToId = d.VmToId
        }
      | SubModelSelectedItemData d -> SubModelSelectedItemData {
          Get = f >> d.Get
          Set = binaryHelper d.Set
          SubModelSeqBindingName = d.SubModelSeqBindingName
        }
    let rec recursiveCase = function
      | BaseBindingData d -> d |> baseCase |> BaseBindingData
      | CachingData d -> d |> recursiveCase |> CachingData
      | ValidationData d -> ValidationData {
          BindingData = recursiveCase d.BindingData
          Validate = f >> d.Validate
        }
      | LazyData d -> LazyData {
          BindingData = d.BindingData
          Get = f >> d.Get
          Set = binaryHelper d.Set
          Equals = d.Equals
        }
      | AlterMsgStreamData d -> AlterMsgStreamData {
          BindingData = d.BindingData
          AlterMsgStream = d.AlterMsgStream
          Get = f >> d.Get
          Set = binaryHelper d.Set
        }
    recursiveCase

  let mapMsgWithModel (f: 'T -> 'model -> 'b) =
    let baseCase = function
      | OneWayData d -> d |> OneWayData
      | OneWaySeqData d -> d |> OneWaySeqData
      | OneWaySeqGroupedData d -> d |> OneWaySeqGroupedData
      | TwoWayData d -> TwoWayData {
          Get = d.Get
          Set = fun v m -> f (d.Set v m) m
        }
      | TwoWaySeqData d -> TwoWaySeqData {
          Get = d.Get
          CreateCollection = d.CreateCollection
          GetId = d.GetId
          ItemEquals = d.ItemEquals
          Update = fun args m -> f (d.Update args m) m
        }
      | CmdData d -> CmdData {
          Exec = fun p m -> d.Exec p m |> ValueOption.map (fun msg -> f msg m)
          CanExec = fun p m -> d.CanExec p m
        }
      | SubModelData d -> SubModelData {
          GetModel = d.GetModel
          CreateViewModel = d.CreateViewModel
          UpdateViewModel = d.UpdateViewModel
          ToMsg = fun m bMsg -> f (d.ToMsg m bMsg) m
        }
      | SubModelWinData d -> SubModelWinData {
          GetState = d.GetState
          CreateViewModel = d.CreateViewModel
          UpdateViewModel = d.UpdateViewModel
          ToMsg = fun m bMsg -> f (d.ToMsg m bMsg) m
          GetWindow = fun m dispatch -> d.GetWindow m (fun msg -> f msg m |> dispatch)
          OnCloseRequested = fun m -> m |> d.OnCloseRequested |> ValueOption.map (fun msg -> f msg m)
        }
      | SubModelSeqUnkeyedData d -> SubModelSeqUnkeyedData {
          GetModels = d.GetModels
          CreateViewModel = d.CreateViewModel
          CreateCollection = d.CreateCollection
          UpdateViewModel = d.UpdateViewModel
          ToMsg = fun m bMsg -> f (d.ToMsg m bMsg) m
        }
      | SubModelSeqKeyedData d -> SubModelSeqKeyedData {
          GetSubModels = d.GetSubModels
          CreateViewModel = d.CreateViewModel
          CreateCollection = d.CreateCollection
          UpdateViewModel = d.UpdateViewModel
          ToMsg = fun m bMsg -> f (d.ToMsg m bMsg) m
          BmToId = d.BmToId
          VmToId = d.VmToId
        }
      | SubModelSelectedItemData d -> SubModelSelectedItemData {
          Get = d.Get
          Set = fun v m -> f (d.Set v m) m
          SubModelSeqBindingName = d.SubModelSeqBindingName
        }
    let rec recursiveCase = function
      | BaseBindingData d -> d |> baseCase |> BaseBindingData
      | CachingData d -> d |> recursiveCase |> CachingData
      | ValidationData d -> ValidationData {
          BindingData = recursiveCase d.BindingData
          Validate = d.Validate
        }
      | LazyData d -> LazyData {
          BindingData = d.BindingData
          Get = d.Get
          Set = fun a m -> f (d.Set a m) m
          Equals = d.Equals
        }
      | AlterMsgStreamData d -> AlterMsgStreamData {
          BindingData = d.BindingData
          Get = d.Get
          Set = fun a m -> f (d.Set a m) m
          AlterMsgStream = d.AlterMsgStream
        }
    recursiveCase

  let mapMsg f = mapMsgWithModel (fun a _ -> f a)

  let setMsgWithModel f = mapMsgWithModel (fun _ m -> f m)
  let setMsg msg = mapMsg (fun _ -> msg)

  let addCaching b = b |> CachingData
  let addValidation validate b = { BindingData = b; Validate = validate } |> ValidationData
  let addLazy (equals: 'model -> 'model -> bool) b =
      { BindingData = b |> mapModel LanguagePrimitives.IntrinsicFunctions.UnboxFast |> mapMsg (box >> nonNull)
        Get = (box >> nonNull)
        Set = fun (dMsg: obj) _ -> unbox dMsg
        Equals = fun m1 m2 -> equals (unbox m1) (unbox m2)
      } |> LazyData
  let alterMsgStream
      (alteration: ('dispatchMsg -> unit) -> 'bindingMsg -> unit)
      (b: BindingData<'bindingModel, 'bindingMsg, 't>)
      : BindingData<'model, 'msg, 't> =
    { BindingData = b |> mapModel LanguagePrimitives.IntrinsicFunctions.UnboxFast |> mapMsg (box >> nonNull)
      Get = (box >> nonNull)
      Set = fun (dMsg: obj) _ -> unbox dMsg
      AlterMsgStream =
        fun (f: obj -> unit) ->
          let f' = box >> f
          let g = alteration f'
          unbox >> g
    } |> AlterMsgStreamData
  let addSticky (predicate: 'model -> bool) (binding: BindingData<'model, 'msg, 't>) =
    let mutable stickyModel = None
    let f newModel =
      if predicate newModel then
        stickyModel <- Some newModel
        newModel
      else
        stickyModel |> Option.defaultValue newModel
    binding |> mapModel f


  module Option =

    let box ma = ma |> Option.map (box >> nonNull) |> Option.toObj |> nonNull
    let unbox obj = obj |> Option.ofObj |> Option.map unbox

  module ValueOption =

    let box ma = ma |> ValueOption.map (box >> nonNull) |> ValueOption.toObj |> nonNull
    let unbox obj = obj |> ValueOption.ofObj |> ValueOption.map unbox


  module OneWay =

    let id<'T, 'msg> : BindingData<'T, 'msg, 'T> =
      { Get = id }
      |> OneWayData
      |> BaseBindingData

    let private mapFunctions
        mGet
        (d: OneWayData<'model, 'T>) =
      { d with Get = mGet d.Get }

    let measureFunctions
        mGet =
      mapFunctions
        (mGet "get")


  module OneWaySeq =

    let mapMinorTypes
        (outMapA: 'T -> 'a0)
        (outMapId: 'id -> 'id0)
        (inMapA: 'a0 -> 'T)
        (d: OneWaySeqData<'model, 'T, 'aCollection, 'id>) = {
      Get = d.Get >> Seq.map outMapA
      CreateCollection = Seq.map inMapA >> d.CreateCollection >> CollectionTarget.mapA outMapA inMapA
      GetId = inMapA >> d.GetId >> outMapId
      ItemEquals = fun a1 a2 -> d.ItemEquals (inMapA a1) (inMapA a2)
    }

    let boxMinorTypes d = d |> mapMinorTypes (box >> nonNull) (box >> nonNull) LanguagePrimitives.IntrinsicFunctions.UnboxFast

    let create itemEquals getId =
      { Get = (fun x -> upcast x)
        CreateCollection = ObservableCollection >> CollectionTarget.create
        ItemEquals = itemEquals
        GetId = getId }
      |> boxMinorTypes
      |> OneWaySeqData
      |> BaseBindingData

    let private mapFunctions
        mGet
        mGetId
        mItemEquals
        (d: OneWaySeqData<'model, 'T, 'aCollection, 'id>) =
      { d with Get = mGet d.Get
               GetId = mGetId d.GetId
               ItemEquals = mItemEquals d.ItemEquals }

    let measureFunctions
        mGet
        mGetId
        mItemEquals =
      mapFunctions
        (mGet "get")
        (mGetId "getId")
        (mItemEquals "itemEquals")


  module OneWaySeqGrouped =

    let mapMinorTypes
        (outMapA: 'T -> 'a0)
        (outMapId: 'id -> 'id0)
        (outMapKey: 'key -> 'key0)
        (inMapA: 'a0 -> 'T)
        (inMapKey: 'key0 -> 'key)
        (d: OneWaySeqGroupedData<'model, 'T, 'aCollection, 'id, 'key>) = {
      Get = d.Get >> Seq.map outMapA
      CreateCollection = Seq.map inMapA >> d.CreateCollection >> GroupedCollectionTarget.mapA inMapA outMapKey inMapKey
      GetId = inMapA >> d.GetId >> outMapId
      GetKey = inMapA >> d.GetKey >> outMapKey
      KeyComparer = Comparer.Create (fun k1 k2 -> d.KeyComparer.Compare (inMapKey k1, inMapKey k2))
      ItemEquals = fun a1 a2 -> d.ItemEquals (inMapA a1) (inMapA a2)
    }

    let boxMinorTypes d =
      d |> mapMinorTypes
             (box >> nonNull) (box >> nonNull) (box >> nonNull)
             LanguagePrimitives.IntrinsicFunctions.UnboxFast LanguagePrimitives.IntrinsicFunctions.UnboxFast

    let createWithComparer itemEquals getId getGrouppingKey keyComparer =
      { Get = (fun x -> upcast x)
        CreateCollection =
          fun items -> items.ToObservableLookup<'key,_>(keyComparer, Func<_,'key>(getGrouppingKey)) |> GroupedCollectionTarget.create
        ItemEquals = itemEquals
        GetId = getId
        GetKey = getGrouppingKey
        KeyComparer = keyComparer }
      |> boxMinorTypes
      |> OneWaySeqGroupedData
      |> BaseBindingData

    let create itemEquals getId getGrouppingKey (compareKeys : ('key -> 'key -> int) voption) =
      let comparer =
        match compareKeys with
        | ValueNone -> Comparer<'key>.Default
        | ValueSome compareKeys -> Comparer.Create (Comparison compareKeys)
      createWithComparer itemEquals getId getGrouppingKey comparer

    let private mapFunctions
        mGet
        mGetId
        mItemEquals
        (d: OneWaySeqGroupedData<'model, 'T, 'aCollection, 'id, 'GrouppingKey>) =
      { d with Get = mGet d.Get
               GetId = mGetId d.GetId
               ItemEquals = mItemEquals d.ItemEquals }

    let measureFunctions
        mGet
        mGetId
        mItemEquals =
      mapFunctions
        (mGet "get")
        (mGetId "getId")
        (mItemEquals "itemEquals")


  module TwoWay =

    let id<'T> : BindingData<'T, 'T, 'T> =
      { TwoWayData.Get = id
        Set = Func2.id1 }
      |> TwoWayData
      |> BaseBindingData

    let private mapFunctions
        mGet
        mSet
        (d: TwoWayData<'model, 'msg, 'T>) =
      { d with Get = mGet d.Get
               Set = mSet d.Set }

    let measureFunctions
        mGet
        mSet =
      mapFunctions
        (mGet "get")
        (mSet "set")


  module TwoWaySeq =

    let mapMinorTypes
        (outMapA: 'T -> 'a0)
        (outMapId: 'id -> 'id0)
        (inMapA: 'a0 -> 'T)
        (d: TwoWaySeqData<'model, 'msg, 'T, 'aCollection, 'id>) = {
      Get = d.Get >> Seq.map outMapA
      CreateCollection = Seq.map inMapA >> d.CreateCollection >> CollectionTarget.mapA outMapA inMapA
      GetId = inMapA >> d.GetId >> outMapId
      ItemEquals = fun a1 a2 -> d.ItemEquals (inMapA a1) (inMapA a2)
      Update = d.Update
    }

    let boxMinorTypes d = d |> mapMinorTypes (box >> nonNull) (box >> nonNull) LanguagePrimitives.IntrinsicFunctions.UnboxFast

    let create get itemEquals getId update =
      { Get = get
        CreateCollection = ObservableCollection >> CollectionTarget.create
        ItemEquals = itemEquals
        GetId = getId
        Update = update }
      |> boxMinorTypes
      |> TwoWaySeqData
      |> BaseBindingData

    let private mapFunctions
        mGet
        mGetId
        mItemEquals
        (d: TwoWaySeqData<'model, 'msg, 'T, 'aCollection, 'id>) =
      { d with Get = mGet d.Get
               GetId = mGetId d.GetId
               ItemEquals = mItemEquals d.ItemEquals }

    let measureFunctions
        mGet
        mGetId
        mItemEquals =
      mapFunctions
        (mGet "get")
        (mGetId "getId")
        (mItemEquals "itemEquals")


  module Cmd =

    let createWithParam exec canExec : BindingData<'model, 'msg, ICommand> =
      { Exec = exec
        CanExec = canExec }
      |> CmdData
      |> BaseBindingData

    let private mapFunctions
        mExec
        mCanExec
        (d: CmdData<'model, 'msg>) =
      { d with Exec = mExec d.Exec
               CanExec = mCanExec d.CanExec }

    let measureFunctions
        mExec
        mCanExec =
      mapFunctions
        (mExec "exec")
        (mCanExec "canExec")


  module SubModelSelectedItem =

    let mapMinorTypes
        (outMapId: 'id -> 'id0)
        (inMapId: 'id0 -> 'id)
        (d: SubModelSelectedItemData<'model, 'msg, 'id>) = {
      Get = d.Get >> ValueOption.map outMapId
      Set = ValueOption.map inMapId >> d.Set
      SubModelSeqBindingName = d.SubModelSeqBindingName
    }

    let boxMinorTypes d = d |> mapMinorTypes (box >> nonNull) LanguagePrimitives.IntrinsicFunctions.UnboxFast

    let create subModelSeqBindingName =
      { Get = id
        Set = Func2.id1
        SubModelSeqBindingName = subModelSeqBindingName }
      |> boxMinorTypes
      |> SubModelSelectedItemData
      |> BaseBindingData

    let private mapFunctions
        mGet
        mSet
        (d: SubModelSelectedItemData<'model, 'msg, 'id>) =
      { d with Get = mGet d.Get
               Set = mSet d.Set }

    let measureFunctions
        mGet
        mSet =
      mapFunctions
        (mGet "get")
        (mSet "set")


  module SubModel =

    let mapMinorTypes
        (outMapBindingModel: 'bindingModel -> 'bindingModel0)
        (outMapBindingMsg: 'bindingMsg -> 'bindingMsg0)
        (inMapBindingModel: 'bindingModel0 -> 'bindingModel)
        (inMapBindingMsg: 'bindingMsg0 -> 'bindingMsg)
        (d: SubModelData<'model, 'msg, 'bindingModel, 'bindingMsg, 'vm>) = {
      GetModel = d.GetModel >> ValueOption.map outMapBindingModel
      CreateViewModel = fun args -> d.CreateViewModel(args |> ViewModelArgs.map inMapBindingModel outMapBindingMsg)
      UpdateViewModel = fun (vm, m) -> (vm, inMapBindingModel m) |> d.UpdateViewModel
      ToMsg = fun m bMsg -> d.ToMsg m (inMapBindingMsg bMsg)
    }

    let boxMinorTypes d =
      d |> mapMinorTypes
             (box >> nonNull) (box >> nonNull)
             LanguagePrimitives.IntrinsicFunctions.UnboxFast LanguagePrimitives.IntrinsicFunctions.UnboxFast

    let create createViewModel updateViewModel =
      { GetModel = id
        CreateViewModel = createViewModel
        UpdateViewModel = updateViewModel
        ToMsg = Func2.id2 }
      |> boxMinorTypes
      |> SubModelData
      |> BaseBindingData

    let private mapFunctions
        mGetModel
        mGetBindings
        mUpdateViewModel
        mToMsg
        (d: SubModelData<'model, 'msg, 'bindingModel, 'bindingMsg, 'vm>)
        : SubModelData<'model,'msg,'bindingModel,'bindingMsg,'vm> =
      { d with GetModel = mGetModel d.GetModel
               CreateViewModel = mGetBindings d.CreateViewModel
               UpdateViewModel = mUpdateViewModel d.UpdateViewModel
               ToMsg = mToMsg d.ToMsg }

    let measureFunctions
        mGetModel
        mGetBindings
        mUpdateViewModel
        mToMsg =
      mapFunctions
        (mGetModel "getSubModel") // sic: "getModel" would be following the pattern
        (mGetBindings "bindings") // sic: "getBindings" would be following the pattern
        (mUpdateViewModel "updateViewModel")
        (mToMsg "toMsg")


  module SubModelWin =

    let mapMinorTypes
        (outMapBindingModel: 'bindingModel -> 'bindingModel0)
        (outMapBindingMsg: 'bindingMsg -> 'bindingMsg0)
        (inMapBindingModel: 'bindingModel0 -> 'bindingModel)
        (inMapBindingMsg: 'bindingMsg0 -> 'bindingMsg)
        (d: SubModelWinData<'model, 'msg, 'bindingModel, 'bindingMsg, 'vm>) = {
      GetState = d.GetState >> WindowState.map outMapBindingModel
      CreateViewModel = fun args -> d.CreateViewModel(args |> ViewModelArgs.map inMapBindingModel outMapBindingMsg)
      UpdateViewModel = fun (vm, m) -> d.UpdateViewModel (vm, inMapBindingModel m)
      ToMsg = fun m bMsg -> d.ToMsg m (inMapBindingMsg bMsg)
      GetWindow = d.GetWindow
      OnCloseRequested = d.OnCloseRequested
    }

    let boxMinorTypes d =
      d |> mapMinorTypes
             (box >> nonNull) (box >> nonNull)
             LanguagePrimitives.IntrinsicFunctions.UnboxFast LanguagePrimitives.IntrinsicFunctions.UnboxFast

    let create getState createViewModel updateViewModel toMsg getWindow onCloseRequested =
      { GetState = getState
        CreateViewModel = createViewModel
        UpdateViewModel = updateViewModel
        ToMsg = toMsg
        GetWindow = getWindow
        OnCloseRequested = onCloseRequested }
      |> boxMinorTypes
      |> SubModelWinData
      |> BaseBindingData

    let private mapFunctions
        mGetState
        mGetBindings
        mUpdateViewModel
        mToMsg
        mGetWindow
        mOnCloseRequested
        (d: SubModelWinData<'model, 'msg, 'bindingModel, 'bindingMsg, 'vm>) =
      { d with GetState = mGetState d.GetState
               CreateViewModel = mGetBindings d.CreateViewModel
               UpdateViewModel = mUpdateViewModel d.UpdateViewModel
               ToMsg = mToMsg d.ToMsg
               GetWindow = mGetWindow d.GetWindow
               OnCloseRequested = mOnCloseRequested d.OnCloseRequested }

    let measureFunctions
        mGetState
        mGetBindings
        mUpdateViewModel
        mToMsg =
      mapFunctions
        (mGetState "getState")
        (mGetBindings "bindings") // sic: "getBindings" would be following the pattern
        (mUpdateViewModel "updateViewModel")
        (mToMsg "toMsg")
        id // sic: could measure GetWindow
        id // sic: could measure OnCloseRequested


  module SubModelSeqUnkeyed =

    let mapMinorTypes
        (outMapBindingModel: 'bindingModel -> 'bindingModel0)
        (outMapBindingMsg: 'bindingMsg -> 'bindingMsg0)
        (outMapBindingViewModel: 'vm -> 'vm0)
        (inMapBindingModel: 'bindingModel0 -> 'bindingModel)
        (inMapBindingMsg: 'bindingMsg0 -> 'bindingMsg)
        (inMapBindingViewModel: 'vm0 -> 'vm)
        (d: SubModelSeqUnkeyedData<'model, 'msg, 'bindingModel, 'bindingMsg, 'vm, 'vmCollection>) = {
      GetModels = d.GetModels >> Seq.map outMapBindingModel
      CreateViewModel = fun args -> d.CreateViewModel(args |> ViewModelArgs.map inMapBindingModel outMapBindingMsg) |> outMapBindingViewModel
      CreateCollection = Seq.map inMapBindingViewModel >> d.CreateCollection >> CollectionTarget.mapA outMapBindingViewModel inMapBindingViewModel
      UpdateViewModel = fun (vm, m) -> d.UpdateViewModel (inMapBindingViewModel vm, inMapBindingModel m)
      ToMsg = fun m (idx, bMsg) -> d.ToMsg m (idx, (inMapBindingMsg bMsg))
    }

    let boxMinorTypes d =
      d |> mapMinorTypes
             (box >> nonNull) (box >> nonNull) (box >> nonNull)
             LanguagePrimitives.IntrinsicFunctions.UnboxFast LanguagePrimitives.IntrinsicFunctions.UnboxFast LanguagePrimitives.IntrinsicFunctions.UnboxFast

    let create createViewModel updateViewModel =
      { GetModels = (fun x -> upcast x)
        CreateViewModel = createViewModel
        CreateCollection = ObservableCollection >> CollectionTarget.create
        UpdateViewModel = updateViewModel
        ToMsg = Func2.id2 }
      |> boxMinorTypes
      |> SubModelSeqUnkeyedData
      |> BaseBindingData

    let private mapFunctions
        mGetModels
        mGetBindings
        mCreateCollection
        mUpdateViewModel
        mToMsg
        (d: SubModelSeqUnkeyedData<'model, 'msg, 'bindingModel, 'bindingMsg, 'vm, 'vmCollection>) =
      { d with GetModels = mGetModels d.GetModels
               CreateViewModel = mGetBindings d.CreateViewModel
               CreateCollection = mCreateCollection d.CreateCollection
               UpdateViewModel = mUpdateViewModel d.UpdateViewModel
               ToMsg = mToMsg d.ToMsg }

    let measureFunctions
        mGetModels
        mGetBindings
        mCreateCollection
        mUpdateViewModel
        mToMsg =
      mapFunctions
        (mGetModels "getSubModels") // sic: "getModels" would follow the pattern
        (mGetBindings "bindings") // sic: "getBindings" would follow the pattern
        (mCreateCollection "createCollection")
        (mUpdateViewModel "updateViewModel")
        (mToMsg "toMsg")


  module SubModelSeqKeyed =

      let mapMinorTypes
          (outMapBindingModel: 'bindingModel -> 'bindingModel0)
          (outMapBindingMsg: 'bindingMsg -> 'bindingMsg0)
          (outMapBindingViewModel: 'vm -> 'vm0)
          (outMapId: 'id -> 'id0)
          (inMapBindingModel: 'bindingModel0 -> 'bindingModel)
          (inMapBindingMsg: 'bindingMsg0 -> 'bindingMsg)
          (inMapBindingViewModel: 'vm0 -> 'vm)
          (inMapId: 'id0 -> 'id)
          (d: SubModelSeqKeyedData<'model, 'msg, 'bindingModel, 'bindingMsg, 'vm, 'vmCollection, 'id>) = {
        GetSubModels = d.GetSubModels >> Seq.map outMapBindingModel
        CreateViewModel = fun args -> d.CreateViewModel(args |> ViewModelArgs.map inMapBindingModel outMapBindingMsg) |> outMapBindingViewModel
        CreateCollection = Seq.map inMapBindingViewModel >> d.CreateCollection >> CollectionTarget.mapA outMapBindingViewModel inMapBindingViewModel
        UpdateViewModel = fun (vm, m) -> (inMapBindingViewModel vm, inMapBindingModel m) |> d.UpdateViewModel
        ToMsg = fun m (id, bMsg) -> d.ToMsg m ((inMapId id), (inMapBindingMsg bMsg))
        BmToId = inMapBindingModel >> d.BmToId >> outMapId
        VmToId = fun vm -> vm |> inMapBindingViewModel |> d.VmToId |> outMapId
      }

      let boxMinorTypes d =
        d |> mapMinorTypes
              (box >> nonNull) (box >> nonNull) (box >> nonNull) (box >> nonNull)
              LanguagePrimitives.IntrinsicFunctions.UnboxFast LanguagePrimitives.IntrinsicFunctions.UnboxFast LanguagePrimitives.IntrinsicFunctions.UnboxFast LanguagePrimitives.IntrinsicFunctions.UnboxFast

      let create createViewModel updateViewModel bmToId vmToId =
        { GetSubModels = (fun x -> upcast x)
          CreateViewModel = createViewModel
          CreateCollection = ObservableCollection >> CollectionTarget.create
          UpdateViewModel = updateViewModel
          ToMsg = Func2.id2
          BmToId = bmToId
          VmToId = vmToId }
        |> boxMinorTypes
        |> SubModelSeqKeyedData
        |> BaseBindingData

      let private mapFunctions
          mGetSubModels
          mGetBindings
          mCreateCollection
          mUpdateViewModel
          mToMsg
          mGetId
          mGetVmId
          (d: SubModelSeqKeyedData<'model, 'msg, 'bindingModel, 'bindingMsg, 'vm, 'vmCollection, 'id>) =
        { d with GetSubModels = mGetSubModels d.GetSubModels
                 CreateViewModel = mGetBindings d.CreateViewModel
                 CreateCollection = mCreateCollection d.CreateCollection
                 UpdateViewModel = mUpdateViewModel d.UpdateViewModel
                 ToMsg = mToMsg d.ToMsg
                 BmToId = mGetId d.BmToId
                 VmToId = mGetVmId d.VmToId }

      let measureFunctions
          mGetSubModels
          mGetBindings
          mCreateCollection
          mUpdateViewModel
          mToMsg
          mGetId
          mGetVmId =
        mapFunctions
          (mGetSubModels "getSubModels")
          (mGetBindings "getBindings")
          (mCreateCollection "createCollection")
          (mUpdateViewModel "updateViewModel")
          (mToMsg "toMsg")
          (mGetId "getId")
          (mGetVmId "getVmId")


  module Validation =

    let private mapFunctions
        mValidate
        (d: ValidationData<'model, 'msg, 't>) =
      { d with Validate = mValidate d.Validate }

    let measureFunctions
        mValidate =
      mapFunctions
        (mValidate "validate")

  module Lazy =

    let private mapFunctions
        mGet
        mSet
        mEquals
        (d: LazyData<'model, 'msg, 'bindingModel, 'bindingMsg, 't>) =
      { d with Get = mGet d.Get
               Set = mSet d.Set
               Equals = mEquals d.Equals }

    let measureFunctions
        mGet
        mSet
        mEquals =
      mapFunctions
        (mGet "get")
        (mSet "set")
        (mEquals "equals")
