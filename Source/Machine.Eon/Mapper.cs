using System;
using System.Collections.Generic;

using Mono.Cecil;

using Machine.Eon.Mapping.Inspection;
using Machine.Eon.Mapping.Repositories;
using Machine.Eon.Mapping.Repositories.Impl;

namespace Machine.Eon
{
  public class Mapper
  {
    private readonly IAssemblyRepository _assemblyRepository;
    private readonly ITypeRepository _typeRepository;
    private readonly IMemberRepository _memberRepository;

    public Mapper()
    {
      _assemblyRepository = new AssemblyRepository();
      _typeRepository = new TypeRepository(_assemblyRepository);
      _memberRepository = new MemberRepository(_assemblyRepository);
    }

    public void Include(string path)
    {
      AssemblyDefinition definition = AssemblyFactory.GetAssembly(path);
      Listener listener = new Listener(_typeRepository, _memberRepository);
      MyReflectionStructureVisitor visitor = new MyReflectionStructureVisitor(listener);
      definition.Accept(visitor);
    }

    public QueryRoot ToQueryRoot()
    {
      return new QueryRoot(_assemblyRepository.FindAll());
    }
  }
}
