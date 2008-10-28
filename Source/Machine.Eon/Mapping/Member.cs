using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public abstract class Member : Node
  {
    private readonly MemberName _name;

    public MemberName MemberName
    {
      get { return _name; }
    }

    protected Member(MemberName name)
    {
      _name = name;
    }
  }
}
