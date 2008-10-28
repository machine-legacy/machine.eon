using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public abstract class Node
  {
    public abstract NodeName NodeName
    {
      get;
    }
    public abstract Usage Usage();
  }
  public abstract class NodeName
  {
  }
}
