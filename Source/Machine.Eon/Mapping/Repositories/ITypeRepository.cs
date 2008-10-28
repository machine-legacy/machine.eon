using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping.Repositories
{
  public interface ITypeRepository
  {
    Type FindType(TypeName name);
  }
}