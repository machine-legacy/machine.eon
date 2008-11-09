using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Machine.Eon.Mapping.Inspection
{
  public class CodeVisitor : BaseCodeVisitor
  {
    private readonly ModelCreator _modelCreator;

    public CodeVisitor(ModelCreator modelCreator)
    {
      _modelCreator = modelCreator;
    }

    public override void VisitInstruction(Instruction instr)
    {
      /*
      if (instr.Operand is TypeReference)
      {
        _modelCreator.UseType(((TypeReference)instr.Operand).ToTypeKey());
      }
      if (instr.Operand is FieldReference)
      {
        TypeKey typeKey = ((FieldReference)instr.Operand).FieldType.ToTypeKey();
        if (typeKey != null)
        {
          _modelCreator.UseType(typeKey);
        }
      }
      */
      if (instr.Operand is MethodReference)
      {
        _modelCreator.UseType(((MethodReference)instr.Operand).DeclaringType.ToTypeKey());
        _modelCreator.UseMethod(((MethodReference)instr.Operand).ToMethodKey());
      }
      if (instr.Operand is PropertyReference)
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

    public override void VisitVariableDefinitionCollection(VariableDefinitionCollection variables)
    {
      /*
      foreach (VariableDefinition variable in variables)
      {
        _modelCreator.UseType(variable.ToTypeKey());
      }
      */
    }

    public override void VisitMethodBody(MethodBody body) { }

    public override void VisitScope(Scope scope) { }

    public override void VisitScopeCollection(ScopeCollection scopes) { }

    public override void VisitVariableDefinition(VariableDefinition variable) { }

    public override void TerminateMethodBody(MethodBody body) { }

    public override void VisitExceptionHandler(ExceptionHandler eh) { }

    public override void VisitExceptionHandlerCollection(ExceptionHandlerCollection seh) { }
  }
}