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
    None = 0,
    Pending = 1,
    Constructor = 2,
    Abstract = 4,
    Virtual = 8,
    Static = 16,
    Incomplete = 32
  }
  public class Method : Member, IMethod, IHaveIndirectUses
  {
    private readonly MethodKey _key;
    private List<Parameter> _parameters;
    private Type _returnType;
    private MethodFlags _flags = MethodFlags.Pending;

    public IEnumerable<Parameter> Parameters
    {
      get
      {
        EnsureMemberIsNotPending();
        return _parameters;
      }
    }

    public MethodKey Key
    {
      get { return _key; }
    }

    public bool IsSetter
    {
      get { return _key.Name.StartsWith("set_"); }
    }

    public bool IsGetter
    {
      get { return _key.Name.StartsWith("get_"); }
    }

    public bool IsEventAdder
    {
      get { return _key.Name.StartsWith("add_"); }
    }

    public bool IsEventRemover
    {
      get { return _key.Name.StartsWith("remove_"); }
    }

    public Type ReturnType
    {
      get
      {
        EnsureMemberIsNotPending();
        return _returnType;
      }
      set { _returnType = value; }
    }

    public bool IsPending
    {
      get { return (_flags & MethodFlags.Pending) == MethodFlags.Pending; }
    }

    public bool IsConstructor
    {
      get { return HasFlag(MethodFlags.Constructor); }
    }

    public bool IsAbstract
    {
      get { return HasFlag(MethodFlags.Abstract); }
    }

    public bool IsVirtual
    {
      get { return HasFlag(MethodFlags.Virtual); }
    }

    public bool IsStatic
    {
      get { return HasFlag(MethodFlags.Static); }
    }

    public MethodFlags MethodFlags
    {
      set { _flags = value;}
    }

    private bool HasFlag(MethodFlags flag)
    {
      EnsureMemberIsNotPending();
      return (_flags & flag) == flag;
    }

    public Method(Type type, MethodKey key)
      : base(type, key)
    {
      _key = key;
    }

    public override Usage CreateUsage()
    {
      return new MethodUsage(this);
    }

    public IndirectUses IndirectlyUses
    {
      get
      {
        EnsureMemberIsNotPending();
        return this.DirectlyUses.CreateIndirectUses();
      }
    }

    public void SetParameters(List<Parameter> parameters)
    {
      _parameters = parameters;
    }

    protected override void EnsureMemberIsNotPending()
    {
      if (this.IsPending)
      {
        throw new NodeIsPendingException(this.Key.ToString());
      }
    }
  }
}