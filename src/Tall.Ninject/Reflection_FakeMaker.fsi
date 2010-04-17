// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details

namespace Tall.Ninject.Reflection

  /// Interface describing the Fake Maker contract. 
  type IFakeMaker =
    interface
      /// <summary>Returns <c>true</c> if the type can be faked.</summary>
      /// <param name="service">The type to be faked.</param>
      /// <returns><c>true</c> if the type can be faked; otherwise <c>false</c></returns>
      abstract member CanFake : service:System.Type -> bool
      /// <summary>Implements a fake version of the given type.</summary>
      /// <param name="service">The type to be faked.</param>
      /// <returns>A new type that derives from <c>service</c>.</returns>
      abstract member ImplementType : service:System.Type -> System.Type
    end

  /// Class that builds a type from an interface.
  type FakeMaker =
    class
      interface IFakeMaker
      /// Initializes a new instance of FakeMaker using the defaults.
      new : unit -> FakeMaker
      /// Initializes a new instance of FakeMake using the supplied
      /// <see cref="System.Reflection.Emit.ModuleBuilder">ModuleBuilder</see>.
      new : moduleBuilder:System.Reflection.Emit.ModuleBuilder -> FakeMaker
      /// <summary>Returns <c>true</c> if the type can be faked.</summary>
      /// <param name="service">The type to be faked.</param>
      /// <returns><c>true</c> if the type can be faked; otherwise <c>false</c></returns>
      member CanFake : service:System.Type -> bool
      /// <summary>Implements a fake version of the given type.</summary>
      /// <param name="service">The type to be faked.</param>
      /// <returns>A new type that derives from <c>service</c>.</returns>
      member ImplementType : service:System.Type -> System.Type

      /// <summary>Creates a new ModuleBuilder</summary>
      static member CreateModuleBuilder : unit -> System.Reflection.Emit.ModuleBuilder
    end
