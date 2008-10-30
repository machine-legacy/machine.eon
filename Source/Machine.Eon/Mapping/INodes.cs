using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public interface INode
  {
  }
  public interface IAssembly : INode, INodeNamed<AssemblyName>
  {
  }
  public interface INamespace : INode, INodeNamed<NamespaceName>
  {
  }
  public interface IType : INode, INodeNamed<TypeName>
  {
  }
  public interface IMember : INode
  {
  }
  public interface IProperty : IMember, INodeNamed<PropertyName>
  {
    bool IsReadOnly { get; }
    bool IsWriteOnly { get; }
    bool IsReadWrite { get; }
  }
  public interface IMethod : IMember, INodeNamed<MethodName>
  {
    bool IsGetter { get; }
    bool IsSetter { get; }
  }
  public interface IField : IMember, INodeNamed<FieldName>
  {
  }
}
