using System;
using System.Collections.Generic;

using Machine.Specifications;

namespace Machine.Eon.Specs
{
  public class AClass
  {
  }

  public class AnotherClassThatIsAClass : AClass
  {
  }

  public abstract class AnAbstractClass
  {
  }

  public interface IAmInterface
  {
  }

  public class AClassThatImplementsIAmInterface : IAmInterface
  {
  }

  [Subject("ClassesAndInterfaces")]
  public class with_a_class : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(AClass));

    It should_be_a_class = () =>
      type.IsClass.ShouldBeTrue();

    It should_not_be_an_interface = () =>
      type.IsInterface.ShouldBeFalse();

    It should_not_be_abstract = () =>
      type.IsAbstract.ShouldBeFalse();
  }

  [Subject("ClassesAndInterfaces")]
  public class with_another_class_that_is_derrived_from_a_class : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(AnotherClassThatIsAClass));

    It should_have_a_class_as_base_type = () =>
      type.BaseType.ShouldEqual(qr.FromSystemType(typeof(AClass)));
  }

  [Subject("ClassesAndInterfaces")]
  public class with_an_abstract_class : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(AnAbstractClass));

    It should_be_a_class = () =>
      type.IsClass.ShouldBeTrue();

    It should_not_be_an_interface = () =>
      type.IsInterface.ShouldBeFalse();

    It should_be_abstract = () =>
      type.IsAbstract.ShouldBeTrue();
  }

  [Subject("ClassesAndInterfaces")]
  public class with_an_interface : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(IAmInterface));

    It should_not_be_a_class = () =>
      type.IsClass.ShouldBeFalse();

    It should_be_an_interface = () =>
      type.IsInterface.ShouldBeTrue();

    It should_be_abstract = () =>
      type.IsAbstract.ShouldBeTrue();
  }

  [Subject("ClassesAndInterfaces")]
  public class with_a_class_that_implements_an_interface : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(AClassThatImplementsIAmInterface));

    It should_have_system_object_as_base_type = () =>
      type.BaseType.ShouldEqual(qr.SystemObject);

    It should_have_interface = () =>
      type.Interfaces.ShouldContainOnly(qr.FromSystemType(typeof(IAmInterface)));
  }
}