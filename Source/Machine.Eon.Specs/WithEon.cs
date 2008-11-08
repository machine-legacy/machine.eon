using System;
using System.Collections.Generic;
using System.Linq;

using Machine.Eon.Mapping;
using Type = Machine.Eon.Mapping.Type;

using Machine.Specifications;

namespace Machine.Eon.Specs
{
  public class with_eon
  {
    protected static QueryRoot qr;
    protected static Type systemVoid;
    protected static Type systemString;
    protected static Type systemObject;

    Establish context = () =>
    {
      log4net.Config.XmlConfigurator.Configure();
      if (qr == null)
      {
        Mapper mapper = new Mapper();
        mapper.Include(typeof(Mapper).Assembly.Location);
        mapper.Include(typeof(with_eon).Assembly.Location);
        qr = mapper.ToQueryRoot();
        systemVoid = qr.FromSystemType(typeof(void));
        systemString = qr.FromSystemType(typeof(string));
        systemObject = qr.SystemObject;
      }
    };
  }

  public class with_eon_query_root
  {
    protected static QueryRoot qr;
    protected static Type systemVoid;
    protected static Type systemString;

    Establish context = () =>
    {
      log4net.Config.XmlConfigurator.Configure();
      if (qr == null)
      {
        Mapper mapper = new Mapper();
        mapper.Include(@"E:\Source\Page5of4\collaborate\build\debug\Machine.Mta.dll");
        mapper.Include(@"E:\Source\Page5of4\collaborate\build\debug\Collaborate.Core.dll");
        mapper.Include(@"E:\Source\Page5of4\collaborate\build\debug\Collaborate.Messages.dll");
        mapper.Include(@"E:\Source\Page5of4\collaborate\build\debug\Collaborate.Server.dll");
        qr = mapper.ToQueryRoot();
        systemVoid = qr.FromSystemType(typeof(void));
        systemString = qr.FromSystemType(typeof(string));
      }
    };
  }

  public class with_messages : with_eon_query_root
  {
    protected static Type message;
    protected static IEnumerable<Type> messages;
    
    Establish context = () =>
    {
      message = qr[new TypeKey(new AssemblyKey("Machine.Mta"), "Machine.Mta.IMessage")];
      messages = qr.TypesThatAre(message);
    };
  }

  [Subject("Messages")]
  public class when_defined : with_messages
  {
    It should_have_no_fields = () =>
      (from type in messages from field in type.Fields select field).ShouldBeEmpty();

    It should_have_no_methods = () =>
      (from type in messages from method in type.MethodsNotPartOfProperties select method).ShouldBeEmpty();

    It should_have_only_read_write_properties = () =>
      (from type in messages from property in type.Properties where !property.IsReadWrite select property).ShouldBeEmpty();

    It should_be_an_interface = () =>
      (from type in messages where !type.IsInterface select type).ShouldBeEmpty();
  }

  [Subject("Messages")]
  public class when_used : with_messages
  {
    static IEnumerable<Method> setters;
    static IEnumerable<Method> methods_that_use_setters;
    static IEnumerable<Type> types_that_use_setters;
    static Type builder;
    
    Because of = () =>
    {
      builder = qr[new TypeKey(new AssemblyKey("Collaborate.Server"), "Collaborate.Server.MessageHandlers.MessageBuilders")];
      setters = from type in messages from method in type.Methods where method.IsSetter select method;
      methods_that_use_setters = (from method in qr.Methods where method.DirectlyUses.Methods.ContainsAny(setters) select method).Distinct();
      types_that_use_setters = (from method in methods_that_use_setters select method.Type).Distinct();
    };

    It should_only_set_properties_from_message_builders = () =>
      types_that_use_setters.ShouldContainOnly(builder);
  }
}
