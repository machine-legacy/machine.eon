using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Field : Member, IField, ICanUseNodes
  {
    private readonly FieldName _name;
    private readonly UsageSet _usages = new UsageSet();
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

    public void Use(Node node)
    {
      _usages.Add(node);
    }

    public override UsageSet DirectlyUses
    {
      get { return _usages; }
    }
  }
}
