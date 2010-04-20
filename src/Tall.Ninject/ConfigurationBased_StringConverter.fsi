// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details

namespace Tall.Ninject.ConfigurationBased
  type IStringConverterComponent =
    interface
      inherit Ninject.Components.INinjectComponent
      abstract member Convert : service:System.Type * value:string -> obj option
      abstract member Supports : service:System.Type -> bool
    end
  type StringToStringConverterComponent =
    class
      inherit Ninject.Components.NinjectComponent
      interface IStringConverterComponent
      new : unit -> StringToStringConverterComponent
    end
  type StringToIntConverterComponent =
    class
      inherit Ninject.Components.NinjectComponent
      interface IStringConverterComponent
      new : unit -> StringToIntConverterComponent
    end

