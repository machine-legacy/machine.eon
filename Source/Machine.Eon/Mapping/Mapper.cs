using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Machine.Eon.Mapping.Repositories;
using Machine.Eon.Mapping.Repositories.Impl;

namespace Machine.Eon.Mapping
{
  public class Mapper
  {
    private readonly IAssemblyRepository _assemblyRepository;
    private readonly ITypeRepository _typeRepository;
    private readonly IMemberRepository _memberRepository;

    public Mapper()
    {
      _assemblyRepository = new AssemblyRepository();
      _typeRepository = new TypeRepository(_assemblyRepository);
      _memberRepository = new MemberRepository(_assemblyRepository);
    }

    public void Include(string path)
    {
      AssemblyDefinition definition = AssemblyFactory.GetAssembly(path);
      Listener listener = new Listener(_typeRepository, _memberRepository);
      MyReflectionStructureVisitor visitor = new MyReflectionStructureVisitor(listener);
      definition.Accept(visitor);
    }

    public QueryRoot ToQueryRoot()
    {
      return new QueryRoot(_assemblyRepository.FindAll());
    }
  }
  public class MyReflectionStructureVisitor : BaseStructureVisitor
  {
    private readonly Listener _listener;

    public MyReflectionStructureVisitor(Listener listener)
    {
      _listener = listener;
    }

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
  }
  public class MyReflectionVisitor : BaseReflectionVisitor
  {
    private readonly Listener _listener;

    public MyReflectionVisitor(Listener listener)
    {
      _listener = listener;
    }

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
        _listener.HasAttribute(customAttribute.Constructor.DeclaringType.ToTypeName());
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
        _listener.UseType(field.FieldType.ToTypeName());
        _listener.StartField(field.ToFieldName());
        _listener.SetFieldType(field.FieldType.ToTypeName());
        field.Accept(this);
        _listener.EndField();
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
        _listener.UseType(type.ToTypeName());
        _listener.ImplementsInterface(type.ToTypeName());
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
      _listener.UseType(property.PropertyType.ToTypeName());
      _listener.StartProperty(property.ToName());
      _listener.SetPropertyType(property.PropertyType.ToTypeName());
      property.CustomAttributes.Accept(this);
      if (property.GetMethod != null)
      {
        ApplyMethodListeners(property.GetMethod);
        _listener.SetPropertyGetter(property.ToName(), property.GetMethod.ToMethodName());
      }
      if (property.SetMethod != null)
      {
        ApplyMethodListeners(property.SetMethod);
        _listener.SetPropertySetter(property.ToName(), property.SetMethod.ToMethodName());
      }
      _listener.EndProperty();
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
      TypeName typeName = type.ToTypeName();
      _listener.StartNamespace(typeName.Namespace);
      _listener.StartType(typeName);
      _listener.SetTypeFlags(type.IsInterface, type.IsAbstract);
      if (type.BaseType != null)
      {
        _listener.SetBaseType(type.BaseType.ToTypeName());
        _listener.UseType(type.BaseType.ToTypeName());
      }
      type.Accept(this);
      _listener.EndType();
      _listener.EndNamespace();
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

    private void ApplyMethodListeners(MethodDefinition method)
    {
      _listener.UseType(method.ToReturnTypeName());
      foreach (ParameterDefinition parameter in method.Parameters)
      {
        _listener.UseType(parameter.ParameterType.ToTypeName());
      }
      _listener.StartMethod(method.ToName());
      _listener.SetMethodPrototype(method.ToReturnTypeName(), method.ToParameterTypeNames());
      _listener.UseType(method.ToReturnTypeName());
      foreach (TypeName typeName in method.ToParameterTypeNames())
      {
        _listener.UseType(typeName);
      }
      method.Accept(this);
      _listener.EndMethod();
    }
  }
  public class MyCodeVisitor : BaseCodeVisitor
  {
    private readonly Listener _listener;

    public MyCodeVisitor(Listener listener)
    {
      _listener = listener;
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
        _listener.UseType(((TypeReference)instr.Operand).ToTypeName());
      }
      else if (instr.Operand is FieldReference)
      {
        TypeName typeName = ((FieldReference)instr.Operand).FieldType.ToTypeName();
        if (typeName != null)
        {
          _listener.UseType(typeName);
        }
      }
      else if (instr.Operand is MethodReference)
      {
        _listener.UseType(((MethodReference)instr.Operand).DeclaringType.ToTypeName());
        _listener.UseMethod(((MethodReference)instr.Operand).ToMethodName());
      }
      else if (instr.Operand is PropertyReference)
      {
        _listener.UseType(((PropertyReference)instr.Operand).DeclaringType.ToTypeName());
        _listener.UseProperty(((PropertyReference)instr.Operand).ToPropertyName());
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
  }
  public static class Mapping
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
      if (reference.Scope == null)
      {
        return new GenericParameterTypeName(AssemblyName.None, reference.Name);
      }
      AssemblyName assemblyName = reference.Scope.ToAssemblyName();
      GenericInstanceType genericInstanceType = reference as GenericInstanceType;
      if (genericInstanceType != null)
      {
        return new TypeName(assemblyName, reference.FullName.Substring(0, reference.FullName.IndexOf('`')));
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
