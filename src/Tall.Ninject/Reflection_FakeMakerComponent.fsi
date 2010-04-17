// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details

namespace Tall.Ninject.Reflection

  /// Interface that describes a Ninject component for FakeMaker
  type IFakeMakerComponent =
    interface
      inherit Ninject.Components.INinjectComponent
      /// <summary>
      ///   Gets the <see cref="Tall.Ninject.FakeMaker">FakeMaker</see> instance 
      ///   for this component
      ///  </summary>
      ///  <returns>
      ///   the <see cref="Tall.Ninject.FakeMaker">FakeMaker</see> instance 
      ///   for this component
      ///  </returns>
      abstract member FakeMaker : IFakeMaker
    end

  /// Implementation of the <see cref="Tall.Ninject.IFakeMaker">IFakeMaker</see>
  /// interface that uses sensible defaults.
  type FakeMakerComponent =
    class
      inherit Ninject.Components.NinjectComponent
      interface IFakeMakerComponent
      /// Initializes a new instance of the FakeMakerComponent
      new : unit -> FakeMakerComponent
    end
