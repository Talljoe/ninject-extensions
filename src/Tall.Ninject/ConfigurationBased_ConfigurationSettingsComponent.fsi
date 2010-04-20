// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details

namespace Tall.Ninject.ConfigurationBased

  /// Interface describing a component for retrieving configuration values.
  type IConfigurationSettingsComponent =
    interface
      inherit Ninject.Components.INinjectComponent

      /// <summary>
      ///   Gets a value from the configuration settings.
      /// </summary>
      /// <param name="name">Name of the setting to retrieve.</param>
      /// <returns><c>Some</c> if there is a value for the setting; otherwise <c>None</c>.</returns>
      abstract member GetValue : name:string -> string option
    end

  /// An implementation of the 
  /// <see cref="Tall.Ninject.ConfiguratioNBased.IConfigurationSettingsComponent">IConfigurationSettingsComponent</see>
  /// that works against the AppSettings store.
  type AppSettingConfigurationSettingsComponent =
    class
      inherit Ninject.Components.NinjectComponent
      interface IConfigurationSettingsComponent

      /// Initializes a new instanc of the AppSettingConfigurationSettingsComponent class
      new : unit -> AppSettingConfigurationSettingsComponent
    end

