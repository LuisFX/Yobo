module Yobo.Server.Core.Domain

open System

module CmdArgs =
    
    type AddCredits = {
        UserId : Guid
        Credits : int
        Expiration : DateTimeOffset
    }
    
    type SetExpiration = {
        UserId : Guid
        Expiration : DateTimeOffset
    }
    
    type CreateLesson = {
        Id : Guid
        StartDate : DateTimeOffset
        EndDate : DateTimeOffset
        Name : string
        Description : string
        Capacity : int
    }
    
    type CreateWorkshop = {
        Id : Guid
        StartDate : DateTimeOffset
        EndDate : DateTimeOffset
        Name : string
        Description : string
    }
    
    type CreateOnlineLesson = {
        Id : Guid
        StartDate : DateTimeOffset
        EndDate : DateTimeOffset
        Name : string
        Description : string
        Capacity : int
    }
    
    type ChangeLessonDescription = {
        Id : Guid
        Name : string
        Description : string
    }

//
//    type AddReservation = {
//        Id : Guid
//        UserId : Guid
//        Count : int
//        UseCredits : bool
//    }
//
//    type CancelReservation = {
//        Id : Guid
//        UserId : Guid
//    }
//
//    type CancelLesson = {
//        Id : Guid
//    }
//

//
//    type DeleteWorkshop = {
//        Id : Guid
//    }
//
//
//    type WithdrawCredits = {
//        UserId : Guid
//        Amount : int
//        LessonId : Guid
//    }
//
//    type RefundCredits = {
//        UserId : Guid
//        Amount : int
//        LessonId : Guid
//    }
//
//    type BlockCashReservations = {
//        UserId : Guid
//        Expires : DateTimeOffset
//        LessonId : Guid
//    }
//
//    type UnblockCashReservations = {
//        UserId : Guid
//    }
//

//
//    type DeleteLesson = {
//        Id : Guid
//    }
//
//    type UpdateLesson = {
//        Id : Guid
//        StartDate : DateTimeOffset
//        EndDate : DateTimeOffset
//        Name : string
//        Description : string
//    }


type Event =
    | CreditsAdded of CmdArgs.AddCredits
    | ExpirationSet of CmdArgs.SetExpiration
    | LessonCreated of CmdArgs.CreateLesson
    | WorkshopCreated of CmdArgs.CreateWorkshop
    | OnlineLessonCreated of CmdArgs.CreateOnlineLesson
    | LessonDescriptionChanged of CmdArgs.ChangeLessonDescription
    
//    | ReservationAdded of CmdArgs.AddReservation
//    | ReservationCancelled of CmdArgs.CancelReservation
//    | LessonCancelled of CmdArgs.CancelLesson
//    | CreditsWithdrawn of CmdArgs.WithdrawCredits
//    | CreditsRefunded of CmdArgs.RefundCredits
//    | CashReservationsBlocked of CmdArgs.BlockCashReservations
//    | CashReservationsUnblocked of CmdArgs.UnblockCashReservations
//    | WorkshopDeleted of CmdArgs.DeleteWorkshop
//    | LessonDeleted of CmdArgs.DeleteLesson
