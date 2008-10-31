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

    public Type FindType(TypeKey key)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(key.AssemblyKey);
      if (assembly == null)
      {
        assembly = new Assembly(key.AssemblyKey);
        _assemblyRepository.SaveAssembly(assembly);
      }
      return assembly.FindOrCreateType(key);
    }
  }
}