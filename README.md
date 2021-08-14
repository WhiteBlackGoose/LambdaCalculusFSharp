# λ calculus in F#

Fun library for lambda calculus made purely in F#. It has a [web app](https://whiteblackgoose.github.io/LambdaCalculusFSharp/) made for demonstration.

## Syntax of λ-calculus

Rules:
- Variables are single-character lower-case Latin letters
- Lambda starts with `\`, then is followed by 1 or more variable, then a dot (`.`), then expression.
Until wrapped with parentheses, the body of a lambda has a lower priority than `()` (e. g. `\x.x(y)` is the same as `(\x.x)(y)`).
- Associativity is left-sided: `xyz` is `(xy)z`.
- Symbol `λ` can be used instead of `\`, but is not essential.

Examples:
- `x` - an expression of one free variable x
- `xy` - `y` applied to `x`
- `\x.x` - an identity
- `\x.xx` - a lambda which returns its only parameter applied to itself
- `(\x.x)y` - `y` applied to identity (will return `y` after beta reduction)
- `(\x.xx)(\x.xx)` - simple example of beta-irreducible expression

## Library API

Expression is defined as following:
```fs
type Expression =
    | Variable of char
    | Lambda of Head : Variable * Body : Expression
    | Applied of Lambda : Expression * Argument : Expression
```
in `LambdaCalculus.Atoms`.

In the same module `LambdaCalculus.Atoms` there are 
- `betaReduce` or `βReduce`: `Expression -> Expression` performs beta-reduction, that is, simplification of an expression
(starting from bottom, replaces parameters of lambdas in their bodies with the applied expressions)
- `alphaEqual` or `αEqual`: `Expression -> Expression -> Expression` compares two expressions up to lambdas parameter names
- ``

In the module `LambdaCalculus.Parsing` there is `parse` which takes a string as an argument.

## Fun facts

