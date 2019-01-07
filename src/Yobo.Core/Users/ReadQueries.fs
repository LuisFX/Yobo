module Yobo.Core.Users.ReadQueries

open Yobo.Core
open System
open FSharp.Rop
open Yobo.Shared.Auth
open Yobo.Core.Data

type User = {
    Id : Guid
    Email : string
    FirstName : string
    LastName : string
    ActivatedUtc : DateTime option
}

type UserQueries<'a> = {
    GetById : Guid -> Result<User, 'a>
    GetAll : unit -> Result<User list, 'a>
}

let withError (fn:'a -> 'b) (q:UserQueries<'a>) = {
    GetById = q.GetById >> Result.mapError fn
    GetAll = q.GetAll >> Result.mapError fn

}

let internal userFromDbEntity (u:ReadDb.Db.dataContext.``dbo.UsersEntity``) =
    {
        Id = u.Id
        Email = u.Email
        FirstName = u.FirstName
        LastName = u.LastName
        ActivatedUtc = u.ActivatedUtc
    }

let private getById i (ctx:ReadDb.Db.dataContext) =
    query {
        for x in ctx.Dbo.Users do
        where (x.Id = i)
        select x
    }
    |> Data.oneOrError i
    <!> userFromDbEntity

let private getAll () (ctx:ReadDb.Db.dataContext) =
    query {
        for x in ctx.Dbo.Users do
        sortBy x.LastName
        select x
    }
    |> Seq.map userFromDbEntity
    |> Seq.toList

let createDefault (connString:string) =
    let ctx = ReadDb.Db.GetDataContext(connString)
    {
        GetById = getById >> Data.tryQueryResult ctx
        GetAll = getAll >> Data.tryQuery ctx
        
    }