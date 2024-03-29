﻿module Tests.Substitution

open LambdaCalculus.Atoms
open LambdaCalculus.Parsing
open Xunit

let assertSub s old _new expected =
    match (parse s, parse _new, parse expected) with
    | (Ok s, Ok _new, Ok expected) ->
        let sub = substitute old _new s
        Assert.Equal(expected, sub)
    | _ ->
        Assert.False(true)


[<Fact>]
let ``Test substitute 1`` () =
    assertSub "x" 'x' "y" "y"

[<Fact>]
let ``Test substitute 2`` () =
    assertSub "xx" 'x' "y" "yy"

[<Fact>]
let ``Test substitute 3`` () =
    assertSub @"\x.x" 'x' "y" @"\x.x"

[<Fact>]
let ``Test substitute 4`` () =
    assertSub @"(\y.y)y" 'y' "z" @"(\y.y)z"

[<Fact>]
let ``Test substitute 5`` () =
    assertSub @"\x.y" 'y' "x" @"\z.x"

[<Fact>]
let ``Test substitute 6`` () =
    assertSub @"\x.xy" 'y' "x" @"\z.zx"