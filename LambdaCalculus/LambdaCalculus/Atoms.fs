﻿module LambdaCalculus.Atoms

open LambdaCalculus.Utils
type Variable = char

let VariableAlphabet = "xyzabcdefghijklmnopqrstuvw"

type Expression =
    | Variable of Variable
    | Lambda of Head : Variable * Body : Expression
    | Applied of Lambda : Expression * Argument : Expression


let rec freeVariables = function
    | Variable x -> [ x ]
    | Lambda (y, body) ->
        freeVariables body
        |> listRemove y
    | Applied (left, right) ->
        List.append (freeVariables left) (freeVariables right)
    
let newVariable oldOnes =
    VariableAlphabet
    |> Seq.find (fun c -> List.contains c oldOnes |> not)

let rec substitute x value expr =
    match expr with
    | Variable x' when x' = x -> value
    | Lambda (x', body) when x' = x -> Lambda (x', body)
    | Lambda (y, body) when y <> x ->
        let valueFreeVars = freeVariables value
        if List.contains y valueFreeVars then
            let z = newVariable (List.append valueFreeVars (freeVariables body))
            let newBody = substitute y (Variable z) body
            Lambda(z, substitute x value newBody)
        else
            Lambda(y, substitute x value body)
    | Applied (lambda, argument) -> Applied(substitute x value lambda, substitute x value argument)
    | other -> other

let rec complexity = function
    | Variable _ -> 1
    | Lambda (_, right) -> complexity right
    | Applied (left, right) -> complexity left + complexity right

let rec betaReduce expr : Expression =
    let rec betaReduceInner expr : Expression =
        match expr with
        | Applied(Lambda(x, body), arg) ->
            substitute x arg body
        | Applied(Variable x, arg) ->
            Applied(Variable x, betaReduceInner arg)
        | Applied(maybeLambda, arg) ->
            let nowLambdaOrNot = betaReduceInner maybeLambda
            match nowLambdaOrNot with
            | Lambda _ ->
                (nowLambdaOrNot, betaReduceInner arg)
                |> Applied
                |> betaReduceInner
            | _ -> Applied (nowLambdaOrNot, betaReduceInner arg)
        | Lambda(x, body) -> Lambda(x, betaReduceInner body)
        | Variable x -> Variable x

    let reduced = betaReduceInner expr
    if complexity reduced < complexity expr then
        betaReduce reduced
    else
        expr

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
