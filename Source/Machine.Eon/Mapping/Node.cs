using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public abstract class Node : INode
  {
    public virtual Usage CreateUsage()
    {
      throw new InvalidOperationException();
    }
  }
  public interface INodeNamed<TName> where TName : NodeName
  {
    TName Name
    {
      get;
    }
  }
  public abstract class NodeName
  {
  }
}
