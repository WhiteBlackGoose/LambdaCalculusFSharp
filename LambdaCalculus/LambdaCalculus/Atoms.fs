module LambdaCalculus.Atoms

type Variable = char

type Expression =
    | Variable of Variable
    | Constant of obj
    | Lambda of Head : Variable * Body : Expression
    | Applied of Lambda : Expression * Argument : Expression

let rec substitute x value = function
    | Variable x' when x' = x -> value
    | Constant c -> Constant c
    | Lambda (y, body) when y <> x -> substitute x value body
    | Applied (lambda, argument) -> Applied(substitute x value lambda, substitute x value argument)
    | other -> other


let rec betaReduce = function
    | Applied(Lambda(x, body), arg) -> substitute x arg body
    | other -> other