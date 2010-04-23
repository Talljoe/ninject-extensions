// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details

module Tall.Utility

module Option =
    let fromTryParse (b, v) = if b then Some(v) else None
    let elseF f = function None -> f() | v -> v
    let valueToOption = function null -> None | v -> Some(v)
    let ofObj = function None -> None | Some(v) -> Some(v :> obj)
    let combine f o1 o2 = o1 |> Option.bind (fun v1 -> o2 |> Option.map (f v1))

module Seq =
    let headOrNone (s: 'a seq) = 
        use e = s.GetEnumerator()
        if e.MoveNext() then Some(e.Current) else None
