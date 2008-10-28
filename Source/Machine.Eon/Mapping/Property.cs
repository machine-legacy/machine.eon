using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Property
  {
    private readonly PropertyName _name;
    private readonly UsageSet _usages = new UsageSet();

    public PropertyName Name
    {
      get { return _name; }
    }

    public Property(PropertyName name)
    {
      _name = name;
    }

    public void UseType(TypeName name)
    {
      _usages.Add(name);
    }
  }
}
