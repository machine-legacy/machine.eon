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

    private Assembly FindAssembly(MemberKey key)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(key.TypeKey.AssemblyKey);
      if (assembly == null)
      {
        assembly = new Assembly(key.TypeKey.AssemblyKey);
        _assemblyRepository.SaveAssembly(assembly);
      }
      return assembly;
    }

    private Type FindType(MemberKey key)
    {
      return FindAssembly(key).FindOrCreateType(key.TypeKey);
    }

    public Method FindMethod(MethodKey key)
    {
      Type type = FindType(key);
      Method member = type.FindMethod(key);
      if (member == null)
      {
        member = type.AddMethod(key);
      }
      return member;
    }

    public Property FindProperty(PropertyKey key)
    {
      Type type = FindType(key);
      Property member = type.FindProperty(key);
      if (member == null)
      {
        member = type.AddProperty(key);
      }
      return member;
    }

    public Field FindField(FieldKey key)
    {
      Type type = FindType(key);
      Field member = type.FindField(key);
      if (member == null)
      {
        member = type.AddField(key);
      }
      return member;
    }

    public Event FindEvent(EventKey key)
    {
      Type type = FindType(key);
      Event member = type.FindEvent(key);
      if (member == null)
      {
        member = type.AddEvent(key);
      }
      return member;
    }
  }
}
