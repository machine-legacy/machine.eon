using System;

namespace Machine.Eon.Mapping.Repositories.Impl
{
  public class MemberRepository : IMemberRepository
  {
    private readonly AssemblyRepository _assemblyRepository = new AssemblyRepository();

    public Method FindMethod(AssemblyName assemblyName, MethodName name)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(assemblyName);
      if (assembly == null)
      {
        assembly = new Assembly(assemblyName);
        _assemblyRepository.SaveAssembly(assembly);
      }
      return assembly.FindOrCreateMethod(name);
    }

    public Property FindProperty(AssemblyName assemblyName, PropertyName name)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(assemblyName);
      if (assembly == null)
      {
        assembly = new Assembly(assemblyName);
        _assemblyRepository.SaveAssembly(assembly);
      }
      return assembly.FindOrCreateProperty(name);
    }
  }
}
