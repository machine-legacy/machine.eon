using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Field : Member, IField
  {
    private readonly FieldName _name;
    private Type _fieldType;

    public FieldName Name
    {
      get { return _name; }
    }

    public Type FieldType
    {
      get { return _fieldType; }
      set { _fieldType = value; }
    }

    public Field(Type type, FieldName name)
      : base(type, name)
    {
      _name = name;
    }
  }
}
