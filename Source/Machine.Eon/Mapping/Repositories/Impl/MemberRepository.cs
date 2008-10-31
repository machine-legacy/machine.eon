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

    public Method FindMethod(MethodKey key)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(key.TypeKey.AssemblyKey);
      if (assembly == null)
      {
        assembly = new Assembly(key.TypeKey.AssemblyKey);
        _assemblyRepository.SaveAssembly(assembly);
      }
      return assembly.FindOrCreateMethod(key);
    }

    public Property FindProperty(PropertyKey key)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(key.TypeKey.AssemblyKey);
      if (assembly == null)
      {
        assembly = new Assembly(key.TypeKey.AssemblyKey);
        _assemblyRepository.SaveAssembly(assembly);
      }
      return assembly.FindOrCreateProperty(key);
    }

    public Field FindField(FieldKey key)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(key.TypeKey.AssemblyKey);
      if (assembly == null)
      {
        assembly = new Assembly(key.TypeKey.AssemblyKey);
        _assemblyRepository.SaveAssembly(assembly);
      }
      return assembly.FindOrCreateField(key);
    }

    public Event FindEvent(EventKey key)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(key.TypeKey.AssemblyKey);
      if (assembly == null)
      {
        assembly = new Assembly(key.TypeKey.AssemblyKey);
        _assemblyRepository.SaveAssembly(assembly);
      }
      return assembly.FindOrCreateEvent(key);
    }
  }
}
