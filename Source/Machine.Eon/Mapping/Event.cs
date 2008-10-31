using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Event : Member, IEvent
  {
    private readonly EventKey _key;

    public EventKey Key
    {
      get { return _key; }
    }

    public Event(Type type, EventKey key)
      : base(type, key)
    {
      _key = key;
    }
  }
}
