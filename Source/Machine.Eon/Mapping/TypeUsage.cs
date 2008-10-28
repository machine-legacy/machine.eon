using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class TypeUsage : Usage
  {
    private readonly TypeName _typeName;

    public TypeName TypeName
    {
      get { return _typeName; }
    }

    public TypeUsage(TypeName typeName)
    {
      _typeName = typeName;
    }

    public override string ToString()
    {
      return "Usage<" + _typeName + ">";
    }
  }
}