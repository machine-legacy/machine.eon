using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public abstract class Member : Node, IMember, ICanUseNodes, ICanHaveAttributes, IHaveDirectUses
  {
    private readonly Type _type;
    private readonly MemberKey _key;
    private readonly List<Type> _attributes = new List<Type>();
    private readonly UsageSet _usages = new UsageSet();

    public Type Type
    {
      get { return _type; }
    }

    public virtual IEnumerable<Type> Attributes
    {
      get
      {
        EnsureMemberIsNotPending();
        return _attributes;
      }
    }

    protected Member(Type type, MemberKey key)
    {
      _type = type;
      _key = key;
    }

    public void AddAttribute(Type type)
    {
      if (!_attributes.Contains(type))
      {
        _attributes.Add(type);
      }
    }

    public override string ToString()
    {
      return _key.ToString();
    }

    public void Use(Node node)
    {
      _usages.Add(node);
    }

    public virtual UsageSet DirectlyUses
    {
      get
      {
        EnsureMemberIsNotPending();
        return _usages;
      }
    }

    protected virtual void EnsureMemberIsNotPending()
    {
    }
  }
}
