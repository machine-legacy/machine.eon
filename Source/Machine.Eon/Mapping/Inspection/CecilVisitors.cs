using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Machine.Eon.Mapping.Inspection
{
  public class MyReflectionStructureVisitor : BaseStructureVisitor
  {
    private readonly ModelCreator _modelCreator;

    public MyReflectionStructureVisitor(ModelCreator modelCreator)
    {
      _modelCreator = modelCreator;
    }

    public override void VisitAssemblyDefinition(AssemblyDefinition asm)
    {
      _modelCreator.StartAssembly(asm.ToKey());
    }

    public override void TerminateAssemblyDefinition(AssemblyDefinition asm)
    {
      _modelCreator.EndAssembly();
    }

    public override void VisitAssemblyLinkedResource(AssemblyLinkedResource res)
    {
    }

    public override void VisitAssemblyNameDefinition(AssemblyNameDefinition name)
    {
    }

    public override void VisitAssemblyNameReference(AssemblyNameReference name)
    {
    }

    public override void VisitAssemblyNameReferenceCollection(AssemblyNameReferenceCollection names)
    {
      foreach (AssemblyNameReference name in names)
      {
        name.Accept(this);
      }
    }

    public override void VisitEmbeddedResource(EmbeddedResource res)
    {
    }

    public override void VisitLinkedResource(LinkedResource res)
    {
    }

    public override void VisitModuleDefinition(ModuleDefinition module)
    {
      module.Accept(new MyReflectionVisitor(_modelCreator));
    }

    public override void VisitModuleDefinitionCollection(ModuleDefinitionCollection modules)
    {
      foreach (ModuleDefinition module in modules)
      {
        module.Accept(this);
      }
    }

    public override void VisitModuleReference(ModuleReference module)
    {
    }

    public override void VisitModuleReferenceCollection(ModuleReferenceCollection modules)
    {
      foreach (ModuleReference module in modules)
      {
        module.Accept(this);
      }
    }

    public override void VisitResourceCollection(ResourceCollection resources)
    {
      foreach (Resource resource in resources)
      {
        resource.Accept(this);
      }
    }
  }
  public class MyReflectionVisitor : BaseReflectionVisitor
  {
    private readonly ModelCreator _modelCreator;

    public MyReflectionVisitor(ModelCreator modelCreator)
    {
      _modelCreator = modelCreator;
    }

    public override void TerminateModuleDefinition(ModuleDefinition module)
    {
    }

    public override void VisitConstructor(MethodDefinition ctor)
    {
      ctor.Body.Accept(new MyCodeVisitor(_modelCreator));
    }

    public override void VisitConstructorCollection(ConstructorCollection ctors)
    {
      foreach (MethodDefinition ctor in ctors)
      {
        ApplyMethodListeners(ctor);
      }
    }

    public override void VisitCustomAttribute(CustomAttribute customAttr)
    {
    }

    public override void VisitCustomAttributeCollection(CustomAttributeCollection customAttrs)
    {
      foreach (CustomAttribute customAttribute in customAttrs)
      {
        _modelCreator.HasAttribute(customAttribute.Constructor.DeclaringType.ToTypeKey());
        customAttribute.Accept(this);
      }
    }

    public override void VisitEventDefinition(EventDefinition evt)
    {
    }

    public override void VisitEventDefinitionCollection(EventDefinitionCollection events)
    {
      foreach (EventDefinition eventDefinition in events)
      {
        eventDefinition.Accept(this);
      }
    }

    public override void VisitExternType(TypeReference externType)
    {
    }

    public override void VisitExternTypeCollection(ExternTypeCollection externTypes)
    {
      foreach (TypeReference type in externTypes)
      {
        type.Accept(this);
      }
    }

    public override void VisitFieldDefinition(FieldDefinition field)
    {
    }

    public override void VisitFieldDefinitionCollection(FieldDefinitionCollection fields)
    {
      foreach (FieldDefinition field in fields)
      {
        _modelCreator.StartField(field.ToFieldKey());
        _modelCreator.SetFieldType(field.FieldType.ToTypeKey());
        _modelCreator.UseType(field.FieldType.ToTypeKey());
        field.Accept(this);
        _modelCreator.EndField();
      }
    }

    public override void VisitGenericParameter(GenericParameter parameter)
    {
    }

    public override void VisitGenericParameterCollection(GenericParameterCollection parameters)
    {
    }

    public override void VisitInterface(TypeReference interf)
    {
    }

    public override void VisitInterfaceCollection(InterfaceCollection interfaces)
    {
      foreach (TypeReference type in interfaces)
      {
        _modelCreator.ImplementsInterface(type.ToTypeKey());
        _modelCreator.UseType(type.ToTypeKey());
        type.Accept(this);
      }
    }

    public override void VisitMarshalSpec(MarshalSpec marshalSpec)
    {
    }

    public override void VisitMemberReference(MemberReference member)
    {
    }

    public override void VisitMemberReferenceCollection(MemberReferenceCollection members)
    {
      foreach (MemberReference member in members)
      {
        member.Accept(this);
      }
    }

    public override void VisitMethodDefinition(MethodDefinition method)
    {
      if (method.Body != null)
      {
        method.Body.Accept(new MyCodeVisitor(_modelCreator));
      }
    }

    public override void VisitMethodDefinitionCollection(MethodDefinitionCollection methods)
    {
      foreach (MethodDefinition method in methods)
      {
        ApplyMethodListeners(method);
      }
    }

    public override void VisitModuleDefinition(ModuleDefinition module)
    {
    }

    public override void VisitNestedType(TypeDefinition nestedType)
    {
    }

    public override void VisitNestedTypeCollection(NestedTypeCollection nestedTypes)
    {
      foreach (TypeDefinition type in nestedTypes)
      {
        ApplyTypeVisitors(type);
      }
    }

    public override void VisitOverride(MethodReference ov)
    {
    }

    public override void VisitOverrideCollection(OverrideCollection overrides)
    {
      foreach (MethodReference method in overrides)
      {
        method.Accept(this);
      }
    }

    public override void VisitPInvokeInfo(PInvokeInfo pinvk)
    {
    }

    public override void VisitParameterDefinition(ParameterDefinition parameter)
    {
      _modelCreator.UseType(parameter.ToTypeKey());
    }

    public override void VisitParameterDefinitionCollection(ParameterDefinitionCollection parameters)
    {
      foreach (ParameterDefinition parameter in parameters)
      {
        parameter.Accept(this);
      }
    }

    public override void VisitPropertyDefinition(PropertyDefinition property)
    {
      _modelCreator.StartProperty(property.ToKey());
      _modelCreator.SetPropertyType(property.PropertyType.ToTypeKey());
      _modelCreator.UseType(property.PropertyType.ToTypeKey());
      property.CustomAttributes.Accept(this);
      if (property.GetMethod != null)
      {
        ApplyMethodListeners(property.GetMethod);
        _modelCreator.SetPropertyGetter(property.ToKey(), property.GetMethod.ToMethodKey());
      }
      if (property.SetMethod != null)
      {
        ApplyMethodListeners(property.SetMethod);
        _modelCreator.SetPropertySetter(property.ToKey(), property.SetMethod.ToMethodKey());
      }
      _modelCreator.EndProperty();
    }

    public override void VisitPropertyDefinitionCollection(PropertyDefinitionCollection properties)
    {
      foreach (PropertyDefinition property in properties)
      {
        property.Accept(this);
      }
    }

    public override void VisitSecurityDeclaration(SecurityDeclaration secDecl)
    {
    }

    public override void VisitSecurityDeclarationCollection(SecurityDeclarationCollection securityDeclarations)
    {
      foreach (SecurityDeclaration securityDeclaration in securityDeclarations)
      {
        securityDeclaration.Accept(this);
      }
    }

    public override void VisitTypeDefinition(TypeDefinition type)
    {
    }

    public override void VisitTypeDefinitionCollection(TypeDefinitionCollection types)
    {
      foreach (TypeDefinition type in types)
      {
        ApplyTypeVisitors(type);
      }
    }

    private void ApplyTypeVisitors(TypeDefinition type)
    {
      TypeKey typeKey = type.ToTypeKey();
      _modelCreator.StartNamespace(typeKey.Namespace);
      _modelCreator.StartType(typeKey);
      _modelCreator.SetTypeFlags(type.IsInterface, type.IsAbstract, false);
      if (type.BaseType != null)
      {
        type.BaseType.Accept(this);
        _modelCreator.SetBaseType(type.BaseType.ToTypeKey());
      }
      foreach (GenericParameter parameter in type.GenericParameters)
      {
        _modelCreator.UseType(parameter.ToTypeKey());
      }
      type.Accept(this);
      _modelCreator.EndType();
      _modelCreator.EndNamespace();
    }

    public override void VisitTypeReference(TypeReference type)
    {
      GenericInstanceType genericInstanceType = type as GenericInstanceType;
      _modelCreator.UseType(type.ToTypeKey());
      if (genericInstanceType != null)
      {
        foreach (TypeReference reference in genericInstanceType.GenericArguments)
        {
          reference.Accept(this);
        }
      }
    }

    public override void VisitTypeReferenceCollection(TypeReferenceCollection refs)
    {
      foreach (TypeReference reference in refs)
      {
        reference.Accept(this);
      }
    }

    private void ApplyMethodListeners(MethodDefinition method)
    {
      _modelCreator.StartMethod(method.ToKey());
      _modelCreator.SetMethodPrototype(method.ToReturnTypeKey(), method.ToParameterTypeKey());
      _modelCreator.SetMethodFlags(method.IsConstructor, method.IsAbstract, method.IsVirtual, method.IsStatic);
      _modelCreator.UseType(method.ToReturnTypeKey());
      foreach (TypeKey typeName in method.ToParameterTypeKey())
      {
        _modelCreator.UseType(typeName);
      }
      method.Accept(this);
      _modelCreator.EndMethod();
    }
  }
  public class MyCodeVisitor : BaseCodeVisitor
  {
    private readonly ModelCreator _modelCreator;

    public MyCodeVisitor(ModelCreator modelCreator)
    {
      _modelCreator = modelCreator;
    }

    public override void TerminateMethodBody(MethodBody body)
    {
    }

    public override void VisitExceptionHandler(ExceptionHandler eh)
    {
    }

    public override void VisitExceptionHandlerCollection(ExceptionHandlerCollection seh)
    {
      foreach (ExceptionHandler handler in seh)
      {
        handler.Accept(this);
      }
    }

    public override void VisitInstruction(Instruction instr)
    {
      if (instr.Operand is TypeReference)
      {
        _modelCreator.UseType(((TypeReference)instr.Operand).ToTypeKey());
      }
      else if (instr.Operand is FieldReference)
      {
        TypeKey typeKey = ((FieldReference)instr.Operand).FieldType.ToTypeKey();
        if (typeKey != null)
        {
          _modelCreator.UseType(typeKey);
        }
      }
      else if (instr.Operand is MethodReference)
      {
        _modelCreator.UseType(((MethodReference)instr.Operand).DeclaringType.ToTypeKey());
        _modelCreator.UseMethod(((MethodReference)instr.Operand).ToMethodKey());
      }
      else if (instr.Operand is PropertyReference)
      {
        _modelCreator.UseType(((PropertyReference)instr.Operand).DeclaringType.ToTypeKey());
        _modelCreator.UseProperty(((PropertyReference)instr.Operand).ToPropertyKey());
      }
    }

    public override void VisitInstructionCollection(InstructionCollection instructions)
    {
      foreach (Instruction instruction in instructions)
      {
        instruction.Accept(this);
      }
    }

    public override void VisitMethodBody(MethodBody body)
    {
    }

    public override void VisitScope(Scope scope)
    {
    }

    public override void VisitScopeCollection(ScopeCollection scopes)
    {
      foreach (Scope scope in scopes)
      {
        scope.Accept(this);
      }
    }

    public override void VisitVariableDefinition(VariableDefinition variable)
    {
      _modelCreator.UseType(variable.ToTypeKey());
    }

    public override void VisitVariableDefinitionCollection(VariableDefinitionCollection variables)
    {
      foreach (VariableDefinition variable in variables)
      {
        variable.Accept(this);
      }
    }
  }
}
