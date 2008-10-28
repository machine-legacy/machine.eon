using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Method : Member, ICanUse
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

    public override Usage Usage()
    {
      return new MethodUsage(this, _name);
    }

    public void Use(Node node)
    {
      _usages.Add(node);
    }
  }
}