﻿module Tests.BetaReduction

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

[<Fact>]
let ``Test β-reduction 3`` () =
    assertBeta @"(\x.x)y" "y"

[<Fact>]
let ``Test β-reduction 4`` () =
    assertBeta @"(\x.x)(\y.y)z" "z"

[<Fact>]
let ``Test β-reduction 5`` () =
    assertBeta @"(\x.x)(\y.y)y" "y"

[<Fact>]
let ``Test β-reduction 6`` () =
    assertBeta @"(\x.xxy)y" "yyy"

[<Fact>]
let ``Test β-reduction 7`` () =
    assertBeta @"(\x.xx)yy" "yyy"

[<Fact>]
let ``Test β-reduction 8`` () =
    assertBeta @"(\x.xxx)abcd" "aaabcd"

[<Fact>]
let ``Test β-reduction 9`` () =
    assertBeta @"(\x.xx)(\y.yy)" @"(\x.xx)(\y.yy)"

[<Fact>]
let ``Test β-reduction 10`` () =
    assertBeta @"(\x.xxx)(\x.x)" @"\x.x"