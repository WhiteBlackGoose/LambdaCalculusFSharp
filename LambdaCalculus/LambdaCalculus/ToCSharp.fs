module LambdaCalculus.ToCSharp

open LambdaCalculus.Atoms

let rec toCSharp = function
    | Variable x -> x.ToString()
    | Lambda (x, body) -> $@"((Func<dynamic, dynamic>)((dynamic {x}) => {toCSharp body}))"
    | Applied (left, right) -> $"({toCSharp left})({toCSharp right})"