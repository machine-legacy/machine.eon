using System;

namespace Machine.Eon.Mapping.Repositories.Impl
{
  public class TypeRepository : ITypeRepository
  {
    private readonly IAssemblyRepository _assemblyRepository;

    public TypeRepository(IAssemblyRepository assemblyRepository)
    {
      _assemblyRepository = assemblyRepository;
    }

    private Assembly FindAssembly(TypeKey key)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(key.AssemblyKey);
      if (assembly == null)
      {
        assembly = new Assembly(key.AssemblyKey);
        _assemblyRepository.SaveAssembly(assembly);
      }
      return assembly;
    }

    public Type FindType(TypeKey key)
    {
      Assembly assembly = FindAssembly(key);
      Type type = assembly.FindType(key);
      if (type == null)
      {
        type = assembly.AddType(key);
      }
      return type;
    }
  }
}