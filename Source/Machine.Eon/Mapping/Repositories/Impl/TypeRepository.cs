using System;

namespace Machine.Eon.Mapping.Repositories.Impl
{
  public class TypeRepository : ITypeRepository
  {
    private readonly AssemblyRepository _assemblyRepository = new AssemblyRepository();

    #region ITypeRepository Members
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
    #endregion
  }
}