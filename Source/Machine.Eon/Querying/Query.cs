using System;
using System.Collections.Generic;

namespace Machine.Eon.Querying
{
  public class Query
  {
    private Condition _assemblyCondition = new AllCondition();
    private Condition _namespaceCondition = new AllCondition();
    private Condition _typeCondition = new AllCondition();
    private Condition _memberCondition = new AllCondition();

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
  }
}
