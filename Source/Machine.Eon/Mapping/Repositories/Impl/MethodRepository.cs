using System;

namespace Machine.Eon.Mapping.Repositories.Impl
{
  public class MethodRepository : IMethodRepository
  {
    private readonly AssemblyRepository _assemblyRepository = new AssemblyRepository();

    #region IMethodRepository Members
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
    #endregion
  }
}
