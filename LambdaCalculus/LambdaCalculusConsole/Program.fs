// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open LambdaCalculus.Utils
open LambdaCalculus.Parsing
open LambdaCalculus.Output
open LambdaCalculus.Atoms


let a = Ok (Variable 'x')
let b = Ok (Variable 'x')

let equal (a, b) =
    a.Equals(b)

printfn $"{a = b}"
printfn $"{a.Equals(b)}"
printfn $"{equal(a, b)}"
