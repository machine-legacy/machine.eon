using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping.Repositories
{
  public interface IMemberRepository
  {
    Method FindMethod(AssemblyName assemblyName, MethodName name);
    Property FindProperty(AssemblyName assemblyName, PropertyName name);
  }
}