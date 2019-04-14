module Yobo.Shared.Calendar.Communication

open System
open Domain
open Yobo.Shared.Communication

let routeBuilder _ m = sprintf "/api/calendar/%s" m

type API = {
    GetLessonsForDateRange : SecuredParam<DateTimeOffset * DateTimeOffset> -> ServerResponse<Lesson list>
    GetWorkshopsForDateRange : SecuredParam<DateTimeOffset * DateTimeOffset> -> ServerResponse<Yobo.Shared.Domain.Workshop list>
    AddReservation : SecuredParam<AddReservation> -> ServerResponse<unit>
    CancelReservation : SecuredParam<Guid> -> ServerResponse<unit>
} 