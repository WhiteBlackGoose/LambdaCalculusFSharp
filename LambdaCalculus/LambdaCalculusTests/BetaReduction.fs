module Tests.BetaReduction

open LambdaCalculus.Atoms
open LambdaCalculus.Parsing
open Xunit

let assertBeta old _new =
    match parse old, parse _new with
    | (Ok old, Ok _new) ->
        Assert.Equal(_new, βReduce old)
    | _ ->
        Assert.False(true)

[<Fact>]
let ``Test β-reduction 1`` () =
    assertBeta "xx" "xx"

[<Fact>]
let ``Test β-reduction 2`` () =
    assertBeta @"\x.x" @"\x.x"