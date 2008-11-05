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
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(Mapper));
    private readonly IAssemblyRepository _assemblyRepository;
    private readonly ITypeRepository _typeRepository;
    private readonly IMemberRepository _memberRepository;

    public Mapper()
    {
      _assemblyRepository = new AssemblyRepository();
      _typeRepository = new TypeRepository(_assemblyRepository);
      _memberRepository = new MemberRepository(_typeRepository);
    }

    public void Include(string path)
    {
      AssemblyDefinition definition = AssemblyFactory.GetAssembly(path);
      ModelCreator modelCreator = new ModelCreator(_typeRepository, _memberRepository);
      MyReflectionStructureVisitor visitor = new MyReflectionStructureVisitor(modelCreator);
      definition.Accept(visitor);
    }

    public QueryRoot ToQueryRoot()
    {
      return new QueryRoot(_assemblyRepository.FindAll());
    }
  }
}
