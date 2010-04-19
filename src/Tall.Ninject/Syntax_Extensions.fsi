namespace Tall.Ninject.Syntax

  /// Extension to the Ninject binding syntax
  [<System.Runtime.CompilerServices.Extension ()>]
  type SyntaxExtensions = 
    class
      /// <summary>
      ///   Creates a condition such that the binding is only resolved
      ///   when the target has the specified name.
      /// </summary>
      /// <param name="syntax">The syntax object.</param>
      /// <param name="name">The name of the target to bind to.</param>
      /// <returns>Syntax object for chaining.</returns>
      [<System.Runtime.CompilerServices.Extension ()>]
      static member WhenTargetNamed 
         : syntax:Ninject.Syntax.IBindingWhenSyntax<'T> * name:string 
        -> Ninject.Syntax.IBindingInNamedWithOrOnSyntax<'T>
    end


