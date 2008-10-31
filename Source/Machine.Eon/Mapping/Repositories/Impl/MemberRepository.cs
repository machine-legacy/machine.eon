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
      Type type = assembly.FindOrCreateType(key.TypeKey);
      Method member = type.FindMethod(key);
      if (member == null)
      {
        member = type.AddMethod(key);
      }
      return member;
    }

    public Property FindProperty(PropertyKey key)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(key.TypeKey.AssemblyKey);
      if (assembly == null)
      {
        assembly = new Assembly(key.TypeKey.AssemblyKey);
        _assemblyRepository.SaveAssembly(assembly);
      }
      Type type = assembly.FindOrCreateType(key.TypeKey);
      Property member = type.FindProperty(key);
      if (member == null)
      {
        member = type.AddProperty(key);
      }
      return member;
    }

    public Field FindField(FieldKey key)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(key.TypeKey.AssemblyKey);
      if (assembly == null)
      {
        assembly = new Assembly(key.TypeKey.AssemblyKey);
        _assemblyRepository.SaveAssembly(assembly);
      }
      Type type = assembly.FindOrCreateType(key.TypeKey);
      Field member = type.FindField(key);
      if (member == null)
      {
        member = type.AddField(key);
      }
      return member;
    }

    public Event FindEvent(EventKey key)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(key.TypeKey.AssemblyKey);
      if (assembly == null)
      {
        assembly = new Assembly(key.TypeKey.AssemblyKey);
        _assemblyRepository.SaveAssembly(assembly);
      }
      Type type = assembly.FindOrCreateType(key.TypeKey);
      Event member = type.FindEvent(key);
      if (member == null)
      {
        member = type.AddEvent(key);
      }
      return member;
    }
  }
}
