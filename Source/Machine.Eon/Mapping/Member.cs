using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public abstract class Member : Node, IMember
  {
    private readonly Type _type;
    private readonly MemberName _name;

    public Type Type
    {
      get { return _type; }
    }

    public MemberName MemberName
    {
      get { return _name; }
    }

    protected Member(Type type, MemberName name)
    {
      _type = type;
      _name = name;
    }

    public override string ToString()
    {
      return _name.ToString();
    }
  }
}
