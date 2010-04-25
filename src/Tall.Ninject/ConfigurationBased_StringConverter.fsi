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

  // Base clase for simple number conversions
  [<AbstractClassAttribute ()>]
  type StringToNumberConverterComponent<'a> =
    class
      inherit Ninject.Components.NinjectComponent
      interface IStringConverterComponent

      /// Initializes a new instance of StringToNumberConverterComponent
      new : unit -> StringToNumberConverterComponent<'a>

      /// <summary>
      ///   Attempts to parse the string into a value using the given
      ///   <see cref="System.Globalization.NumberStyles">NumberStyles</see>.
      /// </summary>
      /// <param name="value">The value to convert.</param>
      /// <param name="style">The style to use.</param>
      /// <returns><c>Some</c> indicating the new object if successful; otherwise <c>None</c>.</returns>
      abstract member TryParse : value:string * style:System.Globalization.NumberStyles 
                              -> 'a option

      /// Gets the collection of NumberStyles to try.
      abstract member NumberStyles : seq<System.Globalization.NumberStyles>
    end

  /// Base class that converts a string to a signed integer
  [<AbstractClass>]
  type StringToSignedIntegerConverterComponent<'a> =
    class 
      inherit StringToNumberConverterComponent<'a>
      new : unit -> StringToSignedIntegerConverterComponent<'a>

      /// Gets the collection of NumberStyles to try.
      override NumberStyles : seq<System.Globalization.NumberStyles>
    end

  /// Base class that converts a string to an unsigned integer
  [<AbstractClassAttribute ()>]
  type StringToUnsignedIntegerConverterComponent<'a> =
    class
      inherit StringToNumberConverterComponent<'a>
      new : unit -> StringToUnsignedIntegerConverterComponent<'a>

      /// Gets the collection of NumberStyles to try.
      override NumberStyles : seq<System.Globalization.NumberStyles>
    end

  /// Class that represents a null conversion.  The string is returned unchanged.
  type StringToStringConverterComponent =
    class
      inherit Ninject.Components.NinjectComponent
      interface IStringConverterComponent

      /// Initializes a new instance of StringToStringConverterComponent
      new : unit -> StringToStringConverterComponent
    end

  /// Class that converts a string to a character.
  type StringToCharConverterComponent =
    class
      inherit Ninject.Components.NinjectComponent
      interface IStringConverterComponent

      /// Initializes a new instance of StringToStringConverterComponent
      new : unit -> StringToCharConverterComponent
    end

  /// Class that converts a string to an enumeration.
  type StringToEnumConverterComponent =
    class
      inherit Ninject.Components.NinjectComponent
      interface IStringConverterComponent

      /// Initializes a new instance of StringToStringConverterComponent
      new : unit -> StringToEnumConverterComponent
    end

  /// Class that converts a string to a boolean.
  type StringToBoolConverterComponent =
    class
      inherit Ninject.Components.NinjectComponent
      interface IStringConverterComponent

      /// Initializes a new instance of StringToStringConverterComponent
      new : unit -> StringToBoolConverterComponent
    end

  /// Class that converts a string to signed byte.  Supports decimal and hexadecimal.
  type StringToSByteConverterComponent =
    class
      inherit StringToSignedIntegerConverterComponent<sbyte>

      /// Initializes a new instance of StringToSByteConverterComponent
      new : unit -> StringToSByteConverterComponent

      /// <summary>
      ///   Attempts to parse the string into a value using the given
      ///   <see cref="System.Globalization.NumberStyles">NumberStyles</see>.
      /// </summary>
      /// <param name="value">The value to convert.</param>
      /// <param name="style">The style to use.</param>
      /// <returns><c>Some</c> indicating the new object if successful; otherwise <c>None</c>.</returns>
      override TryParse : value:string * style:System.Globalization.NumberStyles -> sbyte option
    end

  /// Class that converts a string to an int16.  Supports decimal and hexadecimal.
  type StringToInt16ConverterComponent =
    class
      inherit StringToSignedIntegerConverterComponent<int16>

      /// Initializes a new instance of StringToInt16ConverterComponent
      new : unit -> StringToInt16ConverterComponent

      /// <summary>
      ///   Attempts to parse the string into a value using the given
      ///   <see cref="System.Globalization.NumberStyles">NumberStyles</see>.
      /// </summary>
      /// <param name="value">The value to convert.</param>
      /// <param name="style">The style to use.</param>
      /// <returns><c>Some</c> indicating the new object if successful; otherwise <c>None</c>.</returns>
      override TryParse : value:string * style:System.Globalization.NumberStyles -> int16 option
    end

  /// Class that converts a string to an int32.  Supports decimal and hexadecimal.
  type StringToInt32ConverterComponent =
    class
      inherit StringToSignedIntegerConverterComponent<int>

      /// Initializes a new instance of StringToInt32ConverterComponent
      new : unit -> StringToInt32ConverterComponent

      /// <summary>
      ///   Attempts to parse the string into a value using the given
      ///   <see cref="System.Globalization.NumberStyles">NumberStyles</see>.
      /// </summary>
      /// <param name="value">The value to convert.</param>
      /// <param name="style">The style to use.</param>
      /// <returns><c>Some</c> indicating the new object if successful; otherwise <c>None</c>.</returns>
      override TryParse : s:string * style:System.Globalization.NumberStyles -> int option
    end

  /// Class that converts a string to an 64-bit integer.  Supports decimal and hexadecimal.
  type StringToInt64ConverterComponent =
    class
      inherit StringToSignedIntegerConverterComponent<int64>

      /// Initializes a new instance of StringToInt64ConverterComponent
      new : unit -> StringToInt64ConverterComponent

      /// <summary>
      ///   Attempts to parse the string into a value using the given
      ///   <see cref="System.Globalization.NumberStyles">NumberStyles</see>.
      /// </summary>
      /// <param name="value">The value to convert.</param>
      /// <param name="style">The style to use.</param>
      /// <returns><c>Some</c> indicating the new object if successful; otherwise <c>None</c>.</returns>
      override TryParse : s:string * style:System.Globalization.NumberStyles -> int64 option
    end

  type StringToByteConverterComponent =
    class
      inherit StringToUnsignedIntegerConverterComponent<byte>

      /// Initializes a new instance of StringToByteConverterComponent
      new : unit -> StringToByteConverterComponent

      /// <summary>
      ///   Attempts to parse the string into a value using the given
      ///   <see cref="System.Globalization.NumberStyles">NumberStyles</see>.
      /// </summary>
      /// <param name="value">The value to convert.</param>
      /// <param name="style">The style to use.</param>
      /// <returns><c>Some</c> indicating the new object if successful; otherwise <c>None</c>.</returns>
      override TryParse : value:string * style:System.Globalization.NumberStyles -> byte option
    end

  type StringToUInt16ConverterComponent =
    class
      inherit StringToUnsignedIntegerConverterComponent<uint16>

      /// Initializes a new instance of StringToUInt16ConverterComponent
      new : unit -> StringToUInt16ConverterComponent

      /// <summary>
      ///   Attempts to parse the string into a value using the given
      ///   <see cref="System.Globalization.NumberStyles">NumberStyles</see>.
      /// </summary>
      /// <param name="value">The value to convert.</param>
      /// <param name="style">The style to use.</param>
      /// <returns><c>Some</c> indicating the new object if successful; otherwise <c>None</c>.</returns>
      override TryParse : value:string * style:System.Globalization.NumberStyles -> uint16 option
    end

  type StringToUInt32ConverterComponent =
    class
      inherit StringToUnsignedIntegerConverterComponent<uint32>

      /// Initializes a new instance of StringToUInt32ConverterComponent
      new : unit -> StringToUInt32ConverterComponent

      /// <summary>
      ///   Attempts to parse the string into a value using the given
      ///   <see cref="System.Globalization.NumberStyles">NumberStyles</see>.
      /// </summary>
      /// <param name="value">The value to convert.</param>
      /// <param name="style">The style to use.</param>
      /// <returns><c>Some</c> indicating the new object if successful; otherwise <c>None</c>.</returns>
      override TryParse : value:string * style:System.Globalization.NumberStyles -> uint32 option
    end

  type StringToUInt64ConverterComponent =
    class
      inherit StringToUnsignedIntegerConverterComponent<uint64>

      /// Initializes a new instance of StringToUInt64ConverterComponent
      new : unit -> StringToUInt64ConverterComponent

      /// <summary>
      ///   Attempts to parse the string into a value using the given
      ///   <see cref="System.Globalization.NumberStyles">NumberStyles</see>.
      /// </summary>
      /// <param name="value">The value to convert.</param>
      /// <param name="style">The style to use.</param>
      /// <returns><c>Some</c> indicating the new object if successful; otherwise <c>None</c>.</returns>
      override TryParse : value:string * style:System.Globalization.NumberStyles -> uint64 option
    end

  type StringToDecimalConverterComponent =
    class
      inherit StringToNumberConverterComponent<decimal>

      /// Initializes a new instance of StringToDecimalConverterComponent
      new : unit -> StringToDecimalConverterComponent

      /// <summary>
      ///   Attempts to parse the string into a value using the given
      ///   <see cref="System.Globalization.NumberStyles">NumberStyles</see>.
      /// </summary>
      /// <param name="value">The value to convert.</param>
      /// <param name="style">The style to use.</param>
      /// <returns><c>Some</c> indicating the new object if successful; otherwise <c>None</c>.</returns>
      override TryParse : value:string * style:System.Globalization.NumberStyles -> decimal option
    end

  type StringToDoubleConverterComponent =
    class
      inherit StringToNumberConverterComponent<double>

      /// Initializes a new instance of StringToDoubleConverterComponent
      new : unit -> StringToDoubleConverterComponent

      /// <summary>
      ///   Attempts to parse the string into a value using the given
      ///   <see cref="System.Globalization.NumberStyles">NumberStyles</see>.
      /// </summary>
      /// <param name="value">The value to convert.</param>
      /// <param name="style">The style to use.</param>
      /// <returns><c>Some</c> indicating the new object if successful; otherwise <c>None</c>.</returns>
      override TryParse : value:string * style:System.Globalization.NumberStyles -> double option
    end

  type StringToSingleConverterComponent =
    class
      inherit StringToNumberConverterComponent<single>

      /// Initializes a new instance of StringToDoubleConverterComponent
      new : unit -> StringToSingleConverterComponent

      /// <summary>
      ///   Attempts to parse the string into a value using the given
      ///   <see cref="System.Globalization.NumberStyles">NumberStyles</see>.
      /// </summary>
      /// <param name="value">The value to convert.</param>
      /// <param name="style">The style to use.</param>
      /// <returns><c>Some</c> indicating the new object if successful; otherwise <c>None</c>.</returns>
      override TryParse : value:string * style:System.Globalization.NumberStyles -> single option
    end
