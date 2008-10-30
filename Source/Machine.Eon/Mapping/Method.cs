using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Method : Member, IMethod, ICanUse, IHaveUses
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

    public bool IsSetter
    {
      get { return _name.Name.StartsWith("set_"); }
    }

    public bool IsGetter
    {
      get { return _name.Name.StartsWith("get_"); }
    }

    public Method(Type type, MethodName name)
      : base(type, name)
    {
      _name = name;
    }

    public override NodeName NodeName
    {
      get { return _name; }
    }

    public override Usage CreateUsage()
    {
      return new MethodUsage(this);
    }

    public void Use(Node node)
    {
      _usages.Add(node);
    }

    public UsageSet DirectlyUses
    {
      get { return _usages; }
    }

    public UsageSet IndirectlyUses
    {
      get { return _usages; }
    }
  }
}