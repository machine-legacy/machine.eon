using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class UsageSet
  {
    private readonly Dictionary<TypeName, TypeUsage> _usages = new Dictionary<TypeName, TypeUsage>();

    public ICollection<TypeUsage> Usages
    {
      get { return _usages.Values; }
    }

    public void Add(TypeName typeName)
    {
      if (_usages.ContainsKey(typeName))
      {
        return;
      }
      _usages[typeName] = new TypeUsage(typeName);
    }
  }
}