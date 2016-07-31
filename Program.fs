module MiniSuave
open Suave.Http
open Suave.Console
open Suave.Successful

[<EntryPoint>]
let main argv = 
    let request = {Type = Suave.Http.GET; Route = "/"}
    let response = {Status = 200; Content = ""}
    let context = {Request = request; Response = response}
    executeInLoop context (OK "Hello")
    0 // return an integer exit code
