using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Property : Member, IProperty
  {
    private readonly PropertyKey _key;
    private Type _propertyType;
    private Method _getter;
    private Method _setter;

    public PropertyKey Key
    {
      get { return _key; }
    }

    public Type PropertyType
    {
      get { return _propertyType; }
      set { _propertyType = value; }
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

    public Property(Type type, PropertyKey key)
      : base(type, key)
    {
      _key = key;
    }

    public override Usage CreateUsage()
    {
      return new PropertyUsage(this);
    }
  }
}
