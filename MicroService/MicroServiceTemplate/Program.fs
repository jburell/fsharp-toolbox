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

    let api = pipeline {
        plug acceptJson
        set_header "x-pipeline-type" "Api"
    }

    let apiRouter = router {
//        not_found_handler (text "Api 404")
        pipe_through api

        forward "/someApi" WeatherForecastHttpHandler.weatherForecastController
    }

    let appRouter = router {
        forward "/api" apiRouter
    }


    let endpointPipe = pipeline {
        plug head
        plug requestId
    }
    let app = application {
        pipe_through endpointPipe

//        error_handler (fun ex _ -> pipeline { render_html (InternalError.layout ex) })
        use_router appRouter
        url "http://0.0.0.0:8085/"
        memory_cache
        use_static "static"
        use_gzip
        use_config (fun _ -> {connectionString = "DataSource=database.sqlite"} ) //TODO: Set development time configuration
    }

    [<EntryPoint>]
    let main args =
//        CreateHostBuilder(args).Build().Run()
        run app
        exitCode
