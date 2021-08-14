module LambdaCalculus.Atoms

type Variable = char


type [<CustomEquality; CustomComparison>] Expression =
    | Variable of Variable
    | Lambda of Head : Variable * Body : Expression
    | Applied of Lambda : Expression * Argument : Expression
    interface System.IComparable with
         member this.CompareTo o = this.GetHashCode().CompareTo(o.GetHashCode())

    member this.Substitute x value =
        match this with
        | Variable x' when x' = x -> value
        | Lambda (y, body) when y <> x -> Lambda(y, body.Substitute x value)
        | Applied (lambda, argument) -> Applied(lambda.Substitute x value, argument.Substitute x value)
        | other -> other

    override this.Equals o =
        match o with
        | :? Expression as other ->
            match this, other with
            | Variable x, Variable y -> x = y
            | Applied(lambda1, arg1), Applied(lambda2, arg2) -> lambda1 = lambda2 && arg1 = arg2
            | Lambda(x, body1), Lambda(y, body2) ->
                body1.Substitute x (Variable y) = body2
                && body2.Substitute y (Variable x) = body1
            | _ -> false
        | _ -> false

    override this.GetHashCode () =
        let rec countHashCode (nextVar : char) expr =
            match expr with
            | Variable v -> v.GetHashCode()
            | Applied(left, right) -> 
                let leftCode = countHashCode nextVar left
                let rightCode = countHashCode nextVar right
                struct (leftCode, rightCode).GetHashCode()
            | Lambda(x, body) -> 
                let varIndependentHashCode =
                    body.Substitute x (Variable nextVar)
                    |> countHashCode (nextVar + char 1)
                varIndependentHashCode
        countHashCode ('z' + char 1) this

        (*
let betaReduce expr =
    let rec betaReduce encountered expr : Expression option =
        if Set.contains expr encountered then
            None
        else
            let nextBetaReduce = betaReduce (Set.add expr encountered)
            match expr with
            | Applied(maybeLambda, arg) -> 
                match betaReduce Set.empty maybeLambda with
                | None -> None
                | Some(Lambda(x, body)) ->
                    body.Substitute x arg |> nextBetaReduce
                | Some(other) -> Applied(other, arg) |> Some

            | Lambda(x, body) ->
                nextBetaReduce body
                |> Option.map (fun reduced -> Lambda(x, reduced))
                
            | Variable x -> Variable x |> Some

    betaReduce Set.empty expr
*)

let betaReduce expr =
    let rec betaReduce expr : Expression =
        match expr with
        | Applied(Lambda(x, body), arg) ->
            body.Substitute x arg
            // |> betaReduce
        | Applied(Variable x, arg) ->
            Applied(Variable x, betaReduce arg)
        | Applied(maybeLambda, arg) ->
            (betaReduce maybeLambda, arg)
            |> Applied
            |> betaReduce
        | Lambda(x, body) -> Lambda(x, betaReduce body)
        | Variable x -> Variable x

    betaReduce expr

let βreduce expr = betaReduce expr