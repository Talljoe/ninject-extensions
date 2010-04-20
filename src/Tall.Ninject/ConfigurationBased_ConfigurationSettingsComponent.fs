// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details
namespace Tall.Ninject.ConfigurationBased

open System.Configuration
open Tall.Utility
open Ninject.Components

type IConfigurationSettingsComponent =
    inherit INinjectComponent
    abstract GetValue : name:string -> string option

type AppSettingConfigurationSettingsComponent() =
    inherit NinjectComponent()

    interface IConfigurationSettingsComponent with 
        member this.GetValue(name) = ConfigurationManager.AppSettings.[name] |> Option.valueToOption
    


