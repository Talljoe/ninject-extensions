// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details
namespace Tall.Ninject.ConfigurationBased

open System
open System.Collections.Generic
open Tall.Utility
open Ninject.Activation
open Ninject.Components
open Ninject.Planning.Bindings
open Ninject.Planning.Bindings.Resolvers

type ConfigurationBasedProvider(service: Type, converter: IStringConverterComponent, config: IConfigurationSettingsComponent) =
    let resolve name = config.GetValue(name) |> Option.bind (fun value -> converter.Convert(service, value))

    let satisfiesRequest (request: IRequest) =
            match request.Target with | null -> false | t -> resolve t.Name |> Option.isSome

    interface IProvider with
        member this.Type = service
        member this.Create(context) = 
            if context.Request.Target = null then failwith "Can't create without a target."
            match resolve context.Request.Target.Name with
                | None -> failwith "Can't create...did you check IBinding.Condition first?"
                | Some(v) -> v

    member this.GetBinding() =
        let binding = new Binding(service)
        binding.Condition <- (fun r -> satisfiesRequest r)
        binding.ProviderCallback <- (fun _ -> this :> IProvider)
        binding.Target <- BindingTarget.Provider
        binding

type ConfigurationBasedBindingResolver(converters: IEnumerable<IStringConverterComponent>, config: IConfigurationSettingsComponent) =
    inherit NinjectComponent()
    let getBinding service converter = 
        let provider = new ConfigurationBasedProvider(service, converter, config)
        provider.GetBinding()

    interface IBindingResolver with
        member this.Resolve(bindings, service) =
            converters
            |> Seq.filter (fun c -> c.Supports(service))
            |> Seq.map (getBinding service)
            |> Seq.cast

    static member RegisterDefaultsIn(components: IComponentContainer) =
        components.Add<IBindingResolver, ConfigurationBasedBindingResolver>();
        components.Add<IConfigurationSettingsComponent, AppSettingConfigurationSettingsComponent>();
        components.Add<IStringConverterComponent, StringToStringConverterComponent>();
        components.Add<IStringConverterComponent, StringToIntConverterComponent>();
