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

[<Fact>]
let ``Test η-reduction 6`` () =
    assertEta @"(\x.xx)(\y.yy)" "(\x.xx)(\y.yy)"

[<Fact>]
let ``Test η-reduction 7`` () =
    assertEta @"\x.xx" "\x.xx"

[<Fact>]
let ``Test η-reduction 8`` () =
    assertEta @"\x.(ebac)x" "ebac"

[<Fact>]
let ``Test η-reduction 9`` () =
    assertEta @"\x.(ebaxc)x" @"\x.(ebaxc)x"

[<Fact>]
let ``Test η-reduction 10`` () =
    assertEta @"\x.(eba(\x.x)c)x" @"(eba(\x.x)c)"

[<Fact>]
let ``Test η-reduction 11`` () =
    assertEta @"\b.(\x.ax)(\x.bx)" "a"
