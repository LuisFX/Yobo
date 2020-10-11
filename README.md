# About this project
State of this project is rather untypical for public GitHub projects so please read lines below to fully understand my intentions and your limitations: 

## Is it OSS project?
Not in typical meaning. It's "free to look" public project I've created and maintained for my wife's yoga class, which is used in production. I am the only maintainer and I don't look for any other contribution. Its evolution is and will be driven by business needs only.

## Why is it public?
As a proud member of F# community I wanted to publish how the real project is done using F#, Fable, Azure Functions and other cool stuff. Even if you cannot contribute directly I believe it could be handy (if you're interested) to see how real project evolves.

## What licence is it?
None. :) By [definition of missing licence](https://choosealicense.com/no-permission/) it means:

> Without a license, the default copyright laws apply, meaning that you retain all rights to your source code and no one may reproduce, distribute, or create derivative works from your work.

However let me be clear on this:

## Can I still use some parts of it for my own good?
Absolutely! And that's the reason I made this project public. Take any **code part** of the repository and do what you like. Really. It's built on the top of OSS libraries anyway, so I would be happy to somehow contribute back.

## So what's the limitation?
All graphic content (especially logo) and "Mindful Yoga" phrase are trademarks registered on [Úřad Průmyslového Vlastnictví](https://upv.cz/en.html) (Industrial Property Office in Czech Republic) so please avoid of reusing those.

## That's all?
Yup. Just clone the repo, delete trademarked `/src/Yobo.Client/public/img/logo.png`, rename "Mindful Yoga" to something else like "My Amazing Yoga Kickass Booking" (it's currently in [3 files](https://github.com/Dzoukr/Yobo/search?q=mindful) anyway, so no brainer here :-)) and we're all ok.


Here are the following steps to get the application configured and running:

## Run a docker container with sqlserver:

(from https://hub.docker.com/_/microsoft-mssql-server)

```
sudo docker pull microsoft/mssql-server-linux:2017-latest
```

Replace the password with your password
```
docker run \
-e 'ACCEPT_EULA=Y' \
-e 'MSSQL_SA_PASSWORD=YourSTRONG!Passw0rd' \
-p 1401:1433 \
--name sql1 \
-d microsoft/mssql-server-linux:2017-latest
```

## Run database migrations

In order to scaffold the database schema, you must run the DbMigrations project. The database scripts are located in the ./database directory

From tools/DbMigrations:

```
dotnet run <connectionString> <scriptsLocation>

eg;
dotnet run 'Server=127.0.0.1,1401; Database=Master; User Id=SA; Password=YourSTRONG!Passw0rd' '../../database'
```


## Add local.settings.json file with basic connectivity to the Yobo.Server directory:

Enter your information as appropriate. Pay attention to set the proper Jwt properties with your own values.

```
{
    "MailChimpApiKey": "ABCD",
    "ReadDbConnectionString": "Server=127.0.0.1,1401; Database=Master; User Id=SA; Password=YourSTRONG!Passw0rd",

    "AuthIssuer": "authIssuer",
    "AuthAudience": "http://localhost:8080",
    "AuthSecret": "http://localhost:8080",
    "AuthTokenLifetime":"60",
    "EmailsFromName":"name@domain.com",
    "MailjetApiKey": "mailjet",
    "MailjetSecretKey": "secretKey",

    "ServerBaseUrl": "http://localshost:1401",
    "AdminEmail": "admin@domain.com",
    "AdminPassword": "secret",

    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=True",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet"
    }
}
```

## Running on a *nix platform

The server has a local kickstart from Yobo.Server.Local. This program needs to handle the operating system to launch the proper process command to start the "Yobo.Server" local functions azure host.

I added modified the code file Program.fs to handle it here: https://gist.github.com/LuisFX/e90a0622946f28fc4a1d3b22e9ef431c

But basically, if you have mac or linux, you need to run this:
with this:
```
        p.FileName <- "func"
        p.Arguments <- "host start"
```
...instead of:
```
        p.FileName <- "cmd.exe"
        p.Arguments <- "/K func host start"
```

## Registering and Login with an admin user:
Register a user matching the email address listed in the above local.settings.json's AdminEmail key. Once an admin user is registered, login using the admin credentials.
