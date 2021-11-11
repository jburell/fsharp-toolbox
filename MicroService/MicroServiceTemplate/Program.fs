namespace MicroServiceTemplate

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open MicroServiceTemplate.Controllers
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open MicroServiceTemplate.Controllers

open Saturn
open Giraffe.Core

type Config = {
    connectionString : string
}

module Program =
    let exitCode = 0

    let CreateHostBuilder args =
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(fun webBuilder ->
                webBuilder.UseStartup<Startup>() |> ignore
            )

    let apiRouter = router {
        not_found_handler (text "Api 404")
        forward "/weather" WeatherForecastHttpHandler.weatherForecastController
    }

    let appRouter = router {
        forward "/api" apiRouter
    }

    let app = application {
        error_handler (fun ex _ -> pipeline {
            set_status_code 500
            json {| description = "Internal server error" |}
        })
        use_router appRouter
        url "http://0.0.0.0:8085/"
        use_gzip
    }

    [<EntryPoint>]
    let main args =
        run app
        exitCode
