using System;

namespace Machine.Eon.Mapping.Repositories
{
  public interface IPropertyRepository
  {
    Property FindProperty(AssemblyName assemblyName, PropertyName name);
  }
}