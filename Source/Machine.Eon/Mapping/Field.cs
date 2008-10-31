using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Field : Member, IField
  {
    private readonly FieldKey _key;
    private Type _fieldType;

    public FieldKey Key
    {
      get { return _key; }
    }

    public Type FieldType
    {
      get { return _fieldType; }
      set { _fieldType = value; }
    }

    public Field(Type type, FieldKey key)
      : base(type, key)
    {
      _key = key;
    }
  }
}
