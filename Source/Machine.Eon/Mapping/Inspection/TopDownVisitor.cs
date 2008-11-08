using System;
using System.Collections.Generic;

using Mono.Cecil;

namespace Machine.Eon.Mapping.Inspection
{
  public class TopDownVisitor
  {
    private readonly Stack<TypeDefinition> _typeDefinitions = new Stack<TypeDefinition>();
    private readonly Stack<TypeReference> _typeReferences = new Stack<TypeReference>();
    private readonly ModelCreator _modelCreator;
    private readonly VisitationOptions _options;

    public TopDownVisitor(ModelCreator modelCreator, VisitationOptions options)
    {
      _modelCreator = modelCreator;
      _options = options;
    }

    public void Visit(AssemblyDefinition assembly)
    {
      foreach (ModuleDefinition module in assembly.Modules)
      {
        foreach (TypeDefinition type in module.Types)
        {
          if (_options.ShouldVisit(type.ToTypeKey()))
          {
            Visit(type);
          }
        }
      }
    }

    private void Visit(TypeDefinition type)
    {
      TypeKey typeKey = type.ToTypeKey();
      if (_typeDefinitions.Contains(type))
      {
        return;
      }
      _typeDefinitions.Push(type);
      _modelCreator.StartNamespace(typeKey.Namespace);
      _modelCreator.StartType(typeKey);
      _modelCreator.SetTypeFlags(type.IsInterface, type.IsAbstract, false, !_options.VisitMembers);
      
      Visit(type.Interfaces);
      Visit(type.GenericParameters);
      Visit(type.CustomAttributes);

      if (_options.VisitMembers)
      {
        if (type.BaseType != null)
        {
          Visit(type.BaseType);
          _modelCreator.SetBaseType(type.BaseType.ToTypeKey());
        }

        Visit(type.Properties);
        Visit(type.Fields);
        Visit(type.Events);
      }
      Visit(type.Constructors);
      Visit(type.Methods);

      _modelCreator.EndType();
      _modelCreator.EndNamespace();
      _typeDefinitions.Pop();
    }

    private void Visit(PropertyDefinition property)
    {
      _modelCreator.StartProperty(property.ToKey());
      _modelCreator.SetPropertyType(property.PropertyType.ToTypeKey());
      _modelCreator.UseType(property.PropertyType.ToTypeKey());
      Visit(property.CustomAttributes);
      if (property.GetMethod != null)
      {
        Visit(property.GetMethod);
        _modelCreator.SetPropertyGetter(property.ToKey(), property.GetMethod.ToMethodKey());
      }
      if (property.SetMethod != null)
      {
        Visit(property.SetMethod);
        _modelCreator.SetPropertySetter(property.ToKey(), property.SetMethod.ToMethodKey());
      }
      _modelCreator.EndProperty();
    }

    private void Visit(FieldDefinition field)
    {
      _modelCreator.StartField(field.ToFieldKey());
      Visit(field.CustomAttributes);
      _modelCreator.SetFieldType(field.FieldType.ToTypeKey());
      _modelCreator.UseType(field.FieldType.ToTypeKey());
      _modelCreator.EndField();
    }

    private void Visit(EventDefinition eventDefinition)
    {
      _modelCreator.StartEvent(eventDefinition.ToKey());
      Visit(eventDefinition.CustomAttributes);
      _modelCreator.SetEventType(eventDefinition.EventType.ToTypeKey());
      _modelCreator.UseType(eventDefinition.EventType.ToTypeKey());
      if (eventDefinition.AddMethod != null)
      {
        Visit(eventDefinition.AddMethod);
        _modelCreator.SetEventAdder(eventDefinition.ToKey(), eventDefinition.AddMethod.ToMethodKey());
      }
      if (eventDefinition.RemoveMethod != null)
      {
        Visit(eventDefinition.RemoveMethod);
        _modelCreator.SetEventRemover(eventDefinition.ToKey(), eventDefinition.RemoveMethod.ToMethodKey());
      }
      _modelCreator.EndEvent();
    }

    private void Visit(MethodDefinition method)
    {
      if (!_options.ShouldVisit(method.ToMethodKey()))
      {
        return;
      }
      _modelCreator.StartMethod(method.ToKey());
      Visit(method.CustomAttributes);
      _modelCreator.SetMethodPrototype(method.ToReturnTypeKey(), method.ToParameterTypeKey());
      _modelCreator.SetMethodFlags(method.IsConstructor, method.IsAbstract, method.IsVirtual, method.IsStatic, _options.VisitMembers);
      _modelCreator.UseType(method.ToReturnTypeKey());
      foreach (TypeKey typeName in method.ToParameterTypeKey())
      {
        _modelCreator.UseType(typeName);
      }
      if (method.Body != null && _options.VisitMembers)
      {
        method.Body.Accept(new CodeVisitor(_modelCreator));
      }
      _modelCreator.EndMethod();
    }
    
    private void Visit(TypeReference type)
    {
      if (_typeReferences.Contains(type))
      {
        return;
      }
      _typeReferences.Push(type);
      _modelCreator.UseType(type.ToTypeKey());
      GenericInstanceType genericInstanceType = type as GenericInstanceType;
      if (genericInstanceType != null)
      {
        Visit(genericInstanceType.GenericArguments);
      }
      _typeReferences.Pop();
    }

    private void Visit(GenericArgumentCollection genericArgumentCollection)
    {
      foreach (TypeReference reference in genericArgumentCollection)
      {
        Visit(reference);
      }
    }

    private void Visit(GenericParameterCollection genericParameterCollection)
    {
      foreach (GenericParameter parameter in genericParameterCollection)
      {
        foreach (TypeReference reference in parameter.Constraints)
        {
          _modelCreator.UseType(reference.ToTypeKey());
        }
        _modelCreator.UseType(parameter.ToTypeKey());
      }
    }

    private void Visit(InterfaceCollection interfaceCollection)
    {
      foreach (TypeReference interfaceType in interfaceCollection)
      {
        _modelCreator.ImplementsInterface(interfaceType.ToTypeKey());
        _modelCreator.UseType(interfaceType.ToTypeKey());
      }
    }

    private void Visit(ConstructorCollection constructorCollection)
    {
      foreach (MethodDefinition method in constructorCollection)
      {
        Visit(method);
      }
    }

    private void Visit(MethodDefinitionCollection methodDefinitionCollection)
    {
      foreach (MethodDefinition method in methodDefinitionCollection)
      {
        Visit(method);
      }
    }

    private void Visit(PropertyDefinitionCollection propertyDefinitionCollection)
    {
      foreach (PropertyDefinition property in propertyDefinitionCollection)
      {
        Visit(property);
      }
    }

    private void Visit(FieldDefinitionCollection fieldDefinitionCollection)
    {
      foreach (FieldDefinition field in fieldDefinitionCollection)
      {
        Visit(field);
      }
    }

    private void Visit(EventDefinitionCollection eventDefinitionCollection)
    {
      foreach (EventDefinition eventDefinition in eventDefinitionCollection)
      {
        Visit(eventDefinition);
      }
    }

    private void Visit(CustomAttributeCollection attributes)
    {
      foreach (CustomAttribute attributeReference in attributes)
      {
        Visit(attributeReference.Constructor.DeclaringType);
      }
    }
  }
}
