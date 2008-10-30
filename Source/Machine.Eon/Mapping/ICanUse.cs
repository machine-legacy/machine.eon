using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public interface ICanUse
  {
    void Use(Node node);
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
