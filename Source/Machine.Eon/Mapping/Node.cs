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
  public interface IKeyedNode<TKey> where TKey : NodeKey
  {
    TKey Key
    {
      get;
    }
  }
  public abstract class NodeKey
  {
  }
}
