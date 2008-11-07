using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping.Inspection
{
  public class VisitationOptions
  {
    private readonly List<TypeKey> _types;
    private readonly List<MethodKey> _methods;
    private readonly bool _visitMethods;
    private readonly bool _visitMembers;

    public bool VisitMethods
    {
      get { return _visitMethods; }
    }

    public bool VisitMembers
    {
      get { return _visitMembers; }
    }

    public bool ShouldVisit(TypeKey typeKey)
    {
      if (_types == null)
      {
        return true;
      }
      return _types.Contains(typeKey);
    }

    public bool ShouldVisit(MethodKey methodKey)
    {
      if (_methods == null)
      {
        return true;
      }
      return _methods.Contains(methodKey);
    }

    public VisitationOptions(bool visitMethods, List<TypeKey> types, List<MethodKey> methods)
    {
      _visitMethods = visitMethods;
      _visitMembers = visitMethods;
      _types = types;
      _methods = methods;
    }

    public VisitationOptions(bool visitMethods)
      : this(visitMethods, null, null)
    {
    }
  }
}
