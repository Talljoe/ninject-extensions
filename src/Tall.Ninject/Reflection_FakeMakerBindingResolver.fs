// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details
namespace Tall.Ninject.Reflection

open System
open Ninject
open Ninject.Components
open Ninject.Planning.Bindings
open Ninject.Planning.Bindings.Resolvers

type FakeMakerBindingResolver(fakeMakerComponent: IFakeMakerComponent) =
    inherit NinjectComponent()
    let typeMap = new System.Collections.Concurrent.ConcurrentDictionary<Type, Lazy<IBinding>>()
    let fakeMaker = fakeMakerComponent.FakeMaker

    interface IBindingResolver with
        member this.Resolve(bindings, service) = 
            let bindType implementation = 
                let provider = Ninject.Activation.Providers.StandardProvider.GetCreationCallback(implementation)
                let binding = new Binding(service, ProviderCallback = provider, Target = BindingTarget.Type)
                binding :> IBinding

            let notBound = Seq.isEmpty bindings.[service]
            seq {
                if notBound && fakeMaker.CanFake(service) then
                    let makeFunc() = fakeMaker.ImplementType(service) |> bindType
                    // By making it lazy we only ever execute makeFunc once preventing
                    // the same type from being created a second time (causing exceptions)
                    yield typeMap.GetOrAdd(service, Lazy.Create(makeFunc)).Force()
            }

    static member RegisterDefaultsIn(components: IComponentContainer) =
        components.Add<IFakeMakerComponent, FakeMakerComponent>()
        components.Add<IBindingResolver, FakeMakerBindingResolver>()

