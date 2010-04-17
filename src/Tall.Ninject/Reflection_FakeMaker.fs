// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details
namespace Tall.Ninject.Reflection

open System
open System.Reflection
open System.Reflection.Emit
open System.Threading

type IFakeMaker =
    abstract ImplementType: Type -> Type
    abstract CanFake: Type -> bool

type FakeMaker(moduleBuilder: ModuleBuilder) =
    let rec getAllForHierarchy (f: Type -> 'a seq) (t: Type) : 'a seq =
        let next = getAllForHierarchy f
        seq {
            yield! f t
            yield! t.GetInterfaces() |> Seq.collect next
            let baseType = t.BaseType
            if baseType <> null then yield! next baseType
        }

    let getAllMethods = getAllForHierarchy (fun t -> t.GetMethods() |> Seq.ofArray)
    let getAllProperties = getAllForHierarchy (fun t -> t.GetProperties() |> Seq.ofArray)

    let createClass (iface: Type) =
        let dropCase = String.mapi (fun i c -> if i = 0 then Char.ToLowerInvariant(c) else c)

        let builder = moduleBuilder.DefineType("<Generated>" + iface.FullName.TrimStart([|'I'|]), TypeAttributes.Class ||| TypeAttributes.Public ||| TypeAttributes.Sealed)

        let properties = iface |> getAllProperties |> Seq.cache
        let createField (propertyInfo: PropertyInfo) =
            builder.DefineField(dropCase propertyInfo.Name, propertyInfo.PropertyType, FieldAttributes.Private ||| FieldAttributes.InitOnly) :> FieldInfo

        let fieldsMap = properties |> Seq.map (fun p -> (p.Name, createField p)) |> Map.ofSeq

        let createProperty i (propertyInfo: PropertyInfo) =
            if propertyInfo.CanWrite then failwith "Property setters are not supported"
            let getMethod = propertyInfo.GetGetMethod()
            let methodAttributes = MethodAttributes.Private |||
                                   MethodAttributes.HideBySig |||
                                   MethodAttributes.SpecialName |||
                                   MethodAttributes.NewSlot |||
                                   MethodAttributes.Virtual
            let methodName = String.Join(".", [| propertyInfo.DeclaringType.Name; getMethod.Name; |])
            let methodBuilder = builder.DefineMethod(methodName, methodAttributes, getMethod.ReturnType, [||])
            let ilGenerator = methodBuilder.GetILGenerator()
            ilGenerator.Emit(OpCodes.Ldarg_0)
            ilGenerator.Emit(OpCodes.Ldfld, fieldsMap.Item propertyInfo.Name)
            ilGenerator.Emit(OpCodes.Ret)
            builder.DefineMethodOverride(methodBuilder, getMethod)

        fieldsMap |> Map.toSeq |> Seq.map snd |> Seq.cache |> FakeMaker.CreateConstructor builder
        builder.AddInterfaceImplementation(iface)
        properties |> Seq.iteri createProperty

        builder.CreateType()

    static member private CreateConstructor (typeBuilder: TypeBuilder) (fields: FieldInfo seq) =
        let loadArgG (ilGenerator: ILGenerator) i =
            let opCodes = [| OpCodes.Ldarg_0; OpCodes.Ldarg_1; OpCodes.Ldarg_2; OpCodes.Ldarg_3 |]
            if i < opCodes.Length 
                then ilGenerator.Emit(opCodes.[i]) 
                else ilGenerator.Emit(OpCodes.Ldarg_S, byte i)

        let constructorBuilder = typeBuilder.DefineConstructor(
                                    MethodAttributes.Public ||| MethodAttributes.SpecialName ||| MethodAttributes.RTSpecialName,
                                    CallingConventions.Standard,
                                    fields |> Seq.map (fun f -> f.FieldType) |> Seq.toArray)
        let ilGenerator = constructorBuilder.GetILGenerator()
        let loadArg = loadArgG ilGenerator
        let setField i (field: FieldInfo) =
            constructorBuilder.DefineParameter(i, ParameterAttributes.None, field.Name) |> ignore
            loadArg 0
            loadArg i
            ilGenerator.Emit(OpCodes.Stfld, field)

        loadArg 0
        ilGenerator.Emit(OpCodes.Call, typeof<System.Object>.GetConstructor(Type.EmptyTypes))
        fields |> Seq.iteri (fun i f -> setField (i + 1) f)
        ilGenerator.Emit(OpCodes.Ret)

    static member GetModuleBuilder() = 
        let name = new AssemblyName("FakeMaker")
        let assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndCollect)
        assemblyBuilder.DefineDynamicModule("Implementation")
    
    interface IFakeMaker with
        member this.ImplementType(iface: Type) =
            if iface.IsInterface then createClass iface else iface

        member this.CanFake (iface: Type) = 
            let unsupportedMethods = iface |> getAllMethods |> Seq.filter (fun m -> m.IsAbstract) |> Seq.filter (fun m -> not(m.Name.StartsWith("get_")))
            iface.IsInterface && 
            not (iface.IsGenericTypeDefinition) &&
            unsupportedMethods |> Seq.isEmpty

    member this.ImplementType(iface) = (this :> IFakeMaker).ImplementType(iface)
    member this.CanFake(iface) = (this :> IFakeMaker).CanFake(iface)

    new() = FakeMaker(FakeMaker.GetModuleBuilder())
