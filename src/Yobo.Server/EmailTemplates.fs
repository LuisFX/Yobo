module Yobo.Server.EmailTemplates

open System
open Yobo.Shared

let private getRegister activateLink =
    """
    <h2>Welcome to the booking system</h2>
    <p>
        To activate your account, please click on the link below: <a href="{{{activate}}}">{{{activate}}}</a>
    </p>
    <p>
        We wish you a beautiful day.
    </p>
    <hr style="margin-top: 40px;" />
    <p>
        <i>This message was automatically generated by the reservation system Yobo Yoga.</i>
    </p>
    """ |> (fun x -> x.Replace("{{{activate}}}", activateLink))
    
let private getPasswordResetInit resetLink =
    """
    <h2>Hello!</h2>
    <p>
        Someone (apparently you) requested a password reset to the reservation system Yobo Yoga. If you want to change your password,
        please click on the link below <a href="{{{reset}}}">{{{reset}}}</a>.
    </p>
    <p>
        We wish you a beautiful day.
    </p>
    <hr style="margin-top: 40px;" />
    <p>
        <i>This message was automatically generated by the reservation system Yobo Yoga.</i>
    </p>
    """ |> (fun x -> x.Replace("{{{reset}}}", resetLink))

type EmailTemplateBuilder = {
    RegisterEmailMessage : Guid -> string
    PasswordResetEmailMessage : Guid -> string
}

let getDefault baseUri = {
    RegisterEmailMessage =
        string
        >> sprintf "/%s/%s" ClientPaths.AccountActivation
        >> fun link -> Uri(baseUri, link)
        >> string
        >> getRegister
    PasswordResetEmailMessage =
        string
        >> sprintf "/%s/%s" ClientPaths.ResetPassword
        >> fun link -> Uri(baseUri, link)
        >> string
        >> getPasswordResetInit
}