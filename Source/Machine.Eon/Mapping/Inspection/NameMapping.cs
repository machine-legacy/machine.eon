using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Machine.Eon.Mapping.Inspection
{
  public static class NameMapping
  {
    public static AssemblyName ToName(this AssemblyDefinition definition)
    {
      return new AssemblyName(definition.Name.Name);
    }

    public static TypeName ToTypeName(this TypeDefinition definition)
    {
      AssemblyName assemblyName = definition.Scope.ToAssemblyName();
      return new TypeName(assemblyName, definition.FullName);
    }

    public static MethodName ToName(this MethodDefinition definition)
    {
      return new MethodName(definition.DeclaringType.ToTypeName(), definition.Name);
    }

    public static PropertyName ToName(this PropertyDefinition definition)
    {
      return new PropertyName(definition.DeclaringType.ToTypeName(), definition.Name);
    }

    public static TypeName ToTypeName(this TypeReference reference)
    {
      if (reference is FunctionPointerType)
      {
        return null;
      }
      if (reference.Scope == null)
      {
        return new GenericParameterTypeName(AssemblyName.None, reference.Name);
      }
      AssemblyName assemblyName = reference.Scope.ToAssemblyName();
      GenericInstanceType genericInstanceType = reference as GenericInstanceType;
      if (genericInstanceType != null)
      {
        return new TypeName(assemblyName, reference.FullName);
      }
      return new TypeName(assemblyName, reference.FullName);
    }

    public static MethodName ToMethodName(this MethodReference reference)
    {
      return new MethodName(reference.DeclaringType.ToTypeName(), reference.Name);
    }

    public static PropertyName ToPropertyName(this PropertyReference reference)
    {
      return new PropertyName(reference.DeclaringType.ToTypeName(), reference.Name);
    }

    public static FieldName ToFieldName(this FieldDefinition definition)
    {
      return new FieldName(definition.DeclaringType.ToTypeName(), definition.Name);
    }

    public static TypeName ToTypeName(this VariableDefinition definition)
    {
      return definition.VariableType.ToTypeName();
    }

    public static TypeName ToTypeName(this ParameterDefinition definition)
    {
      return definition.ParameterType.ToTypeName();
    }

    public static AssemblyName ToAssemblyName(this AssemblyNameReference reference)
    {
      return new AssemblyName(reference.Name);
    }

    public static AssemblyName ToAssemblyName(this ModuleDefinition definition)
    {
      return definition.Assembly.ToName();
    }

    public static AssemblyName ToAssemblyName(this IMetadataScope scope)
    {
      if (scope is AssemblyNameReference)
      {
        return ((AssemblyNameReference)scope).ToAssemblyName();
      }
      if (scope is ModuleDefinition)
      {
        return ((ModuleDefinition)scope).ToAssemblyName();
      }
      throw new InvalidOperationException();
    }

    public static TypeName ToReturnTypeName(this MethodDefinition definition)
    {
      return definition.ReturnType.ReturnType.ToTypeName();
    }

    public static ICollection<TypeName> ToParameterTypeNames(this MethodDefinition definition)
    {
      List<TypeName> names = new List<TypeName>();
      foreach (ParameterDefinition parameter in definition.Parameters)
      {
        names.Add(parameter.ParameterType.ToTypeName());
      }
      return names;
    }
  }
}