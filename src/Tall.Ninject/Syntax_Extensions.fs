// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details
namespace Tall.Ninject.Syntax

open System
open Ninject.Activation
open Ninject.Syntax

[<System.Runtime.CompilerServices.ExtensionAttribute>]
type SyntaxExtensions =
    [<System.Runtime.CompilerServices.ExtensionAttribute>]
    static member WhenTargetNamed(syntax: IBindingWhenSyntax<'T>, name: string) =
        let nameI = String.Intern(name)
        syntax.Binding.Condition <- 
            (function null -> false 
                    | r -> match r.Target with
                           | null -> false 
                           | t -> String.Equals(t.Name, nameI, StringComparison.OrdinalIgnoreCase))
        syntax :?> IBindingInNamedWithOrOnSyntax<'T>
