using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping.Repositories
{
  public interface IMemberRepository
  {
    Method FindMethod(MethodKey key);
    Property FindProperty(PropertyKey key);
    Field FindField(FieldKey key);
  }
}