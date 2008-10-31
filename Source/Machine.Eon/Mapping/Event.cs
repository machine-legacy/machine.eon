using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Event : Member, IEvent
  {
    private readonly EventKey _key;
    private Method _adder;
    private Method _remover;
    private Type _eventType;

    public EventKey Key
    {
      get { return _key; }
    }

    public Method Adder
    {
      get { return _adder; }
      set { _adder = value; }
    }

    public Method Remover
    {
      get { return _remover; }
      set { _remover = value; }
    }

    public Type EventType
    {
      get { return _eventType; }
      set { _eventType = value; }
    }

    public Event(Type type, EventKey key)
      : base(type, key)
    {
      _key = key;
    }
  }
}
