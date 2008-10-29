using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public abstract class Node : INode
  {
    public abstract NodeName NodeName
    {
      get;
    }
    public virtual Usage CreateUsage()
    {
      throw new InvalidOperationException();
    }
  }
  public abstract class NodeName
  {
  }
}
