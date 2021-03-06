open System
open DbUp

[<EntryPoint>]
let main args =
    let connectionString = args.[0]
    let scriptsPath = args.[1]
    printfn "Connection string: %s" args.[0]
    printfn "Database scripts location: %s" args.[1]
    let engine =
            DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsFromFileSystem(scriptsPath)
                .JournalToSqlTable("dbo", "_SchemaVersions")
                .LogToConsole()
                .Build()
    
    let result = engine.PerformUpgrade();
    match result.Successful with
    | true ->
        Console.ForegroundColor <- ConsoleColor.Green
        Console.WriteLine "Success!"
        Console.ResetColor()
        0
    | false ->
        Console.ForegroundColor <- ConsoleColor.Red
        Console.WriteLine result.Error
        Console.ResetColor()
        -1