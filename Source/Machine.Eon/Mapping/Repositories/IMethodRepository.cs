using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping.Repositories
{
  public interface IMethodRepository
  {
    Method FindMethod(AssemblyName assemblyName, MethodName name);
  }
}