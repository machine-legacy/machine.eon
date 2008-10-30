using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Parameter
  {
    private readonly Type _type;

    public Type ParameterType
    {
      get { return _type; }
    }

    public Parameter(Type type)
    {
      _type = type;
    }
  }
  [Flags]
  public enum MethodFlags
  {
    None,
    Constructor,
    Abstract,
    Virtual
  }

  public class Method : Member, IMethod, IHaveIndirectUses
  {
    private readonly MethodName _name;
    private List<Parameter> _parameters;
    private Type _returnType;
    private MethodFlags _flags;

    public IEnumerable<Parameter> Parameters
    {
      get { return _parameters; }
    }

    public MethodName Name
    {
      get { return _name; }
    }

    public bool IsSetter
    {
      get { return _name.Name.StartsWith("set_"); }
    }

    public bool IsGetter
    {
      get { return _name.Name.StartsWith("get_"); }
    }

    public Type ReturnType
    {
      get { return _returnType; }
      set { _returnType = value; }
    }

    public bool IsConstructor
    {
      get { return (_flags & MethodFlags.Constructor) == MethodFlags.Constructor; }
    }

    public bool IsAbstract
    {
      get { return (_flags & MethodFlags.Abstract) == MethodFlags.Abstract; }
    }

    public bool IsVirtual
    {
      get { return (_flags & MethodFlags.Virtual) == MethodFlags.Virtual; }
    }

    public MethodFlags MethodFlags
    {
      set { _flags = value;}
    }

    public Method(Type type, MethodName name)
      : base(type, name)
    {
      _name = name;
    }

    public override Usage CreateUsage()
    {
      return new MethodUsage(this);
    }

    public IndirectUses IndirectlyUses
    {
      get { return this.DirectlyUses.CreateIndirectUses(); }
    }

    public void SetParameters(List<Parameter> parameters)
    {
      _parameters = parameters;
    }
  }
}