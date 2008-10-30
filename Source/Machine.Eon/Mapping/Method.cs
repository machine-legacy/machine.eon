using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Parameter
  {
    private readonly Type _type;

    public Type ParameterType
    {
      get { return _type; }
    }

    public Parameter(Type type)
    {
      _type = type;
    }
  }
  public class Method : Member, IMethod, ICanUseNodes, IHaveIndirectUses
  {
    private readonly MethodName _name;
    private readonly UsageSet _usages = new UsageSet();
    private List<Parameter> _parameters;
    private Type _returnType;

    public Type ReturnType
    {
      get { return _returnType; }
      set { _returnType = value; }
    }

    public IEnumerable<Parameter> Parameters
    {
      get { return _parameters; }
    }

    public MethodName Name
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

    public override Usage CreateUsage()
    {
      return new MethodUsage(this);
    }

    public void Use(Node node)
    {
      _usages.Add(node);
    }

    public override UsageSet DirectlyUses
    {
      get { return _usages; }
    }

    public IndirectUses IndirectlyUses
    {
      get { return _usages.CreateIndirectUses(); }
    }

    public void SetParameters(List<Parameter> parameters)
    {
      _parameters = parameters;
    }
  }
}