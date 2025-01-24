namespace Elmish.Uno

open System
open System.Collections.ObjectModel
open System.Threading.Tasks
open Microsoft.UI.Xaml.Data

type IncrementalLoadingCollection<'t> =
  inherit ObservableCollection<'t>

  val has : unit -> bool
  val load : uint * (uint -> unit) -> unit

  new (hasMoreItems, loadMoreItems)
    =
    {
      inherit ObservableCollection<'t> ()
      has = hasMoreItems
      load = loadMoreItems
    }

  new (collection : 't seq, hasMoreItems, loadMoreItems)
    =
    {
      inherit ObservableCollection<'t> (collection)
      has = hasMoreItems
      load = loadMoreItems
    }

  new (list : 't ResizeArray, hasMoreItems, loadMoreItems)
    =
    {
      inherit ObservableCollection<'t> (list)
      has = hasMoreItems
      load = loadMoreItems
    }

  interface ISupportIncrementalLoading with

    member this.HasMoreItems = this.has ()

    member this.LoadMoreItemsAsync (count) =
      let tcs = TaskCompletionSource<uint> ()
      task {
        this.load (count, fun count -> tcs.SetResult (count))
        try
          let! count = tcs.Task
          return LoadMoreItemsResult (Count = count)
        with _ ->
          return LoadMoreItemsResult ()
      }
      |> _.AsAsyncOperation()
