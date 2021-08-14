module Tests.AlphaEquality

open LambdaCalculus.Atoms
open LambdaCalculus.Parsing
open Xunit

let assertAlpha old _new =
    match parse old, parse _new with
    | (Ok old, Ok _new) ->
        Assert.True(_new, old)
    | _ ->
        Assert.False(true)