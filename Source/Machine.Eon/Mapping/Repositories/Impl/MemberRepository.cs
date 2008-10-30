using System;

namespace Machine.Eon.Mapping.Repositories.Impl
{
  public class MemberRepository : IMemberRepository
  {
    private readonly IAssemblyRepository _assemblyRepository;

    public MemberRepository(IAssemblyRepository assemblyRepository)
    {
      _assemblyRepository = assemblyRepository;
    }

    public Method FindMethod(MethodName name)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(name.TypeName.AssemblyName);
      if (assembly == null)
      {
        assembly = new Assembly(name.TypeName.AssemblyName);
        _assemblyRepository.SaveAssembly(assembly);
      }
      return assembly.FindOrCreateMethod(name);
    }

    public Property FindProperty(PropertyName name)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(name.TypeName.AssemblyName);
      if (assembly == null)
      {
        assembly = new Assembly(name.TypeName.AssemblyName);
        _assemblyRepository.SaveAssembly(assembly);
      }
      return assembly.FindOrCreateProperty(name);
    }

    public Field FindField(FieldName name)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(name.TypeName.AssemblyName);
      if (assembly == null)
      {
        assembly = new Assembly(name.TypeName.AssemblyName);
        _assemblyRepository.SaveAssembly(assembly);
      }
      return assembly.FindOrCreateField(name);
    }
  }
}
