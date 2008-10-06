using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Method
  {
    private readonly MethodName _name;

    public MethodName Name
    {
      get { return _name; }
    }

    public Method(MethodName name)
    {
      _name = name;
    }
  }
}