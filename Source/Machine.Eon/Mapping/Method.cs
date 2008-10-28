using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Method : Member
  {
    private readonly MethodName _name;
    private readonly UsageSet _usages = new UsageSet();

    public MethodName Name
    {
      get { return _name; }
    }

    public Method(MethodName name)
      : base(name)
    {
      _name = name;
    }

    public void UseType(TypeName name)
    {
      _usages.Add(name);
    }
  }
}