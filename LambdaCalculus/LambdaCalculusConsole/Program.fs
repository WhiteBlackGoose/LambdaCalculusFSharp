// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open LambdaCalculus.Utils
open LambdaCalculus.Parsing
open LambdaCalculus.Output
open LambdaCalculus.Atoms
open LambdaCalculus.ToCSharp

let rec inputAndRespond () =
    Console.ForegroundColor <- ConsoleColor.White
    let input = Console.ReadLine()
    match parse input with
    | Ok parsed ->
        Console.ForegroundColor <- ConsoleColor.Green
        printfn $"Parsed: {sprintLambda parsed}"
        printfn $"Beta-reduced: {sprintLambda (betaReduce parsed)}"
        printfn $"Eta-reduced: {sprintLambda (etaReduce parsed)}"
        printfn $"C# : {toCSharp parsed}"
        printfn ""
    | Error error ->
        Console.ForegroundColor <- ConsoleColor.Red
        printfn $"Parse error: {error}"
        printfn ""
    inputAndRespond ()

inputAndRespond ()