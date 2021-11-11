module MicroServiceTemplate.Api.Routing

open Saturn
open Giraffe.Core

open MicroServiceTemplate.Controllers

let apiRouter = router {
    not_found_handler (text "Api 404")
    forward "/weather" WeatherForecastHttpHandler.weatherForecastController
}

let appRouter = router {
    forward "/api" apiRouter
}