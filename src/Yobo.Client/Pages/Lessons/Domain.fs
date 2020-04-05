module Yobo.Client.Pages.Lessons.Domain

open System
open Yobo.Client.Forms
open Yobo.Client.Forms
open Yobo.Shared.Core.Admin.Communication
open Yobo.Shared.Errors
open Yobo.Shared.Core.Admin.Domain.Queries

type ActiveLessonModel = {
    Lesson : Lesson
    ChangeDescriptionForm : ValidatedForm<Request.ChangeLessonDescription>
    CancelLessonForm : ValidatedForm<Request.CancelLesson>
    DeleteLessonForm : ValidatedForm<Request.DeleteLesson>
}

module ActiveLessonModel =
    let init lsn = {
        Lesson = lsn
        ChangeDescriptionForm =
            ({ Id = lsn.Id; Name = lsn.Name; Description = lsn.Description } : Request.ChangeLessonDescription)
            |> ValidatedForm.init
        CancelLessonForm = ({ Id = lsn.Id } : Request.CancelLesson) |> ValidatedForm.init
        DeleteLessonForm = ({ Id = lsn.Id } : Request.DeleteLesson) |> ValidatedForm.init
    }
    
type ActiveWorkshopModel = {
    Workshop : Workshop
    DeleteWorkshopForm : ValidatedForm<Request.DeleteWorkshop>
}

module ActiveWorkshopModel =
    let init w = {
        Workshop = w
        DeleteWorkshopForm = ({ Id = w.Id } : Request.DeleteWorkshop) |> ValidatedForm.init
    }

type ActiveItemModel =
    | Lesson of ActiveLessonModel
    | Workshop of ActiveWorkshopModel
    | OnlineLesson of OnlineLesson

type ActiveLessonMsg =
    | ChangeLessonDescriptionFromChanged of Request.ChangeLessonDescription
    | ChangeLessonDescription
    | LessonDescriptionChanged of ServerResult<unit>
    | CancelLesson
    | LessonCancelled of ServerResult<unit>
    | DeleteLesson
    | LessonDeleted of ServerResult<unit>

type ActiveWorkshopMsg =
    | DeleteWorkshop
    | WorkshopDeleted of ServerResult<unit>

type ActiveItemMsg =
    | ActiveLessonMsg of ActiveLessonMsg
    | ActiveWorkshopMsg of ActiveWorkshopMsg

type ActiveForm =
    | LessonsForm
    | WorkshopsForm
    | OnlinesForm

type Model = {
    Lessons : Lesson list
    Workshops : Workshop list
    Onlines : OnlineLesson list
    WeekOffset : int
    SelectedDates : DateTimeOffset list
    
    ActiveForm : ActiveForm option
    LessonsForm : ValidatedForm<Request.CreateLessons>
    WorkshopsForm : ValidatedForm<Request.CreateWorkshops>
    OnlinesForm : ValidatedForm<Request.CreateOnlineLessons>
    
    ActiveItemModel : ActiveItemModel option
}

module Model =
    let init =
        {
            Lessons = []
            Workshops = []
            Onlines = []
            WeekOffset = 0
            SelectedDates = []
            ActiveForm = None
            LessonsForm = Request.CreateLessons.init |> ValidatedForm.init
            WorkshopsForm = Request.CreateWorkshops.init |> ValidatedForm.init
            OnlinesForm = Request.CreateOnlineLessons.init |> ValidatedForm.init
            ActiveItemModel = None
        }

type Msg =
    | Init
    | SelectActiveForm of ActiveForm option
    | ToggleDate of DateTimeOffset
    | WeekOffsetChanged of int
    | LoadLessons
    | LoadOnlineLessons
    | LoadWorkshops
    | LessonsLoaded of ServerResult<Lesson list>
    | OnlineLessonsLoaded of ServerResult<OnlineLesson list>
    | WorkshopsLoaded of ServerResult<Workshop list>
    | LessonsFormChanged of Request.CreateLessons
    | OnlineLessonsFormChanged of Request.CreateOnlineLessons
    | WorkshopsFormChanged of Request.CreateWorkshops
    | CreateLessons
    | CreateWorkshops
    | CreateOnlineLessons
    | LessonsCreated of ServerResult<unit>
    | WorkshopsCreated of ServerResult<unit>
    | OnlineLessonsCreated of ServerResult<unit>
    
    | SetActiveLesson of Lesson option
    | SetActiveWorkshop of Workshop option
    | SetActiveOnlineLesson of OnlineLesson option
    | ActiveItemMsg of ActiveItemMsg
    
    
