// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details

namespace Tall.Ninject.ConfigurationBased

open System
open System.Collections.Generic
open Tall.Utility
open Ninject.Activation
open Ninject.Activation.Providers
open Ninject.Components
open Ninject.Planning.Bindings
open Ninject.Planning.Bindings.Resolvers

type ConfigurationBasedBindingResolver(converters: IEnumerable<IStringConverterComponent>, config: IConfigurationSettingsComponent) =
    inherit NinjectComponent()

    let valueMap = new System.Collections.Concurrent.ConcurrentDictionary<string * Type, IBinding seq>()

    let swap f a b = f b a

    let resolve(name, service) =
        let makeBinding name service (value: 'a) =
            let satisfiesRequest name (request: IRequest) =
                    request.Target
                    |> Option.valueToOption
                    |> Option.bind (fun t -> t.Name |> Option.valueToOption)
                    |> Option.exists ((=) name)

            let binding = new Binding(service, IsImplicit = true, Target = BindingTarget.Constant)
            binding.Condition <- (fun r -> satisfiesRequest name r)
            binding.ProviderCallback <- (fun _ -> new ConstantProvider<'a>(value) :> IProvider)
            binding

        match config.GetValue(name) with
            | None -> Seq.empty
            | Some(value) -> converters
                             |> Seq.filter (fun c -> c.Supports(service))
                             |> Seq.map (fun c -> c.Convert(value, service))
                             |> Seq.filter Option.isSome
                             |> Seq.map (Option.get >> (makeBinding name service))
                             |> Seq.cast

    interface IMissingBindingResolver with
        member this.Resolve(bindings, request) =
            request.Target
            |> Option.valueToOption
            |> Option.bind (fun t -> t.Name |> Option.valueToOption)
            |> Option.map (fun name -> valueMap.GetOrAdd((name, request.Service), resolve))
            |> ((swap defaultArg) Seq.empty)

    static member RegisterDefaultsIn(components: IComponentContainer) =
        components.Add<IMissingBindingResolver, ConfigurationBasedBindingResolver>();
        components.Add<IConfigurationSettingsComponent, AppSettingConfigurationSettingsComponent>();
        components.Add<IStringConverterComponent, StringToStringConverterComponent>();
        components.Add<IStringConverterComponent, StringToCharConverterComponent>();
        components.Add<IStringConverterComponent, StringToEnumConverterComponent>();
        components.Add<IStringConverterComponent, StringToBoolConverterComponent>();
        components.Add<IStringConverterComponent, StringToInt16ConverterComponent>();
        components.Add<IStringConverterComponent, StringToInt32ConverterComponent>();
        components.Add<IStringConverterComponent, StringToInt64ConverterComponent>();
        components.Add<IStringConverterComponent, StringToUInt16ConverterComponent>();
        components.Add<IStringConverterComponent, StringToUInt32ConverterComponent>();
        components.Add<IStringConverterComponent, StringToUInt64ConverterComponent>();
        components.Add<IStringConverterComponent, StringToSByteConverterComponent>();
        components.Add<IStringConverterComponent, StringToByteConverterComponent>();
        components.Add<IStringConverterComponent, StringToDoubleConverterComponent>();
        components.Add<IStringConverterComponent, StringToSingleConverterComponent>();
        components.Add<IStringConverterComponent, StringToDecimalConverterComponent>();
