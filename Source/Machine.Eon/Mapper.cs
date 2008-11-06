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
    private readonly List<string> _directories = new List<string>();
    private readonly IAssemblyRepository _assemblyRepository;
    private readonly ITypeRepository _typeRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ModelCreator _modelCreator;

    public Mapper()
    {
      _assemblyRepository = new AssemblyRepository();
      _typeRepository = new TypeRepository(_assemblyRepository);
      _memberRepository = new MemberRepository(_typeRepository);
      _modelCreator = new ModelCreator(_typeRepository, _memberRepository);
    }

    public void Include(string path)
    {
      string directory = System.IO.Path.GetDirectoryName(path);
      if (!_directories.Contains(directory))
      {
        _directories.Add(directory);
      }
      AssemblyDefinition definition = AssemblyFactory.GetAssembly(path);
      MyReflectionStructureVisitor visitor = new MyReflectionStructureVisitor(_modelCreator, new VisitationOptions(true));
      definition.Accept(visitor);
    }

    public QueryRoot ToQueryRoot()
    {
      PendingTypeLoader pendingTypeLoader = new PendingTypeLoader(_modelCreator, _directories);
      pendingTypeLoader.Load(_assemblyRepository.FindAll());
      return new QueryRoot(_assemblyRepository.FindAll());
    }
  }

  public class PendingTypeLoader
  {
    private readonly ExternalAssemblyLoader _externalAssemblyLoader;
    private readonly ModelCreator _modelCreator;

    public PendingTypeLoader(ModelCreator modelCreator, List<string> directories)
    {
      _modelCreator = modelCreator;
      _externalAssemblyLoader = new ExternalAssemblyLoader(directories);
    }

    public void Load(IEnumerable<Assembly> assemblies)
    {
      foreach (AssemblyDefinition definition in _externalAssemblyLoader.FindExternalAssemblyDefinitions(assemblies))
      {
        MyReflectionStructureVisitor visitor = new MyReflectionStructureVisitor(_modelCreator, new VisitationOptions(false));
        definition.Accept(visitor);
      }
    }
  }

  public class ExternalAssemblyLoader
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(PendingTypeLoader));
    private readonly List<string> _directories;

    public ExternalAssemblyLoader(List<string> directories)
    {
      _directories = directories;
    }

    public IEnumerable<AssemblyDefinition> FindExternalAssemblyDefinitions(IEnumerable<Assembly> assemblies)
    {
      List<AssemblyDefinition> definitions = new List<AssemblyDefinition>();
      var pending = from assembly in assemblies from type in assembly.Types where type.IsPending select assembly;
      foreach (Assembly assembly in pending.Distinct())
      {
        AssemblyDefinition definition = GetAssemblyDefinition(assembly);
        if (definition == null)
        {
          _log.Warn("Not able to load: " + assembly.Key);
        }
        definitions.Add(definition);
      }
      return definitions;
    }

    private AssemblyDefinition GetAssemblyDefinition(Assembly assembly)
    {
      try
      {
        System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.ReflectionOnlyLoad(assembly.Key.FullName);
        return AssemblyFactory.GetAssembly(dotNetAssembly.Location);
      }
      catch (Exception)
      {
        return GetAssemblyDefinitionFromPath(assembly);
      }
    }

    private AssemblyDefinition GetAssemblyDefinitionFromPath(Assembly assembly)
    {
      string[] extensions = new string[]  { ".dll", ".exe" };
      foreach (string directory in _directories)
      {
        foreach (string extension in extensions)
        {
          string filename = System.IO.Path.Combine(directory, assembly.Key.Name + extension);
          if (System.IO.File.Exists(filename))
          {
            System.Reflection.Assembly dotNetAssembly = System.Reflection.Assembly.ReflectionOnlyLoadFrom(filename);
            return AssemblyFactory.GetAssembly(dotNetAssembly.Location);
          }
        }
      }
      return null;
    }
  }
}
