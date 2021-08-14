module LambdaCalculus.Parsing

open LambdaCalculus.Atoms
open LambdaCalculus.TextParsing
open LambdaCalculus.Utils


let rec parseInner s : Result<Expression, string> =
    match s with
    | [] -> Error "Empty input"
    | [ ValidVariable x ] -> Ok (Variable x)
    | other ->
        let rec blockApplier applied blocks =
            match blocks with
            | [] -> applied |> Ok
            | hd::rest -> blockApplier (Applied(applied, hd)) rest

        opt {
            let! blocks = blocksParse other
            let res = 
                match blocks with
                | hd::tl -> blockApplier hd tl
                | _ -> Error "Empty block"
            return! res
        }

and blocksParse s : Result<Expression list, string> =
    match s with
    | ValidVariable x::rest ->
        blocksParse rest
        |> Result.map (fun parsed -> (Variable x) :: parsed)

    | '\\'::rest ->
        opt {
            let! (lambda, other) = parseLambda rest
            let! parsed = blocksParse other
            return lambda :: parsed
        }

    | '('::rest ->
        opt {
            let! (left, other) = untilCan rest
            let! parsed = parseInner left
            let! blocks = blocksParse other
            return parsed :: blocks
        }

    | [] -> Ok []

    | err -> Error $"Unexpected block start {err}"

and parseLambda s : Result<Expression * char list, string> =
    match s with
    | ValidVariable x::rest ->
        opt {
            let! (lambda, other) = parseLambda rest
            return Lambda(x, lambda), other
        }
        
    | '.'::'('::rest ->
        opt {
            let! (expr, other) = untilCan rest
            let! parsed = parseInner expr
            return parsed, other
        }

    | '.'::'\\'::rest ->
        opt {
            let! (innerLambda, other) = parseLambda rest
            return innerLambda, other
        }

    | '.'::ValidVariable x::rest -> parseFlat (x::rest)
        
    | err -> Error $"Unexpected syntax for lambda: {err}"

and parseFlat s : Result<Expression * char list, string> =
    match s with
    | [ ValidVariable x ] ->
        (Variable x, [])
        |> Ok
    | ValidVariable x::'('::rest | ValidVariable x::'\\'::rest ->
        (Variable x, rest)
        |> Ok
    | ValidVariable x::other ->
        let (variableLine, other) = untilNotVariable other
        let rec variableLineApply (s : char list) res =
            match s with
            | x::rest ->
                variableLineApply rest (Applied(res, Variable x))
            | [] ->
                res
        (variableLineApply variableLine (Variable x), other)
        |> Ok
    | err -> Error $"Flat parser encountered unexpected: {err}"



let parse (s : string) =
    s
    |> (fun s -> s.Replace("λ", "\\"))
    |> (fun s -> s.Replace(" ", ""))    // spaces do not mean anything
    |> List.ofSeq
    |> parseInner
