using System;
using System.Collections.Generic;

namespace Machine.Eon.Querying
{
  public class Query
  {
    private Condition _assemblyCondition = AllCondition.All;
    private Condition _namespaceCondition = AllCondition.All;
    private Condition _typeCondition = AllCondition.All;
    private Condition _memberCondition = AllCondition.All;

    public Condition Assemblies
    {
      get { return _assemblyCondition; }
      set { _assemblyCondition = value; }
    }

    public Condition Namespaces
    {
      get { return _namespaceCondition; }
      set { _namespaceCondition = value; }
    }

    public Condition Types
    {
      get { return _typeCondition; }
      set { _typeCondition = value; }
    }

    public Condition Members
    {
      get { return _memberCondition; }
      set { _memberCondition = value; }
    }

    public bool IsValid()
    {
      if (_assemblyCondition == null)
      {
        return false;
      }
      return true;
    }
  }
}
