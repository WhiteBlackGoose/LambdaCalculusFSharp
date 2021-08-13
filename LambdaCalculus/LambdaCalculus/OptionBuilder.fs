module ResultBuilder

type ResultBuilder () =
    member this.Bind (m, f) =
        match m with
        | Result.Ok res -> f res
        | Result.Error error -> Result.Error error

    member this.Return m = Ok m

    member this.ReturnFrom m = m


let opt = ResultBuilder ()