// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open ResultBuilder
open LambdaCalculus.Parsing
open LambdaCalculus.Output
open LambdaCalculus.Atoms


let res = opt {
    let! (lambda) = parse "x"
    return lambda
}

match res with
| Ok expr ->
    expr
    |> sprintLambda
    |> printfn "%s"

    expr
    |> betaReduce
    |> sprintLambda
    |> printfn "%s"
    (*
    match betaReduce expr with
    | None -> printfn "%s" "Can't be reduced"
    | Some reduced ->
        reduced
        |> sprintLambda
        |> printfn "%s"*)
| Error error ->
    printfn "%s" error