module Tall.Utility

module Option =
    let fromTryParse (b, v) = if b then Some(v) else None
    let elseF f = function None -> f() | v -> v
    let valueToOption = function null -> None | v -> Some(v)
    let ofObj = function None -> None | Some(v) -> Some(v :> obj)
