// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details

namespace Tall.Ninject.Reflection

  /// Implementation of the <see cref="Ninject.Planning.Bindings.Resolvers.IBindingResolver">IBindingResolver</see>
  /// interface for building fake objects from interfaces.
  type FakeMakerBindingResolver =
    class
      inherit Ninject.Components.NinjectComponent
      interface Ninject.Planning.Bindings.Resolvers.IBindingResolver
      /// <summary>
      ///   Initializes a new instance of FakeMakerBindingResolver
      /// </summary>
      /// <param name="fakeMakerComponent">
      ///   The instance of <see cref="Tall.Ninject.IFakeMakerComponent">IFakeMakerComponent</see>
      ///   used to generate types.
      /// </param>
      new : fakeMakerComponent:IFakeMakerComponent -> FakeMakerBindingResolver
      
      /// <summary>
      ///   Registers a default implementation of FakeMakerBindingResolver with Ninject.
      /// </summary>
      /// <param name="components">
      ///   The <see cref="Ninject.Components.IComponentContainer">IComponentContainer</see>
      ///   into which the component should be registered.
      /// </param>
      static member
        RegisterDefaultsIn : components:Ninject.Components.IComponentContainer -> unit
    end

