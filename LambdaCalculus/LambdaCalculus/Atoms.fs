module LambdaCalculus.Atoms

type Variable = char

type Expression =
    | Variable of Variable
    | Lambda of Head : Variable * Body : Expression
    | Applied of Lambda : Expression * Argument : Expression

let rec substitute x value = function
    | Variable x' when x' = x -> value
    | Lambda (y, body) when y <> x -> Lambda(y, substitute x value body)
    | Applied (lambda, argument) -> Applied(substitute x value lambda, substitute x value argument)
    | other -> other


let rec betaReduce = function
    | Applied(maybeLambda, arg) -> 
        match betaReduce maybeLambda with
        | Lambda(x, body) -> substitute x arg body
        | other -> Applied(other, arg)
    | Lambda(x, body) -> Lambda(x, betaReduce body)
    | Variable x -> Variable x
