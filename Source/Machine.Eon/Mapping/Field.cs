using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Field : Member, IField
  {
    private readonly FieldName _name;

    public FieldName Name
    {
      get { return _name; }
    }

    public Field(Type type, FieldName name)
      : base(type, name)
    {
      _name = name;
    }
  }
}
