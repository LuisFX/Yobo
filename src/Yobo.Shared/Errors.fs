module Yobo.Shared.Errors

open System
open Yobo.Shared.Validation

type AuthenticationError =
    | InvalidLoginOrPassword
    | InvalidOrExpiredToken
    | EmailAlreadyRegistered
    | AccountAlreadyActivatedOrNotFound
    | InvalidPasswordResetKey
    
module AuthenticationError =
    let explain = function
        | InvalidLoginOrPassword -> "Incorrectly filled in email or password."
        | InvalidOrExpiredToken -> "The token is not valid or has expired."
        | EmailAlreadyRegistered -> "This email is already registered in the system."
        | AccountAlreadyActivatedOrNotFound -> "This account has already been activated or an invalid activation key has been entered."
        | InvalidPasswordResetKey -> "The code for setting a new password is incorrect or has already been used."

type DomainError =
    | UserNotActivated
    | LessonCannotBeCancelled
    | LessonCannotBeDeleted
    | LessonCannotBeReserved
    | LessonReservationCannotBeCancelled

module DomainError =
    let explain = function
        | UserNotActivated -> "The user has not yet been activated."
        | LessonCannotBeCancelled -> "The lesson cannot be canceled."
        | LessonCannotBeDeleted -> "The lesson cannot be deleted."
        | LessonCannotBeReserved -> "The lesson cannot be booked."
        | LessonReservationCannotBeCancelled -> "You can no longer cancel a lesson reservation."

type ServerError =
    | Exception of string
    | Validation of ValidationError list
    | Authentication of AuthenticationError
    | DatabaseItemNotFound of Guid
    | Domain of DomainError

type ServerResult<'a> = Result<'a, ServerError>

exception ServerException of ServerError

module ServerError =
    let explain = function
        | Exception e -> e
        | Validation errs ->
            errs
            |> List.map ValidationError.explain
            |> String.concat ", "
        | Authentication e -> e |> AuthenticationError.explain
        | DatabaseItemNotFound i -> sprintf "Item with ID %A was not found in the database." i
        | Domain e -> e |> DomainError.explain
        
    let failwith (er:ServerError) = raise (ServerException er)
    
    let ofOption err (v:Option<_>) =
        v
        |> Option.defaultWith (fun _ -> err |> failwith)
    
    let ofResult<'a> (v:Result<'a,ServerError>) =
        match v with
        | Ok v -> v
        | Error e -> e |> failwith

    let validate (validationFn:'a -> ValidationError list) (value:'a) =
        match value |> validationFn with
        | [] -> value
        | errs -> errs |> Validation |> failwith