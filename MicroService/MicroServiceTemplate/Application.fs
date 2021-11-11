module MicroServiceTemplate.Application

open System.Net
open Saturn
open MicroServiceTemplate.Api.Routing

let app = application {
    error_handler (fun _ex _ -> pipeline {
        set_status_code (int HttpStatusCode.InternalServerError)
        json {| description = "Internal server error" |}
    })
    use_router appRouter
    url "http://0.0.0.0:8085/"
    use_gzip
}