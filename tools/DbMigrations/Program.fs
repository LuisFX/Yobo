open System
open DbUp

[<EntryPoint>]
let main args =
    let connectionString = @"
      Server=127.0.0.1,1401;
      Database=Master;
      User Id=SA;
      Password=123!@#qweQWE
   "
    let scriptsPath = args.[0]
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