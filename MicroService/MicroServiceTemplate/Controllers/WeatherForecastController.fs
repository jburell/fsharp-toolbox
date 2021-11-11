namespace MicroServiceTemplate.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MicroServiceTemplate

open Giraffe

[<ApiController>]
[<Route("[controller]")>]
type WeatherForecastController () =
    inherit ControllerBase()

    let summaries =
        [|
            "Freezing"
            "Bracing"
            "Chilly"
            "Cool"
            "Mild"
            "Warm"
            "Balmy"
            "Hot"
            "Sweltering"
            "Scorching"
        |]

    [<HttpGet>]
    member _.Get() =
        let rng = System.Random()
        [|
            for index in 0..4 ->
                { Date = DateTime.Now.AddDays(float index)
                  TemperatureC = rng.Next(-20,55)
                  Summary = summaries.[rng.Next(summaries.Length)] }
        |]

module WeatherForecastHttpHandler =
    let weatherForecastController (next : HttpFunc) (ctx: HttpContext) : HttpFuncResult =
        let response = WeatherForecastController().Get()
        json response next ctx