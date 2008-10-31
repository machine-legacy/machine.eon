using System;
using System.Collections.Generic;
using System.Linq;

using Mono.Cecil;

using Machine.Eon.Mapping;
using Machine.Eon.Mapping.Inspection;
using Machine.Eon.Mapping.Repositories;
using Machine.Eon.Mapping.Repositories.Impl;
using Type = Machine.Eon.Mapping.Type;

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
      _memberRepository = new MemberRepository(_assemblyRepository);
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
      CheckForInvalidTypes();
      return new QueryRoot(_assemblyRepository.FindAll());
    }

    public void CheckForInvalidTypes()
    {
      var invalids = from assembly in _assemblyRepository.FindAll()
                     from ns in assembly.Namespaces
                     from type in ns.Types
                     where type.IsPendingCreation
                     select type;
      foreach (Type type in invalids.Distinct())
      {
        _log.Info("Invalid: " + type);
      }

      var assemblies = from assembly in _assemblyRepository.FindAll()
                       where assembly.Key.Name.Equals("Machine.Eon.Specs")
                       select assembly;
      
      var methods = from assembly in assemblies
                    from ns in assembly.Namespaces
                    from type in ns.Types
                    from method in type.Methods
                    where method.IsPendingCreation
                    select method;
      foreach (Method method in methods)
      {
        _log.Info("Invalid: " + method);
      }

      var types = from assembly in assemblies
                  from ns in assembly.Namespaces
                  from type in ns.Types
                  where ns.Key.Name.Equals("Machine.Eon.Specs.ClassesAndInterfaces")
                  select type;
      foreach (Type type in types)
      {
        _log.Info(type);
      }
    }
  }
}
