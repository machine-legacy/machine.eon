using System;

namespace Machine.Eon.Mapping.Repositories.Impl
{
  public class TypeRepository : ITypeRepository
  {
    private readonly AssemblyRepository _assemblyRepository = new AssemblyRepository();

    public Type FindType(AssemblyName assemblyName, TypeName name)
    {
      Assembly assembly = _assemblyRepository.FindAssembly(assemblyName);
      if (assembly == null)
      {
        assembly = new Assembly(assemblyName);
        _assemblyRepository.SaveAssembly(assembly);
      }
      return assembly.FindOrCreateType(name);
    }

    public Type FindType(TypeName name)
    {
      foreach (Assembly assembly in _assemblyRepository.FindAll())
      {
        Type type = assembly.FindType(name);
        if (type != null)
        {
          return type;
        }
      }
      throw new InvalidOperationException();
    }
  }
}