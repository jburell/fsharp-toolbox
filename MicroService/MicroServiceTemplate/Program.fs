namespace MicroServiceTemplate

open Saturn
open MicroServiceTemplate.Application

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main _args =
        run app
        exitCode
