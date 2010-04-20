// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details

namespace Tall.Ninject.ConfigurationBased

  /// A component that can convert a string into other types.
  type IStringConverterComponent =
    interface
      inherit Ninject.Components.INinjectComponent
      /// <summary>Determines whether the converter supports a given type.</summary>
      /// <param name="service">The type to check.</param>
      /// <returns><c>True</c> if the converter supports the type; otherwise <c>false</c>.</returns>
      abstract member Supports : service:System.Type -> bool
      /// <summary>Attempts to convert the given string to the given type.</summary>
      /// <param name="value">The value to convert from.</param>
      /// <param name="service">The type to convert to.</param>
      /// <returns><c>Some</c> indicating the new object if successful; otherwise <c>None</c>.</returns>
      abstract member Convert : value:string * service:System.Type -> obj option
    end

  /// Class that represents a null conversion.  The string is returned unchanged.
  type StringToStringConverterComponent =
    class
      inherit Ninject.Components.NinjectComponent
      interface IStringConverterComponent

      /// Initializes a new instance of StringToStringConverterComponent
      new : unit -> StringToStringConverterComponent
    end

  /// Class that converts a string to an integer.  Suports decimal and hexadecimal.
  type StringToIntConverterComponent =
    class
      inherit Ninject.Components.NinjectComponent
      interface IStringConverterComponent

      /// Initializes a new instance of StringToIntConverterComponent
      new : unit -> StringToIntConverterComponent
    end

