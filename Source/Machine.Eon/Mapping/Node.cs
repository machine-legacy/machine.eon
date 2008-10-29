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
    public abstract Usage CreateUsage();
  }
  public abstract class NodeName
  {
  }
}
