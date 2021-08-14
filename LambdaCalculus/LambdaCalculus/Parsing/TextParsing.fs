module LambdaCalculus.TextParsing

open LambdaCalculus.Atoms

let (|ValidVariable|_|) (value : char) =
    if VariableAlphabet.Contains value then
        Some value
    else
        None

type SyntaxBlock =
    | OneItem of char
    | Blocks of SyntaxBlock list


let untilCan s =
    let rec untilCanInner s res count =
        match s with
        | [] -> 
            if count = 0 then
                Ok (res, [])
            else
                Error $"Brackets count should be 0 but is {count}"
        | '('::tl ->
            untilCanInner tl ('('::res) (count + 1)
        | ')'::tl ->
            if count - 1 >= 0 then
                untilCanInner tl (')'::res) (count - 1)
            else
                Ok (res, tl)
        | hd::tl ->
            untilCanInner tl (hd::res) count
    untilCanInner s [] 0
    |> Result.map (fun (parsed, rest) -> List.rev parsed, rest)
        

let untilNotVariable s =
    let rec untilNotVariableInner s res =
        match s with
        | ValidVariable x::rest ->
            untilNotVariableInner rest (x::res)
        | other ->
            res, other

    let (res, other) = untilNotVariableInner s []
    List.rev res, other

