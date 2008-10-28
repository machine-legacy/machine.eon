using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Property : Member
  {
    private readonly PropertyName _name;
    private Method _getter;
    private Method _setter;

    public PropertyName Name
    {
      get { return _name; }
    }

    public Method Getter
    {
      get { return _getter; }
      set { _getter = value; }
    }

    public Method Setter
    {
      get { return _setter; }
      set { _setter = value; }
    }

    public Property(PropertyName name)
      : base(name)
    {
      _name = name;
    }

    public override Usage Usage()
    {
      throw new InvalidOperationException();
    }
  }
}
