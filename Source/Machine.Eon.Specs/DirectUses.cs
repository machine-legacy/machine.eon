using System;
using System.Collections.Generic;
using System.Linq;

using Machine.Specifications;

namespace Machine.Eon.Specs
{
  [Subject("Direct Uses")]
  public class with_empty_type : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(EmptyType));

    It should_have_two_direct_uses = () =>
      type.DirectlyUses.Count().ShouldEqual(2);

    It should_have_direct_use_of_system_object = () =>
      type.DirectlyUses.ShouldContain(qr.SystemObject);

    It should_have_direct_use_of_system_void = () =>
      type.DirectlyUses.ShouldContain(systemVoid);
  }

  [Subject("Direct Uses")]
  public class with_type_derrived_from_empty_type : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(DerrivedFromEmptyType));

    It should_have_three_direct_uses = () =>
      type.DirectlyUses.Count().ShouldEqual(2);

    It should_have_direct_use_of_system_void = () =>
      type.DirectlyUses.ShouldContain(systemVoid);

    It should_have_direct_use_of_empty_type = () =>
      type.DirectlyUses.ShouldContain(qr.FromSystemType(typeof(EmptyType)));
  }

  [Subject("Direct Uses")]
  public class with_type_that_only_references_strings : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type6));

    It should_have_three_direct_uses = () =>
      type.DirectlyUses.Count().ShouldEqual(3);

    It should_have_direct_use_of_system_object = () =>
      type.DirectlyUses.ShouldContain(qr.SystemObject);

    It should_have_direct_use_of_system_void = () =>
      type.DirectlyUses.ShouldContain(systemVoid);

    It should_have_direct_use_of_system_string = () =>
      type.DirectlyUses.ShouldContain(systemString);
  }

  [Subject("Direct Uses")]
  public class with_type_that_has_a_field_of_another_type : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type5));

    It should_use_the_fields_type_directly = () =>
      type.DirectlyUses.ShouldContain(qr.FromSystemType(typeof(Type6)));
  }

  [Subject("Direct Uses")]
  public class with_type_that_implements_an_interface_with_a_property_returning_a_string : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type4));

    It should_use_interface_directly = () =>
      type.DirectlyUses.ShouldContain(qr.FromSystemType(typeof(IAmInterfaceReturningString)));
  }

  [Subject("Direct Uses")]
  public class with_type_that_is_an_interface_with_a_property_returning_a_string : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(IAmInterfaceReturningString));

    It should_use_string_directly = () =>
      type.DirectlyUses.ShouldContain(qr.FromSystemType(typeof(String)));
  }

  [Subject("Direct Uses")]
  public class with_type_that_creates_another_type_in_a_method : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type3));

    It should_use_the_type_that_is_created_directly = () =>
      type.DirectlyUses.ShouldContain(qr.FromSystemType(typeof(Type1)));
  }

  [Subject("Direct Uses")]
  public class with_type_that_takes_another_type_as_a_parameter : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type2));

    It should_use_the_type_that_is_taken_directly = () =>
      type.DirectlyUses.ShouldContain(qr.FromSystemType(typeof(Type1)));
  }

  [Subject("Direct Uses")]
  public class with_type_that_has_a_single_field : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type7));

    It should_use_the_fields_type_directly = () =>
      type.DirectlyUses.ShouldContain(qr.FromSystemType(typeof(string)));
  }

  [Subject("Direct Uses")]
  public class with_type_that_has_abstract_property : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type8));

    It should_use_the_properties_type_directly = () =>
      type.DirectlyUses.ShouldContain(qr.FromSystemType(typeof(string)));
  }

  [Subject("Direct Uses")]
  public class with_type_that_extends_another_type : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type9));

    It should_use_the_base_type_directly = () =>
      type.DirectlyUses.ShouldContain(qr.FromSystemType(typeof(Type1)));
  }

  public class Type7
  {
    private string _name;
  }

  public abstract class Type8
  {
    public abstract string Name { get; }
  }

  public class Type9 : Type1
  {
  }

  public class EmptyType
  {
  }

  public class DerrivedFromEmptyType : EmptyType
  {
  }

  public class Type5
  {
    private readonly Type6 _other;

    public Type5(Type6 other)
    {
      _other = other;
    }

    public override string ToString()
    {
      return _other.ToString();
    }
  }

  public interface IAmInterfaceReturningString
  {
    string Name { get; }
  }

  public class Type4 : IAmInterfaceReturningString
  {
    public string Name
    {
      get { throw new NotImplementedException(); }
    }

    public void DoStuff()
    {
    }
  }

  public class Type2
  {
  }

  public class Type1
  {
    public void AMethod(Type2 type)
    {
    }
  }

  public class Type3
  {
    public void AMethod()
    {
      new Type1();
    }
  }

  public class Type6
  {
    private string _name;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public void ChangeName(string name)
    {
      this.Name = name;
    }
  }
}
