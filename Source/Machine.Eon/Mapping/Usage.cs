using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public abstract class Usage
  {
  }

  public abstract class UsageByName<TName> : Usage where TName : class
  {
    private readonly TName _name;

    protected UsageByName(TName name)
    {
      _name = name;
    }

    public TName Name
    {
      get { return _name; }
    }

    public override bool Equals(object obj)
    {
      if (obj is UsageByName<TName>)
      {
        return ((UsageByName<TName>)obj).Name.Equals(this.Name);
      }
      return false;
    }
    
    public override Int32 GetHashCode()
    {
      return this.Name.GetHashCode();
    }

    public override string ToString()
    {
      return "Usage<" + this.Name + ">";
    }
  }
}
