using System;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Machine.Eon.Mapping
{
  public class Mapper
  {
    public void Include(string path)
    {
      AssemblyDefinition definition = AssemblyFactory.GetAssembly(path);
      Listener listener = new Listener();
      MyReflectionStructureVisitor visitor = new MyReflectionStructureVisitor(listener);
      definition.Accept(visitor);
    }
  }
  public class MyReflectionStructureVisitor : BaseStructureVisitor
  {
    private readonly Listener _listener;

    public MyReflectionStructureVisitor(Listener listener)
    {
      _listener = listener;
    }

    #region IReflectionStructureVisitor Members
    public override void VisitAssemblyDefinition(AssemblyDefinition asm)
    {
      _listener.StartAssembly(asm.ToName());
    }

    public override void TerminateAssemblyDefinition(AssemblyDefinition asm)
    {
      _listener.EndAssembly();
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
      module.Accept(new MyReflectionVisitor(_listener));
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
      foreach (ResourceCollection resource in resources)
      {
        resource.Accept(this);
      }
    }
    #endregion
  }
  public class MyReflectionVisitor : BaseReflectionVisitor
  {
    private readonly Listener _listener;

    public MyReflectionVisitor(Listener listener)
    {
      _listener = listener;
    }

    #region IReflectionVisitor Members
    public override void TerminateModuleDefinition(ModuleDefinition module)
    {
    }

    public override void VisitConstructor(MethodDefinition ctor)
    {
      ctor.Body.Accept(new MyCodeVisitor(_listener));
    }

    public override void VisitConstructorCollection(ConstructorCollection ctors)
    {
      foreach (MethodDefinition ctor in ctors)
      {
        _listener.StartMethod(ctor.ToName());
        ctor.Accept(this);
        _listener.EndMethod();
      }
    }

    public override void VisitCustomAttribute(CustomAttribute customAttr)
    {
    }

    public override void VisitCustomAttributeCollection(CustomAttributeCollection customAttrs)
    {
      foreach (CustomAttribute customAttribute in customAttrs)
      {
        customAttribute.Accept(this);
      }
    }

    public override void VisitEventDefinition(EventDefinition evt)
    {
    }

    public override void VisitEventDefinitionCollection(EventDefinitionCollection events)
    {
      foreach (EventDefinitionCollection eventDefinition in events)
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
        field.Accept(this);
      }
    }

    public override void VisitGenericParameter(GenericParameter parameter)
    {
    }

    public override void VisitGenericParameterCollection(GenericParameterCollection parameters)
    {
      foreach (GenericParameter parameter in parameters)
      {
        parameter.Accept(this);
      }
    }

    public override void VisitInterface(TypeReference interf)
    {
    }

    public override void VisitInterfaceCollection(InterfaceCollection interfaces)
    {
      foreach (TypeReference type in interfaces)
      {
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
        method.Body.Accept(new MyCodeVisitor(_listener));
      }
    }

    public override void VisitMethodDefinitionCollection(MethodDefinitionCollection methods)
    {
      foreach (MethodDefinition method in methods)
      {
        _listener.StartMethod(method.ToName());
        method.Accept(this);
        _listener.EndMethod();
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
        _listener.StartNamespace(type.ToNamespaceName());
        _listener.StartType(type.ToTypeName());
        type.Accept(this);
        _listener.EndType();
        _listener.EndNamespace();
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
      _listener.UseType(parameter.ToTypeName());
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
        _listener.StartNamespace(type.ToNamespaceName());
        _listener.StartType(type.ToTypeName());
        type.Accept(this);
        _listener.EndType();
        _listener.EndNamespace();
      }
    }

    public override void VisitTypeReference(TypeReference type)
    {
      _listener.UseType(type.ToTypeName());
    }

    public override void VisitTypeReferenceCollection(TypeReferenceCollection refs)
    {
      foreach (TypeReference reference in refs)
      {
        reference.Accept(this);
      }
    }
    #endregion
  }
  public class MyCodeVisitor : BaseCodeVisitor
  {
    private readonly Listener _listener;

    public MyCodeVisitor(Listener listener)
    {
      _listener = listener;
    }

    #region ICodeVisitor Members
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
        _listener.UseType(((TypeReference)instr.Operand).ToTypeName());
      }
      else if (instr.Operand is FieldReference)
      {
        _listener.UseType(((FieldReference)instr.Operand).FieldType.ToTypeName());
      }
      else if (instr.Operand is MethodReference)
      {
        _listener.UseType(((MethodReference) instr.Operand).DeclaringType.ToTypeName());
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
      _listener.UseType(variable.ToTypeName());
    }

    public override void VisitVariableDefinitionCollection(VariableDefinitionCollection variables)
    {
      foreach (VariableDefinition variable in variables)
      {
        variable.Accept(this);
      }
    }
    #endregion
  }

  public static class Mapping
  {
    public static AssemblyName ToName(this AssemblyDefinition definition)
    {
      return new AssemblyName(definition.Name.FullName);
    }

    public static TypeName ToTypeName(this TypeDefinition definition)
    {
      return new TypeName(definition.FullName);
    }

    public static NamespaceName ToNamespaceName(this TypeDefinition definition)
    {
      return new NamespaceName(definition.Namespace);
    }

    public static MethodName ToName(this MethodDefinition definition)
    {
      return new MethodName(definition.DeclaringType.ToTypeName(), definition.Name);
    }

    public static TypeName ToTypeName(this TypeReference reference)
    {
      return new TypeName(reference.FullName);
    }

    public static TypeName ToTypeName(this VariableDefinition definition)
    {
      return definition.VariableType.ToTypeName();
    }

    public static TypeName ToTypeName(this ParameterDefinition definition)
    {
      return definition.ParameterType.ToTypeName();
    }
  }
}
