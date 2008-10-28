using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping.Repositories.Impl
{
  public class PropertyRepository : IPropertyRepository
  {
    private readonly AssemblyRepository _assemblyRepository = new AssemblyRepository();

    #region IPropertyRepository Members
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
    #endregion
  }
}
