using Mono.Cecil;

namespace Machine.Eon.Mapping.Inspection
{
  public class AssemblyStructureVisitor : BaseStructureVisitor
  {
    private readonly ModelCreator _modelCreator;
    private readonly VisitationOptions _options;

    public AssemblyStructureVisitor(ModelCreator modelCreator, VisitationOptions options)
    {
      _modelCreator = modelCreator;
      _options = options;
    }

    public override void VisitAssemblyDefinition(AssemblyDefinition asm)
    {
      _modelCreator.StartAssembly(asm.ToKey());
    }

    public override void TerminateAssemblyDefinition(AssemblyDefinition asm)
    {
      _modelCreator.EndAssembly();
    }

    public override void VisitModuleDefinition(ModuleDefinition module)
    {
      module.Accept(new ReflectionVisitor(_modelCreator, _options));
    }

    public override void VisitModuleDefinitionCollection(ModuleDefinitionCollection modules)
    {
      foreach (ModuleDefinition module in modules)
      {
        module.Accept(this);
      }
    }

    public override void VisitModuleReference(ModuleReference module) { }

    public override void VisitModuleReferenceCollection(ModuleReferenceCollection modules) { }

    public override void VisitResourceCollection(ResourceCollection resources) { }

    public override void VisitAssemblyLinkedResource(AssemblyLinkedResource res) { }

    public override void VisitAssemblyNameDefinition(AssemblyNameDefinition name) { }

    public override void VisitAssemblyNameReference(AssemblyNameReference name) { }

    public override void VisitAssemblyNameReferenceCollection(AssemblyNameReferenceCollection names) { }

    public override void VisitEmbeddedResource(EmbeddedResource res) { }

    public override void VisitLinkedResource(LinkedResource res) { }
  }
}