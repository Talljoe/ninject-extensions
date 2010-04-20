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
    let valueMap = new System.Collections.Concurrent.ConcurrentDictionary<string, obj option>()
    let resolve name =
        // GetOrAdd won't take a lock so the function may be called twice; that's fine in this case.
        valueMap.GetOrAdd(name, config.GetValue >> Option.bind (fun v -> converter.Convert(service, v)))

    let satisfiesRequest (request: IRequest) =
            // Yes, we have to do a full resolve in order to know if the binding matches.
            request.Target 
            |> Option.valueToOption
            |> Option.map (fun t -> resolve t.Name)
            |> Option.isSome

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
