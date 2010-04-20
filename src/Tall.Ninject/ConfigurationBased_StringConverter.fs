namespace Tall.Ninject.ConfigurationBased

open System
open System.Globalization
open System.ComponentModel
open Tall.Utility
open Ninject.Components

type IStringConverterComponent =
    inherit INinjectComponent
    abstract Supports : service:Type -> bool
    abstract Convert : service:Type * value:string -> obj option

type StringToStringConverterComponent() =
    inherit NinjectComponent()

    let supports service = service = typeof<string>

    interface IStringConverterComponent with
        member this.Supports(service) = supports service
        member this.Convert(service, value) = if supports service then Some(value :> obj) else None

type StringToIntConverterComponent() =
    inherit NinjectComponent()

    let supports service = service = typeof<int>
    let tryP value style = Int32.TryParse(value,style, CultureInfo.InvariantCulture) |> Option.fromTryParse

    interface IStringConverterComponent with
        member this.Supports(service) = supports service
        member this.Convert(service, value) = 
            if not <| supports service 
                then None 
                else
                    tryP value (NumberStyles.Integer ||| NumberStyles.AllowThousands)
                    |> Option.elseF (fun _ -> tryP value NumberStyles.HexNumber) 
                    |> Option.ofObj
