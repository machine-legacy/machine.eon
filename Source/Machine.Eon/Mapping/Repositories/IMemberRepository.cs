using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping.Repositories
{
  public interface IMemberRepository
  {
    Method FindMethod(MethodName name);
    Property FindProperty(PropertyName name);
  }
}