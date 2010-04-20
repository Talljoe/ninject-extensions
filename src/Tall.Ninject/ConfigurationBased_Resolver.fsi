// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details

namespace Tall.Ninject.ConfigurationBased

  /// Implementation of IBindingResolver that uses a configuration store to retrieve values
  type ConfigurationBasedBindingResolver =
    class
      inherit Ninject.Components.NinjectComponent
      interface Ninject.Planning.Bindings.Resolvers.IBindingResolver

      /// <summary>Initializes a new instance of the ConfigurationBasedBindingResolver class.</summary>
      /// <param name="converters">List of supported converters to use.</param>
      /// <param name="config">The configuration store from which to retrieve values.</param>
      new : converters:System.Collections.Generic.IEnumerable<IStringConverterComponent>
          * config:IConfigurationSettingsComponent
         -> ConfigurationBasedBindingResolver

      /// <summary>
      ///   Registers a default implementation of ConfigurationBasedBindingResolver with Ninject.
      /// </summary>
      /// <param name="components">
      ///   The <see cref="Ninject.Components.IComponentContainer">IComponentContainer</see>
      ///   into which the component should be registered.
      /// </param>
      static member
        RegisterDefaultsIn : components:Ninject.Components.IComponentContainer -> unit
    end


