using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Machine.Eon.Mapping.Inspection
{
  public static class KeyMapping
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(KeyMapping));

    public static AssemblyKey ToKey(this AssemblyDefinition definition)
    {
      return new AssemblyKey(definition.Name.Name);
    }

    public static TypeKey ToTypeKey(this TypeDefinition definition)
    {
      AssemblyKey assemblyKey = definition.Scope.ToAssemblyKey();
      return new TypeKey(assemblyKey, definition.FullName);
    }

    public static MethodKey ToKey(this MethodDefinition definition)
    {
      return new MethodKey(definition.DeclaringType.ToTypeKey(), definition.Name);
    }

    public static PropertyKey ToKey(this PropertyDefinition definition)
    {
      return new PropertyKey(definition.DeclaringType.ToTypeKey(), definition.Name);
    }

    public static EventKey ToKey(this EventDefinition definition)
    {
      return new EventKey(definition.DeclaringType.ToTypeKey(), definition.Name);
    }

    public static TypeKey ToTypeKey(this TypeReference reference)
    {
      if (reference.Scope == null)
      {
        throw new InvalidOperationException("I saw this before and it's gone now? I used GenericParameterTypeKey here earlier...");
      }
      if (reference is ArrayType)
      {
        return ((ArrayType)reference).ElementType.ToTypeKey();
      }
      AssemblyKey assemblyKey = reference.Scope.ToAssemblyKey();
      GenericParameter genericParameter = reference as GenericParameter;
      if (genericParameter != null)
      {
        return new GenericParameterTypeKey(assemblyKey, reference.Name);
      }
      GenericInstanceType genericInstanceType = reference as GenericInstanceType;
      if (genericInstanceType != null)
      {
        TypeReference elementType = genericInstanceType.ElementType;
        while (elementType is GenericInstanceType)
        {
          elementType = (elementType as GenericInstanceType).ElementType;
        }
        return new TypeKey(assemblyKey, elementType.FullName);
      }
      return new TypeKey(assemblyKey, reference.FullName);
    }

    public static MethodKey ToMethodKey(this MethodReference reference)
    {
      return new MethodKey(reference.DeclaringType.ToTypeKey(), reference.Name);
    }

    public static PropertyKey ToPropertyKey(this PropertyReference reference)
    {
      return new PropertyKey(reference.DeclaringType.ToTypeKey(), reference.Name);
    }

    public static FieldKey ToFieldKey(this FieldDefinition definition)
    {
      return new FieldKey(definition.DeclaringType.ToTypeKey(), definition.Name);
    }

    public static TypeKey ToTypeKey(this VariableDefinition definition)
    {
      return definition.VariableType.ToTypeKey();
    }

    public static TypeKey ToTypeKey(this ParameterDefinition definition)
    {
      return definition.ParameterType.ToTypeKey();
    }

    public static AssemblyKey ToAssemblyKey(this AssemblyNameReference reference)
    {
      return new AssemblyKey(reference.Name);
    }

    public static AssemblyKey ToAssemblyKey(this ModuleDefinition definition)
    {
      return definition.Assembly.ToKey();
    }

    public static AssemblyKey ToAssemblyKey(this IMetadataScope scope)
    {
      if (scope is AssemblyNameReference)
      {
        return ((AssemblyNameReference)scope).ToAssemblyKey();
      }
      if (scope is ModuleDefinition)
      {
        return ((ModuleDefinition)scope).ToAssemblyKey();
      }
      throw new InvalidOperationException();
    }

    public static TypeKey ToReturnTypeKey(this MethodDefinition definition)
    {
      return definition.ReturnType.ReturnType.ToTypeKey();
    }

    public static ICollection<TypeKey> ToParameterTypeKey(this MethodDefinition definition)
    {
      List<TypeKey> names = new List<TypeKey>();
      foreach (ParameterDefinition parameter in definition.Parameters)
      {
        names.Add(parameter.ParameterType.ToTypeKey());
      }
      return names;
    }
  }
}