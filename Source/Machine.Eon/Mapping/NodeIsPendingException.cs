using System;
using System.Runtime.Serialization;

namespace Machine.Eon.Mapping
{
  [Serializable]
  public class NodeIsPendingException : Exception
  {
    public NodeIsPendingException()
    {
    }

    public NodeIsPendingException(string message) : base(message)
    {
    }

    public NodeIsPendingException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected NodeIsPendingException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}