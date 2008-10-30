using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public interface ICanUse
  {
    void Use(Node node);
  }
  public interface ICanHaveAttributes
  {
    void AddAttribute(Type type);
  }
  public interface IHaveUses
  {
    UsageSet IndirectlyUses
    {
      get;
    }
    UsageSet DirectlyUses
    {
      get;
    }
  }
}
