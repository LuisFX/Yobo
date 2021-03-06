module Yobo.Server.Auth.DbEventHandler

open Domain
open Microsoft.Data.SqlClient
open FSharp.Control.Tasks
open Yobo.Server.Auth
    
let handle (conn:SqlConnection) (e:Event) =
    task {
        match e with
        | Registered args -> do! args |> Database.Updates.registered conn
        | Activated args -> do! args |> Database.Updates.activated conn
        | PasswordResetInitiated args -> do! args |> Database.Updates.passwordResetInitiated conn
        | PasswordResetComplete args -> do! args |> Database.Updates.passwordResetComplete conn
        | ActivationKeyRegenerated args -> do! args |> Database.Updates.activationKeyRegenerated conn
        | SubscribedToNewsletters args -> do! args |> Database.Updates.subscribedToNewsletters conn
    }