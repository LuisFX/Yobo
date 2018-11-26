module Yobo.Client.Register.View

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma
open Fable.Core.JsInterop

open Yobo.Client.Register.Domain
open Yobo.Shared
open Yobo.Shared.Text
open Yobo.Shared.Communication

let render (state : State) (dispatch : Msg -> unit) =
    
    let regInput typ msgType txt =
        let error = state.ValidationResult.TryGetFieldError txt
        let clr = if error.IsSome then Input.Color IsDanger else Input.Option.Props []
        let help = if error.IsSome then 
                    Help.help [ Help.Color IsDanger ]
                        [ str (error.Value |> Locale.errorToCz ) ]
                   else span [] []
        Control.div [] [
            typ [
                clr
                Input.Option.OnChange (fun e -> !!e.target?value |> msgType |> dispatch)
            ]
            help
        ]

    let lbl txt = Label.label [] [ str (Locale.toTitleCz txt) ]

    let btn isLogging =
        let content = if isLogging then i [ ClassName "fa fa-circle-o-notch fa-spin" ] [] else str (Locale.toTitleCz ToRegister)
        Control.div [] [
            Button.button 
                [ Button.Color IsPrimary; Button.IsFullWidth; Button.OnClick (fun _ -> dispatch Register)  ]
                [ content  ]
        ]

    let errorBox =
        match state.RegistrationResult with
        | Some (Error (ServerError.Exception(ex))) ->
            str ex
        | Some (Error (ServerError.DomainError(msg))) ->
            Notification.notification [ Notification.Color IsDanger ]
                [ str <| msg.ToString() ]
        | _ -> str ""
        
    
    let form = 
        div 
            [ ClassName "box"] 
            [
                Heading.h1 [ ] [ str (Locale.toTitleCz Registration) ]

                errorBox

                lbl FirstName
                regInput Input.text ChangeFirstName FirstName
                
                lbl LastName
                regInput Input.text ChangeLastName LastName

                lbl Email
                regInput Input.email ChangeEmail Email

                lbl Password
                regInput Input.password ChangePassword Password
                
                lbl SecondPassword
                regInput Input.password ChangeSecondPassword SecondPassword

                btn state.IsRegistering
                
                str (state.ToString())

            ]
   
    Hero.hero [ ]
        [ Hero.body [ ]
            [ Container.container 
                [ Container.IsFluid; Container.Props [ ClassName "has-text-centered"] ]
                [ Column.column 
                    [ Column.Width (Screen.All, Column.Is4); Column.Offset (Screen.All, Column.Is4) ] 
                    [ form ] 
                ] 
            ]
        ]