using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Method : Member, IMethod, ICanUse
  {
    private readonly MethodName _name;
    private readonly UsageSet _usages = new UsageSet();

    public MethodName Name
    {
      get { return _name; }
    }

    public MethodName MethodName
    {
      get { return _name; }
    }

    public Method(MethodName name)
      : base(name)
    {
      _name = name;
    }

    public override NodeName NodeName
    {
      get { return _name; }
    }

    public override Usage Usage()
    {
      return new MethodUsage(this);
    }

    public void Use(Node node)
    {
      _usages.Add(node);
    }
  }
}