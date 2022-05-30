module Tests.EtaReduction

open LambdaCalculus.Atoms
open LambdaCalculus.Parsing
open Xunit

let assertEta old _new =
    match parse old, parse _new with
    | (Ok old, Ok _new) ->
        Assert.Equal(_new, ηReduce old)
    | _ ->
        Assert.False(true)

[<Fact>]
let ``Test η-reduction 1`` () =
    assertEta @"\x.ex" "e"

[<Fact>]
let ``Test η-reduction 2`` () =
    assertEta @"(\x.ex)a" "ea"

[<Fact>]
let ``Test η-reduction 3`` () =
    assertEta @"(\x.ex)(\y.hy)" "eh"

[<Fact>]
let ``Test η-reduction 4`` () =
    assertEta @"\x.exe" "\x.exe"

[<Fact>]
let ``Test η-reduction 5`` () =
    assertEta @"\yx.ex" "\y.e"

