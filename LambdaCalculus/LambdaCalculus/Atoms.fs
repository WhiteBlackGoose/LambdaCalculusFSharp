module LambdaCalculus.Atoms

type Variable = char


type Expression =
    | Variable of Variable
    | Lambda of Head : Variable * Body : Expression
    | Applied of Lambda : Expression * Argument : Expression


let rec substitute x value expr =
    match expr with
    | Variable x' when x' = x -> value
    | Lambda (y, body) when y <> x -> Lambda(y, substitute x value body)
    | Applied (lambda, argument) -> Applied(substitute x value lambda, substitute x value argument)
    | other -> other


let rec betaReduce expr : Expression =
    match expr with
    | Applied(Lambda(x, body), arg) ->
        substitute x arg body
        // |> betaReduce
    | Applied(Variable x, arg) ->
        Applied(Variable x, betaReduce arg)
    | Applied(maybeLambda, arg) ->
        (betaReduce maybeLambda, arg)
        |> Applied
        |> betaReduce
    | Lambda(x, body) -> Lambda(x, betaReduce body)
    | Variable x -> Variable x

let βReduce expr = betaReduce expr

let rec alphaEqual expr other =
    match expr, other with
    | Variable x, Variable y -> x = y
    | Applied(lambda1, arg1), Applied(lambda2, arg2) -> 
        alphaEqual lambda1 lambda2 
        && alphaEqual arg1 arg2
    | Lambda(x, body1), Lambda(y, body2) ->
        alphaEqual (substitute x (Variable y) body1) body2
        && alphaEqual (substitute y (Variable x) body2) body1
    | _ -> false

let αEqual expr other = alphaEqual expr other