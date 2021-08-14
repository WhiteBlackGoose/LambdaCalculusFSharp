module Tests.Parsing

open System
open Xunit
open LambdaCalculus.Atoms
open LambdaCalculus.Parsing

let resultToString r =
    match r with
    | Ok ok -> $"Ok ({ok})"
    | Error error -> $"Error ({error})"

let equal (a : Result<'a, 'b>, b : Result<'a, 'b>) =
    Assert.True(a.Equals(b), $"Expected: {resultToString b}\nActual: {resultToString a}")

[<Fact>]
let ``Test parser 1`` () =
    equal(
        parse "x",
        Variable 'x' |> Ok
        )

[<Fact>]
let ``Test parser 2`` () =
    equal(
        parse @"\x.x",
        Lambda('x', Variable 'x') |> Ok
        )

[<Fact>]
let ``Test parser 3`` () =
    equal(
        parse "xx",
        Applied(Variable 'x', Variable 'x') |> Ok
        )

[<Fact>]
let ``Test parser 4`` () =
    equal(
        parse @"(x)(\x.x)",
        Applied(Variable 'x', Lambda('x', Variable 'x')) |> Ok
        )

[<Fact>]
let ``Test parser 5`` () =
    equal(
        parse "yxzu",
        Applied(Applied(Applied(Variable 'y', Variable 'x'), Variable 'z'), Variable 'u') |> Ok
        )

[<Fact>]
let ``Test parser 6`` () =
    equal(
        parse "yxzu",
        Applied(Applied(Applied(Variable 'y', Variable 'x'), Variable 'z'), Variable 'u') |> Ok
        )

[<Fact>]
let ``Test parser 7`` () =
    equal(
        parse @"(\x.x)xy",
        Applied(Applied(Lambda('x', Variable 'x'), Variable 'x'), Variable 'y') |> Ok
        )

[<Fact>]
let ``Test parser 8`` () =
    equal(
        parse @"(x)(\x.(xy(\l.l)))xyz(\x.y)",
        Applied(
            Applied(
                Applied(
                    Applied(
                        Applied(
                            Variable 'x',
                            Lambda('x',
                                Applied(
                                    Applied(
                                        Variable 'x',
                                        Variable 'y'
                                    ),
                                    Lambda('l', Variable 'l')
                                )
                            )
                        ),
                        Variable 'x'
                    ),
                    Variable 'y'
                ),
                Variable 'z'
            ),
            Lambda('x', Variable 'y')
        )
        |> Ok
        )

[<Fact>]
let ``Test parser 9`` () =
    equal(
        parse @"\x.x(y)",
        parse @"(\x.x)(y)"
        )

[<Fact>]
let ``Test parser 10`` () =
    equal(
        parse @"\x.xx(y)",
        parse @"(\x.xx)(y)"
        )

[<Fact>]
let ``Test parser 11`` () =
    equal(
        parse @"\xy.xy(y)",
        parse @"(\x.\y.xy)(y)"
        )
   
[<Fact>]
let ``Test parser 12`` () =
    equal(
        parse @"\x.x\y.y",
        parse @"(\x.x)(\y.y)"
        )