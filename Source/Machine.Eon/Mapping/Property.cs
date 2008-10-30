using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Property : Member, IProperty
  {
    private readonly PropertyName _name;
    private Method _getter;
    private Method _setter;

    public PropertyName Name
    {
      get { return _name; }
    }

    public bool IsWriteOnly
    {
      get { return _setter != null && _getter == null;}
    }

    public bool IsReadWrite
    {
      get { return _setter != null && _getter != null;}
    }

    public bool IsReadOnly
    {
      get { return _setter == null && _getter != null;}
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

    public Property(Type type, PropertyName name)
      : base(type, name)
    {
      _name = name;
    }

    public override Usage CreateUsage()
    {
      return new PropertyUsage(this);
    }
  }
}
