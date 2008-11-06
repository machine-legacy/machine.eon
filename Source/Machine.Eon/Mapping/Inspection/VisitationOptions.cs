using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping.Inspection
{
  public class VisitationOptions
  {
    private readonly List<TypeKey> _types;
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

    public VisitationOptions(bool visitMethods, List<TypeKey> types)
    {
      _visitMethods = visitMethods;
      _visitMembers = visitMethods;
      _types = types;
    }

    public VisitationOptions(bool visitMethods)
      : this(visitMethods, null)
    {
    }
  }
}
