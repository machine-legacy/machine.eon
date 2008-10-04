using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Machine.Eon.Mapping
{
  public class Mapper
  {
    public void Include(string path)
    {
      AssemblyDefinition definition = AssemblyFactory.GetAssembly(path);
      MyReflectionStructureVisitor visitor = new MyReflectionStructureVisitor();
      definition.Accept(visitor);
    }
  }
  public class MyReflectionStructureVisitor : BaseStructureVisitor
  {
    #region IReflectionStructureVisitor Members
    public override void TerminateAssemblyDefinition(AssemblyDefinition asm)
    {
    }

    public override void VisitAssemblyDefinition(AssemblyDefinition asm)
    {
      Console.WriteLine("Assembly");
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
      Console.WriteLine("Module");
      module.Accept(new MyReflectionVisitor());
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
    #region IReflectionVisitor Members

    public override void TerminateModuleDefinition(ModuleDefinition module)
    {
    }

    public override void VisitConstructor(MethodDefinition ctor)
    {
      ctor.Body.Accept(new MyCodeVisitor());
    }

    public override void VisitConstructorCollection(ConstructorCollection ctors)
    {
      foreach (MethodDefinition ctor in ctors)
      {
        ctor.Accept(this);
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
      Console.WriteLine("Method " + method.Name);
      method.Body.Accept(new MyCodeVisitor());
    }

    public override void VisitMethodDefinitionCollection(MethodDefinitionCollection methods)
    {
      foreach (MethodDefinition method in methods)
      {
        method.Accept(this);
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
        type.Accept(this);
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
      Console.WriteLine("Type: " + type);
    }

    public override void VisitTypeDefinitionCollection(TypeDefinitionCollection types)
    {
      foreach (TypeDefinition type in types)
      {
        type.Accept(this);
      }
    }

    public override void VisitTypeReference(TypeReference type)
    {
      // Console.WriteLine("Type Reference: {0}", type);
    }

    public override void VisitTypeReferenceCollection(TypeReferenceCollection refs)
    {
      // Console.WriteLine("Type References:");
      foreach (TypeReference reference in refs)
      {
        reference.Accept(this);
      }
    }
    #endregion
  }
  public class MyCodeVisitor : BaseCodeVisitor
  {
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
      // Console.WriteLine("    " + instr.OpCode);
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

    public override void VisitVariableDefinition(VariableDefinition var)
    {
      Console.WriteLine("    {0} {1}", var.Name, var.VariableType);
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
}
