using System;
using System.Collections.Generic;

using Machine.Eon.Mapping;

namespace Machine.Eon.Querying
{
  public abstract class BooleanCondition : Condition
  {
    protected Condition[] _conditions;

    protected BooleanCondition(params Condition[] conditions)
    {
      _conditions = conditions;
    }
  }
  public class AndCondition : BooleanCondition
  {
    public AndCondition(params Condition[] conditions)
      : base(conditions)
    {
    }

    public override bool Matches(Node node)
    {
      foreach (Condition c in _conditions)
      {
        if (!c.Matches(node))
        {
          return false;
        }
      }
      return true;
    }
  }
  public class OrCondition : BooleanCondition
  {
    public OrCondition(params Condition[] conditions)
      : base(conditions)
    {
    }

    public override bool Matches(Node node)
    {
      foreach (Condition c in _conditions)
      {
        if (c.Matches(node))
        {
          return true;
        }
      }
      return false;
    }
  }
}