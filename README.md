# λ calculus in F#

Fun library for lambda calculus made purely in F#.

Construct expressions from three Atoms: `Variable`, `Lambda`, `Applied`. You can also
parse it from string, following the standard notation, for example:
- `x` - an expression of one free variable x
- `xy` - `y` applied to `x`
- `\x.x` - an identity
- `\x.xx` - a lambda which returns its only parameter applied to itself
- `(\x.x)y` - `y` applied to identity (will return `y` after beta reduction)
- `(\x.xx)(\x.xx)` - simple example of beta-irreducible expression

Functions:
- `substitute` substitutes a variable with an expression in an expression 
(ignoring lambdas whose parameter coincides with ours)
- `betaReduce` performs beta-reduction, that is, simplification of an expression
(starting from bottom, replaces parameters of lambdas in their bodies with the applied expressions)
- `parse` parses an expression from the standard notation


You can play with it on the [website](https://whiteblackgoose.github.io/LambdaCalculusFSharp/)
