using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public interface INode
  {
    NodeName NodeName { get; }
  }
  public interface IAssembly : INode
  {
    AssemblyName AssemblyName { get; }
  }
  public interface INamespace : INode
  {
    NamespaceName NamespaceName { get; }
  }
  public interface IType : INode
  {
    TypeName TypeName { get; }
  }
  public interface IMember : INode
  {
    MemberName MemberName { get; }
  }
  public interface IProperty : IMember
  {
    PropertyName PropertyName { get; }
  }
  public interface IMethod : IMember
  {
    MethodName MethodName { get; }
  }
}
