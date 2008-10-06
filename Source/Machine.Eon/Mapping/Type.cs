using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Type
  {
    private readonly TypeName _name;
    private readonly List<Method> _methods = new List<Method>();
    private readonly UsageSet _usages = new UsageSet();

    public TypeName Name
    {
      get { return _name; }
    }

    public IEnumerable<Method> Methods
    {
      get { return _methods; }
    }

    public Type(TypeName name)
    {
      _name = name;
    }

    public Method FindOrCreateMethod(MethodName name)
    {
      if (!name.TypeName.Equals(_name)) throw new ArgumentException("name");
      foreach (Method method in _methods)
      {
        if (method.Name.Equals(name))
        {
          return method;
        }
      }
      Method newMethod = new Method(name);
      _methods.Add(newMethod);
      return newMethod;
    }

    public void UseType(TypeName name)
    {
      _usages.Add(name);
    }
  }
}