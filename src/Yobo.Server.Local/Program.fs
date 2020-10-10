module Yobo.Server.Local.Program

open System.Diagnostics
open System.IO
open System

type OS =
| Windows
| MacOS
| Linux
| Other of string

let getOsType =
    let os = Environment.OSVersion
    match os.Platform with
    | PlatformID.Win32NT
    | PlatformID.Win32S
    | PlatformID.Win32Windows
    | PlatformID.WinCE -> Windows
    | PlatformID.Unix -> Linux
    | PlatformID.MacOSX -> MacOS
    | a -> Other (a.ToString())

[<EntryPoint>]
let main _ =
    let d = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace(".Local",""))
    let p = ProcessStartInfo()
    p.WorkingDirectory <- d

    match getOsType with
    | MacOS
    | Linux ->
        printfn "Running on a *nix machine."
        p.FileName <- "func"
        p.Arguments <- "host start"
    | Windows ->
        printfn "Running on a windows machine."
        p.FileName <- "cmd.exe"
        p.Arguments <- "/K func host start"
    | Other a -> raise (Exception (sprintf "OS not supported! (%s)" a))

    Process.Start(p).WaitForExit()
    0