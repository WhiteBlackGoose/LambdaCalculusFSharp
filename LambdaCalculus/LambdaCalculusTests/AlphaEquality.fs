module Tests.AlphaEquality

open LambdaCalculus.Atoms
open LambdaCalculus.Parsing
open Xunit

let assertAlpha old _new =
    match parse old, parse _new with
    | (Ok old, Ok _new) ->
        αEqual old _new
    | _ ->
        Assert.False(true)
        raise (System.Exception ())

[<Fact>]
let ``Test α-equality 1`` () =
    assertAlpha "x" "x" |> Assert.True

[<Fact>]
let ``Test α-equality 2`` () =
    assertAlpha "x" "y" |> Assert.False

[<Fact>]
let ``Test α-equality 3`` () =
    assertAlpha @"\x.x" @"\y.y" |> Assert.True

[<Fact>]
let ``Test α-equality 4`` () =
    assertAlpha @"\x.y" @"\y.x" |> Assert.False

[<Fact>]
let ``Test α-equality 5`` () =
    assertAlpha @"\x.yyy" @"\y.yyy" |> Assert.False

[<Fact>]
let ``Test α-equality 6`` () =
    assertAlpha @"\x.zzz" @"\y.zzz" |> Assert.True