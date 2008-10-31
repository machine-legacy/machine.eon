using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public interface INode
  {
  }
  public interface IAssembly : INode, IKeyedNode<AssemblyKey>
  {
  }
  public interface INamespace : INode, IKeyedNode<NamespaceKey>
  {
  }
  public interface IType : INode, IKeyedNode<TypeKey>
  {
  }
  public interface IMember : INode
  {
  }
  public interface IProperty : IMember, IKeyedNode<PropertyKey>
  {
    bool IsReadOnly { get; }
    bool IsWriteOnly { get; }
    bool IsReadWrite { get; }
  }
  public interface IMethod : IMember, IKeyedNode<MethodKey>
  {
    bool IsGetter { get; }
    bool IsSetter { get; }
  }
  public interface IField : IMember, IKeyedNode<FieldKey>
  {
  }
  public interface IEvent : IMember, IKeyedNode<EventKey>
  {
  }
}
