using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public interface ICanUseNodes
  {
    void Use(Node node);
  }
  public interface ICanHaveAttributes
  {
    void AddAttribute(Type type);
  }
  public interface IHaveDirectUses
  {
    UsageSet DirectlyUses
    {
      get;
    }
  }
  public interface IHaveIndirectUses
  {
    IndirectUses IndirectlyUses
    {
      get;
    }
  }
}
