using System;
using System.Collections.Generic;

using Machine.Eon.Mapping;

namespace Machine.Eon.Querying
{
  public class NotCondition : Condition
  {
    private readonly Condition _condition;

    public NotCondition(Condition condition)
    {
      _condition = condition;
    }

    public override bool Matches(Node node)
    {
      return !_condition.Matches(node);
    }
  }
}