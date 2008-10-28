using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class UsageSet
  {
    private readonly List<Usage> _usages = new List<Usage>();

    public IEnumerable<Usage> Usages
    {
      get { return _usages; }
    }

    public void Add(Node node)
    {
      Add(node.Usage());
    }

    public void Add(Usage usage)
    {
      if (_usages.Contains(usage))
      {
        return;
      }
      _usages.Add(usage);
    }
  }
}