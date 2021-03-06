module Yobo.Client.Pages.Calendar.State

open Domain
open Elmish
open Yobo.Client.Server
open Yobo.Client.SharedView

let init () =
    {
        WeekOffset = 0
        Lessons = []
        Workshops = []
    }, Cmd.ofMsg (WeekOffsetChanged 0)

let update (props:{| creditsChanged:unit -> unit |}) (msg:Msg) (model:Model) : Model * Cmd<Msg> =
    match msg with
    | WeekOffsetChanged o -> { model with WeekOffset = o }, [ LoadLessons; LoadWorkshops ] |> List.map Cmd.ofMsg |> Cmd.batch
    | LoadLessons -> model, Cmd.OfAsync.eitherAsResult (onReservationsService (fun x -> x.GetLessons)) model.WeekOffset LessonsLoaded
    | LessonsLoaded res ->
        match res with
        | Ok lsns -> { model with Lessons = lsns }, Cmd.none
        | Error e -> model, e |> ServerResponseViews.showErrorToast
    | LoadWorkshops -> model, Cmd.OfAsync.eitherAsResult (onReservationsService (fun x -> x.GetWorkshops)) model.WeekOffset WorkshopsLoaded
    | WorkshopsLoaded res ->
        match res with
        | Ok xs -> { model with Workshops = xs }, Cmd.none
        | Error e -> model, e |> ServerResponseViews.showErrorToast
    | AddReservation res -> model, Cmd.OfAsync.eitherAsResult (onReservationsService (fun x -> x.AddReservation)) res ReservationAdded
    | ReservationAdded res ->
        match res with
        | Ok _ -> model, [ Cmd.ofSub (fun _ -> props.creditsChanged()); Cmd.ofMsg LoadLessons; (ServerResponseViews.showSuccessToast "Lekce úspěšně zarezervována") ] |> Cmd.batch
        | Error e ->
            model, e |> ServerResponseViews.showErrorToast
    | CancelReservation i -> model, Cmd.OfAsync.eitherAsResult (onReservationsService (fun x -> x.CancelReservation)) i ReservationCancelled
    | ReservationCancelled res ->
        match res with
        | Ok _ -> model, [ Cmd.ofSub (fun _ -> props.creditsChanged()); Cmd.ofMsg LoadLessons; (ServerResponseViews.showSuccessToast "Rezervace lekce úspěšně zrušena") ] |> Cmd.batch
        | Error e ->
            model, e |> ServerResponseViews.showErrorToast