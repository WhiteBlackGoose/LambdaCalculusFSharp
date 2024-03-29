﻿module LambdaCalculus.Atoms

open LambdaCalculus.Utils
type Variable = char

let VariableAlphabet = "xyzabcdefghijklmnopqrstuvw"

type Expression =
    | Variable of Variable
    | Lambda of Head : Variable * Body : Expression
    | Applied of Lambda : Expression * Argument : Expression

type ReductionResult =
    | MayTerminate of Expression
    | NeverTerminates

let mapRedRes (f : Expression -> Expression) = function
    | MayTerminate expr -> MayTerminate (f expr)
    | NeverTerminates -> NeverTerminates


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

let αEqual = alphaEqual

let rec alphaSubexpression sub expr =
    if alphaEqual expr sub then
        true
    else
        match expr with
        | Lambda (x, body) -> not (List.contains x (freeVariables sub)) && alphaSubexpression sub body
        | Applied (a, b) -> alphaSubexpression sub a || alphaSubexpression sub b
        | _ -> false

let rec hasRedex = function
    | Applied (Lambda _, _) -> true
    | Applied (a, b) -> hasRedex a || hasRedex b
    | Lambda (_, body) -> hasRedex body
    | Variable _ -> false

let rec betaReduce expr : ReductionResult =
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

    let rec noRepeatReduction encountered expr =
        let reduced = betaReduceInner expr
        if hasRedex reduced |> not then
            MayTerminate reduced
        else if List.exists (alphaEqual reduced) encountered then
            NeverTerminates
        // Proof required!
        //else if List.exists (fun sub -> alphaSubexpression sub reduced) encountered then
        //    NeverTerminates
        else
            noRepeatReduction (reduced::encountered) reduced

    noRepeatReduction [] expr

let βReduce = betaReduce

let rec etaReduce expr : Expression =
    match expr with
    | Lambda (x1, Applied(e, Variable x2)) when x1 = x2 && not (List.contains x1 (freeVariables e)) -> etaReduce e
    | Lambda (x, body) ->
        let reduced = etaReduce body
        if reduced = body then
            Lambda (x, body)
        else
            etaReduce (Lambda(x, reduced))
    | Applied (a, b) -> Applied (etaReduce a, etaReduce b)
    | Variable x -> Variable x

let ηReduce = etaReduce

let betaEtaReduce expr = expr |> betaReduce |> mapRedRes etaReduce

let βηReduce = betaEtaReduce
