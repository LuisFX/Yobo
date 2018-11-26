module Yobo.Core.DbEventHandler

let private safeExecute (ctx:ReadDb.Db.dataContext) (fn:ReadDb.Db.dataContext -> unit) =
    try
        ctx |> fn |> Ok
    with ex -> Error (ex)

let getHandler (connString:string) =
    let ctx = ReadDb.Db.GetDataContext(connString)

    let handle cmd =
        match cmd with
        | CoreEvent.Users e -> e |> Users.DbEventHandler.handle |> safeExecute ctx
    handle