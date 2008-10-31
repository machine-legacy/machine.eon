using System;
using System.Collections.Generic;
using System.Linq;

using Machine.Specifications;

namespace Machine.Eon.Specs.Members
{
  public class Type1
  {
  }

  [Subject("Members")]
  public class with_a_class_that_has_no_members : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type1));

    It should_have_one_member = () =>
      type.Members.Count().ShouldEqual(1);

    It should_have_a_method_that_is_a_constructor = () =>
      type.Methods.First().IsConstructor.ShouldBeTrue();
  }

  public class Type2
  {
    private string _name;

    public Type2(string name)
    {
      _name = name;
    }
  }

  [Subject("Members")]
  public class with_a_class_that_has_one_field : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type2));

    It should_have_two_members = () =>
      type.Members.Count().ShouldEqual(2);

    It should_have_a_method_that_is_a_constructor = () =>
      type.Methods.First().IsConstructor.ShouldBeTrue();

    It should_have_a_field = () =>
      type.Fields.First().ShouldNotBeNull();
  }

  public class Type3
  {
    private string _name;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }
  }

  [Subject("Members")]
  public class with_a_class_that_has_a_property : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type3));

    It should_have_five_members = () =>
      type.Members.Count().ShouldEqual(5);

    It should_have_three_methods = () =>
      type.Methods.Count().ShouldEqual(3);

    It should_have_one_method_not_a_part_of_properties = () =>
      type.MethodsNotPartOfProperties.Count().ShouldEqual(1);

    It should_have_a_method_that_is_a_constructor = () =>
      type.Methods.First().IsConstructor.ShouldBeTrue();

    It should_have_a_field = () =>
      type.Fields.First().ShouldNotBeNull();

    It should_have_a_propery = () =>
      type.Properties.First().ShouldNotBeNull();

    It should_have_a_propery_that_is_read_write = () =>
      type.Properties.First().IsReadWrite.ShouldBeTrue();
  }

  public abstract class Type4
  {
    public abstract string Name { get; }
  }

  [Subject("Members")]
  public class with_a_class_that_has_an_abstract_read_only_property : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type4));

    It should_have_a_propery = () =>
      type.Properties.First().ShouldNotBeNull();

    It should_have_a_propery_that_is_read_only = () =>
      type.Properties.First().IsReadOnly.ShouldBeTrue();

    It should_have_a_propery_that_has_abstract_getter = () =>
      type.Properties.First().Getter.IsAbstract.ShouldBeTrue();

    It should_have_a_propery_that_has_no_setter = () =>
      type.Properties.First().Setter.ShouldBeNull();
  }

  public class Type5
  {
    public virtual string GetName()
    {
      return "Andrea";
    }
  }

  [Subject("Members")]
  public class with_a_class_that_has_virtual_method : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type5));

    It should_have_method_that_is_virtual = () =>
      (from method in type.Methods where method.IsVirtual select method).ShouldNotBeEmpty();
  }

  public class Type6
  {
    public event EventHandler EndOfTheWorld;
  }

  [Subject("Members")]
  public class with_a_class_that_has_an_event : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type6));

    It should_have_five_members = () =>
      type.Members.Count().ShouldEqual(5);

    It should_have_a_method_that_is_a_constructor = () =>
      type.Methods.First().IsConstructor.ShouldBeTrue();

    It should_have_an_event = () =>
      type.Events.First().ShouldNotBeNull();
  }
}
