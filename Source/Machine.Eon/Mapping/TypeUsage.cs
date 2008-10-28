using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class TypeUsage : UsageByName<TypeName>
  {
    public TypeUsage(TypeName name)
      : base(name)
    {
    }
  }
  public class MethodUsage : UsageByName<MethodName>
  {
    public MethodUsage(MethodName name)
      : base(name)
    {
    }
  }
  public class PropertyUsage : UsageByName<PropertyName>
  {
    public PropertyUsage(PropertyName name)
      : base(name)
    {
    }
  }
}