using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine.Eon.Mapping
{
  [Flags]
  public enum TypeFlags
  {
    None = 0,
    Pending = 1,
    Interface = 2,
    Abstract = 4,
    Static = 8,
    Incomplete = 16
  }
  public class Type : Node, IType, ICanUseNodes, IHaveDirectUses, IHaveIndirectUses, ICanHaveAttributes
  {
    private readonly Namespace _namespace;
    private readonly TypeKey _key;
    private readonly List<Method> _methods = new List<Method>();
    private readonly List<Property> _properties = new List<Property>();
    private readonly List<Field> _fields = new List<Field>();
    private readonly List<Event> _events = new List<Event>();
    private readonly List<Type> _interfaces = new List<Type>();
    private readonly List<Type> _attributes = new List<Type>();
    private readonly UsageSet _usages = new UsageSet();
    private TypeFlags _flags = TypeFlags.Pending;
    private Type _baseType;

    public TypeKey Key
    {
      get { return _key; }
    }

    public Namespace Namespace
    {
      get { return _namespace; }
    }

    public TypeFlags TypeFlags
    {
      set { _flags = value; }
    }

    public bool IsPending
    {
      get { return (_flags & TypeFlags.Pending) == TypeFlags.Pending; }
    }

    public bool IsClass
    {
      get { return !IsInterface; }
    }

    public bool IsInterface
    {
      get { return HasFlag(TypeFlags.Interface); }
    }

    public bool IsAbstract
    {
      get { return HasFlag(TypeFlags.Abstract); }
    }

    public bool IsIncomplete
    {
      get { return HasFlag(TypeFlags.Incomplete); }
    }

    private bool HasFlag(TypeFlags flag)
    {
      EnsureTypeIsNotPending();
      return (_flags & flag) == flag;
    }

    public Type BaseType
    {
      get
      {
        EnsureTypeIsNotPending();
        return _baseType;
      }
      set { _baseType = value; }
    }

    public bool IsInDependentAssembly
    {
      get { return _namespace.Assembly.IsDependency; }
    }

    public Type(Namespace ns, TypeKey key)
      : this(ns, key, null)
    {
    }

    private Type(Namespace ns, TypeKey key, Type baseType)
    {
      _namespace = ns;
      _key = key;
      _baseType = baseType;
    }

    public IEnumerable<Type> Attributes
    {
      get { EnsureTypeIsNotPending(); return _attributes; }
    }

    public IEnumerable<Type> Interfaces
    {
      get { EnsureTypeIsNotPending(); return _interfaces; }
    }

    public IEnumerable<Field> Fields
    {
      get { EnsureTypeIsNotPending(); return _fields; }
    }

    public IEnumerable<Property> Properties
    {
      get { EnsureTypeIsNotPending(); return _properties; }
    }

    public IEnumerable<Event> Events
    {
      get { EnsureTypeIsNotPending(); return _events; }
    }

    public IEnumerable<Method> Methods
    {
      get { EnsureTypeIsNotPending(); return _methods; }
    }

    public IEnumerable<Method> Constructors
    {
      get
      {
        foreach (Method method in this.Methods)
        {
          if (method.IsConstructor) yield return method;
        }
      }
    }

    public IEnumerable<Method> MethodsNotPartOfProperties
    {
      get
      {
        EnsureTypeIsNotPending();
        List<Method> methods = new List<Method>(_methods);
        foreach (Property property in _properties)
        {
          methods.Remove(property.Getter);
          methods.Remove(property.Setter);
        }
        return methods;
      }
    }

    public IEnumerable<Member> Members
    {
      get { EnsureTypeIsNotPending(); return MembersPendingOrNot; }
    }

    public IEnumerable<Method> PendingMethods
    {
      get { return from method in _methods where method.IsPending select method; }
    }

    private IEnumerable<Member> MembersPendingOrNot
    {
      get
      {
        foreach (Field field in _fields) yield return field;
        foreach (Property property in _properties) yield return property;
        foreach (Event anEvent in _events) yield return anEvent;
        foreach (Method method in _methods) yield return method;
      }
    }

    public IEnumerable<Member> this[string name]
    {
      get
      {
        foreach (Member member in this.Members)
        {
          if (member.MemberKey.Name.Equals(name))
          {
            yield return member;
          }
        }
      }
    }

    public Method AddMethod(MethodKey key)
    {
      if (!key.TypeKey.Equals(_key)) throw new ArgumentException("name");
      Method newMember = new Method(this, key);
      _methods.Add(newMember);
      return newMember;
    }

    private T FindMember<T, K>(K key) where T : Member where K : MemberKey
    {
      foreach (Member member in this.MembersPendingOrNot)
      {
        IKeyedNode<K> node = member as IKeyedNode<K>;
        if (node != null && node.Key.Equals(key))
        {
          return (T)node;
        }
      }
      return default(T);
    }

    public Method FindMethod(MethodKey key)
    {
      return FindMember<Method, MethodKey>(key);
    }

    public Property AddProperty(PropertyKey key)
    {
      if (!key.TypeKey.Equals(_key)) throw new ArgumentException("name");
      Property newMember = new Property(this, key);
      _properties.Add(newMember);
      return newMember;
    }

    public Property FindProperty(PropertyKey key)
    {
      return FindMember<Property, PropertyKey>(key);
    }

    public Field AddField(FieldKey key)
    {
      if (!key.TypeKey.Equals(_key)) throw new ArgumentException("name");
      Field newMember = new Field(this, key);
      _fields.Add(newMember);
      return newMember;
    }

    public Field FindField(FieldKey key)
    {
      return FindMember<Field, FieldKey>(key);
    }

    public Event AddEvent(EventKey key)
    {
      if (!key.TypeKey.Equals(_key)) throw new ArgumentException("name");
      Event newMember = new Event(this, key);
      _events.Add(newMember);
      return newMember;
    }

    public Event FindEvent(EventKey key)
    {
      return FindMember<Event, EventKey>(key);
    }

    public override Usage CreateUsage()
    {
      return new TypeUsage(this);
    }

    public void Use(Node node)
    {
      _usages.Add(node);
    }

    public UsageSet DirectlyUses
    {
      get
      {
        EnsureTypeIsNotPending();
        return UsageSet.Union(_usages, DirectUsesOfMembers).RemoveReferencesToType(this);
      }
    }

    private UsageSet DirectUsesOfMembers
    {
      get
      {
        EnsureTypeIsNotPending();
        List<UsageSet> usages = new List<UsageSet>();
        foreach (Member member in this.Members)
        {
          usages.Add(member.DirectlyUses);
        }
        return UsageSet.Union(usages.ToArray());
      }
    }

    public UsageSet DirectUsesAndAttributesAndInterfaces
    {
      get
      {
        EnsureTypeIsNotPending();
        UsageSet set = UsageSet.Union(_usages, UsageSet.MakeFrom(_attributes), UsageSet.MakeFrom(_interfaces), UsageSet.MakeFrom(_methods));
        set.Add(this);
        if (this.BaseType != null)
        {
          set.Add(this.BaseType);
        }
        return set;
      }
    }

    public IndirectUses IndirectlyUses
    {
      get
      {
        EnsureTypeIsNotPending();
        return DirectUsesAndAttributesAndInterfaces.CreateIndirectUses();
      }
    }

    public void AddInterface(Type type)
    {
      if (!_interfaces.Contains(type))
      {
        _interfaces.Add(type);
      }
    }

    public void AddAttribute(Type type)
    {
      if (!_attributes.Contains(type))
      {
        _attributes.Add(type);
      }
    }

    public override string ToString()
    {
      return _key.ToString();
    }

    public bool IsA(Type type)
    {
      return IsA(type.Key);
    }

    public bool IsA(TypeKey key)
    {
      if (this.Key.Equals(key)) return true;
      foreach (Type interfaceType in _interfaces)
      {
        if (interfaceType.IsA(key)) return true;
      }
      if (_baseType == null)
      {
        return false;
      }
      return _baseType.IsA(key);
    }

    protected virtual void EnsureTypeIsNotPending()
    {
      if (this.IsPending)
      {
        throw new NodeIsPendingException(this.Key.ToString());
      }
    }
  }
}