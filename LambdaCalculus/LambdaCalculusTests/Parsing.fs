module Tests.Parsing

open System
open Xunit
open LambdaCalculus.Atoms
open LambdaCalculus.Parsing

[<Fact>]
let ``Test parser 1`` () =
    Assert.Equal(
        parse "x",
        Variable 'x' |> Ok
        )

[<Fact>]
let ``Test parser 2`` () =
    Assert.Equal(
        parse @"\x.x",
        Lambda('x', Variable 'x') |> Ok
        )

[<Fact>]
let ``Test parser 3`` () =
    Assert.Equal(
        parse "xx",
        Applied(Variable 'x', Variable 'x') |> Ok
        )

[<Fact>]
let ``Test parser 4`` () =
    Assert.Equal(
        parse @"(x)(\x.x)",
        Applied(Variable 'x', Lambda('x', Variable 'x')) |> Ok
        )

[<Fact>]
let ``Test parser 5`` () =
    Assert.Equal(
        parse "yxzu",
        Applied(Applied(Applied(Variable 'y', Variable 'x'), Variable 'z'), Variable 'u') |> Ok
        )

[<Fact>]
let ``Test parser 6`` () =
    Assert.Equal(
        parse "yxzu",
        Applied(Applied(Applied(Variable 'y', Variable 'x'), Variable 'z'), Variable 'u') |> Ok
        )

[<Fact>]
let ``Test parser 7`` () =
    Assert.Equal(
        parse @"(\x.x)xy",
        Applied(Applied(Lambda('x', Variable 'x'), Variable 'x'), Variable 'y') |> Ok
        )

[<Fact>]
let ``Test parser 8`` () =
    Assert.Equal(
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