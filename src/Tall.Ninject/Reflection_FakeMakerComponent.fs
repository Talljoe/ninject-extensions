// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details
namespace Tall.Ninject.Reflection

open Ninject.Components

type IFakeMakerComponent =
    inherit INinjectComponent
    abstract FakeMaker : IFakeMaker

type FakeMakerComponent() =
    inherit NinjectComponent()
    let fakeMaker = new FakeMaker() :> IFakeMaker
    interface IFakeMakerComponent with member this.FakeMaker = fakeMaker

