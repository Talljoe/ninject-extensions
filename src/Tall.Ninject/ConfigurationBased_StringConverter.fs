// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details

namespace Tall.Ninject.ConfigurationBased

open System
open System.Globalization
open System.ComponentModel
open System.Linq
open Tall.Utility
open Ninject.Components

type IStringConverterComponent =
    inherit INinjectComponent
    abstract Supports : service:Type -> bool
    abstract Convert : value:string * service:Type -> obj option

type StringToStringConverterComponent() =
    inherit NinjectComponent()

    let supports service = service = typeof<string>

    interface IStringConverterComponent with
        member this.Supports(service) = supports service
        member this.Convert(value, service) = if supports service then Some(value :> obj) else None

type StringToCharConverterComponent() =
    inherit NinjectComponent()

    let supports service = service = typeof<char>

    interface IStringConverterComponent with
        member this.Supports(service) = supports service
        member this.Convert(value, service) = 
            if supports service && value.Length = 1 then Some(value.[0] :> obj) else None

type StringToEnumConverterComponent() =
    inherit NinjectComponent()

    let supports (service:Type) = service.IsEnum

    interface IStringConverterComponent with
        member this.Supports(service) = supports service
        member this.Convert(value, service) =
            if value = null || not <| supports service then None
            else
                // If only Enum.TryParse had a non-generic overload
                let enumMap = Enum.GetValues(service)
                                  .Cast<obj>()
                                  .ToDictionary(
                                        (fun v -> Enum.GetName(service, v)), 
                                        (fun v -> v), 
                                        StringComparer.OrdinalIgnoreCase)

                value.Split([|','|], StringSplitOptions.RemoveEmptyEntries)
                |> Seq.map (enumMap.TryGetValue 
                            >> Option.fromTryParse
                            >> Option.map (fun o -> Convert.ToInt64(o, CultureInfo.InvariantCulture)))
                |> Seq.fold (Option.combine (+)) (Some(0L))
                |> Option.map (fun v -> Enum.ToObject(service, v))
                |> Option.ofObj

[<AbstractClass>]
type StringToNumberConverterComponent<'a>() =
    inherit NinjectComponent()

    let supports service = service = typeof<'a>

    abstract member TryParse : value: string * style: NumberStyles -> 'a option
    abstract member NumberStyles : NumberStyles seq
    default this.NumberStyles = Seq.singleton NumberStyles.Any

    interface IStringConverterComponent with
        member this.Supports(service) = supports service
        member this.Convert(value, service) = 
            if value = null || not <| supports service
                then None 
                else
                    let tryParse style = this.TryParse(value, style)
                    // This craziness tries each NumberStyle until we get a result
                    this.NumberStyles 
                    |> Seq.map tryParse // parse
                    |> Seq.filter Option.isSome // throw out "None"
                    |> Seq.headOrNone // get the first, if we can
                    |> Option.map Option.get // 'a option option -> 'a option
                    |> Option.ofObj // 'a option -> obj option

[<AbstractClass>]
type StringToSignedIntegerConverterComponent<'a>() =
    inherit StringToNumberConverterComponent<'a>()
    override this.NumberStyles = 
        seq [
            NumberStyles.Integer ||| NumberStyles.AllowThousands;
            NumberStyles.HexNumber;
        ]

[<AbstractClass>]
type StringToUnsignedIntegerConverterComponent<'a>() =
    inherit StringToNumberConverterComponent<'a>()
    override this.NumberStyles = 
        seq [
            NumberStyles.AllowLeadingWhite ||| NumberStyles.AllowTrailingWhite ||| NumberStyles.AllowThousands;
            NumberStyles.HexNumber;
        ]

type StringToSByteConverterComponent() =
    inherit StringToSignedIntegerConverterComponent<sbyte>()
    override this.TryParse(value, style) = SByte.TryParse(value, style, CultureInfo.InvariantCulture) |> Option.fromTryParse

type StringToInt16ConverterComponent() =
    inherit StringToSignedIntegerConverterComponent<int16>()
    override this.TryParse(value, style) = Int16.TryParse(value, style, CultureInfo.InvariantCulture) |> Option.fromTryParse

type StringToInt32ConverterComponent() =
    inherit StringToSignedIntegerConverterComponent<int>()
    override this.TryParse(value, style) = Int32.TryParse(value, style, CultureInfo.InvariantCulture) |> Option.fromTryParse

type StringToInt64ConverterComponent() =
    inherit StringToSignedIntegerConverterComponent<int64>()
    override this.TryParse(value, style) = Int64.TryParse(value, style, CultureInfo.InvariantCulture) |> Option.fromTryParse

type StringToByteConverterComponent() =
    inherit StringToUnsignedIntegerConverterComponent<byte>()
    override this.TryParse(value, style) = Byte.TryParse(value, style, CultureInfo.InvariantCulture) |> Option.fromTryParse

type StringToUInt16ConverterComponent() =
    inherit StringToUnsignedIntegerConverterComponent<uint16>()
    override this.TryParse(value, style) = UInt16.TryParse(value, style, CultureInfo.InvariantCulture) |> Option.fromTryParse

type StringToUInt32ConverterComponent() =
    inherit StringToUnsignedIntegerConverterComponent<uint32>()
    override this.TryParse(value, style) = UInt32.TryParse(value, style, CultureInfo.InvariantCulture) |> Option.fromTryParse

type StringToUInt64ConverterComponent() =
    inherit StringToUnsignedIntegerConverterComponent<uint64>()
    override this.TryParse(value, style) = UInt64.TryParse(value, style, CultureInfo.InvariantCulture) |> Option.fromTryParse

type StringToDecimalConverterComponent() =
    inherit StringToNumberConverterComponent<decimal>()
    override this.TryParse(value, style) = Decimal.TryParse(value, style, CultureInfo.InvariantCulture) |> Option.fromTryParse

type StringToDoubleConverterComponent() =
    inherit StringToNumberConverterComponent<double>()
    override this.TryParse(value, style) = Double.TryParse(value, style, CultureInfo.InvariantCulture) |> Option.fromTryParse

type StringToSingleConverterComponent() =
    inherit StringToNumberConverterComponent<single>()
    override this.TryParse(value, style) = Single.TryParse(value, style, CultureInfo.InvariantCulture) |> Option.fromTryParse
