module LambdaCalculus.Output

open LambdaCalculus.Atoms

let rec sprintLambda = function
    | Variable x -> x.ToString()
    | Lambda (x, body) -> $@"\{x}.{sprintLambda body}"
    | Applied (left, right) -> $"({sprintLambda left})({sprintLambda right})"
