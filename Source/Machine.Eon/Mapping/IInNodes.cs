using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public interface IInAssembly
  {
    AssemblyName AssemblyName { get; }
  }
  public interface IInNamespace
  {
    NamespaceName NamespaceName { get; }
  }
  public interface IInType
  {
    TypeName TypeName { get; }
  }
  public interface IInMember
  {
    MemberName MemberName { get; }
  }
  public interface IInProperty : IInMember
  {
    PropertyName PropertyName { get; }
  }
  public interface IInMethod : IInMember
  {
    MethodName MethodName { get; }
  }
}
