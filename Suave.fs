namespace Suave

module Http =
    type RequestType = GET | POST
    type Request = {
        Route : string
        Type : RequestType
    }
    type Response = {
        Content : string
        Status : int
    }
    type Context = {
        Request : Request
        Response : Response
    }

    type WebPart = Context -> Async<Context option>

module Successful =
    open Http

    let OK content context =
        {context with Response = {Content = content; Status = 200}}
        |> Some
        |> async.Return

module Console =
    open Http
    let execute inputContext webpart =
        async {
            let! outputContext = webpart inputContext
            match outputContext with
            | Some context ->
                printfn "========"
                printfn "Status: %d" context.Response.Status
                printfn "Content: %s" context.Response.Content
                printfn "========"
            | None ->
                printfn "No output"
        } |> Async.RunSynchronously

    let parseRequest (input : string) =
        let parts = input.Split [|';'|]
        match parts.[0] with
        | "GET" -> {Type = GET; Route = parts.[1]}
        | "POST" -> {Type = GET; Route = parts.[1]}
        | _ -> failwith "invalid request"

    let executeInLoop inputContext webpart =
        let mutable loop = true
        while loop do
            printf "enter request:"
            let input = System.Console.ReadLine()
            try
                if input = "exit" then
                    loop <- false
                else
                    let context = {inputContext with Request = parseRequest input}
                    execute context webpart
            with
                | ex ->
                    printfn "error: %s" ex.Message
