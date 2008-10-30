using System;
using System.Collections.Generic;
using System.Linq;

using Machine.Eon.Specs.Sample;
using Machine.Specifications;

namespace Machine.Eon.Specs
{
  [Subject("Mapping")]
  public class with_empty_type : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
    {
      type = qr.FromSystemType(typeof(EmptyType));
    };

    It should_have_one_member_for_ctor = () =>
      type.Members.Count().ShouldEqual(1);

    It should_have_two_direct_uses = () =>
      type.DirectlyUses.Count().ShouldEqual(2);

    It should_have_direct_use_of_system_object = () =>
      type.DirectlyUses.ShouldContain(qr.SystemObject);

    It should_have_direct_use_of_system_void = () =>
      type.DirectlyUses.ShouldContain(systemVoid);

    It should_have_no_attributes = () =>
      type.Attributes.ShouldBeEmpty();

    It should_have_no_interfaces = () =>
      type.Interfaces.ShouldBeEmpty();

    It should_have_system_object_as_base_type = () =>
      type.BaseType.ShouldEqual(qr.SystemObject);
  }

  [Subject("Mapping")]
  public class with_type_derrived_from_empty_type : with_eon
  {
    static Machine.Eon.Mapping.Type emptyType;
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
    {
      emptyType = qr.FromSystemType(typeof(EmptyType));
      type = qr.FromSystemType(typeof(DerrivedFromEmptyType));
    };

    It should_have_one_member_for_ctor = () =>
      type.Members.Count().ShouldEqual(1);

    It should_have_three_direct_uses = () =>
      type.DirectlyUses.Count().ShouldEqual(2);

    It should_have_direct_use_of_system_void = () =>
      type.DirectlyUses.ShouldContain(systemVoid);

    It should_have_direct_use_of_empty_type = () =>
      type.DirectlyUses.ShouldContain(emptyType);

    It should_have_empty_type_as_base_type = () =>
      type.BaseType.ShouldEqual(emptyType);
  }

  [Subject("Mapping")]
  public class with_type_that_only_references_strings : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
    {
      type = qr.FromSystemType(typeof(TypeThatOnlyReferencesStrings));
    };

    It should_have_one_field = () =>
      type.Fields.Count().ShouldEqual(1);

    It should_have_one_property = () =>
      type.Properties.Count().ShouldEqual(1);
    
    It should_have_four_methods_for_ctor_get_set_and_regular_method = () =>
      type.Methods.Count().ShouldEqual(4);

    It should_have_three_direct_uses = () =>
      type.DirectlyUses.Count().ShouldEqual(3);

    It should_have_direct_use_of_system_object = () =>
      type.DirectlyUses.ShouldContain(qr.SystemObject);

    It should_have_direct_use_of_system_void = () =>
      type.DirectlyUses.ShouldContain(systemVoid);

    It should_have_direct_use_of_system_string = () =>
      type.DirectlyUses.ShouldContain(systemString);
  }

  [Subject("Mapping")]
  public class with_type_that_has_reference_to_type_with_only_strings : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
    {
      type = qr.FromSystemType(typeof(TypeThatHasOnlyStringsType));
    };

    It should_have_one_field = () =>
      type.Fields.Count().ShouldEqual(1);
  }

  [Subject("Mapping")]
  public class with_type_that_implements_interface : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
    {
      type = qr.FromSystemType(typeof(TypeImplementsInterface));
    };

    It should_use_interface_directly = () =>
      type.DirectlyUses.ShouldContain(qr.FromSystemType(typeof(IDoStuff)));

    It should_have_interface = () =>
      type.Interfaces.ShouldContainOnly(qr.FromSystemType(typeof(IDoStuff)));
  }

  [Subject("Mapping")]
  public class with_simple_interface : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
    {
      type = qr.FromSystemType(typeof(IDoStuff));
    };

    It should_use_string_directly = () =>
      type.DirectlyUses.ShouldContain(qr.FromSystemType(typeof(String)));

    It should_be_interface = () =>
      type.IsInterface.ShouldBeTrue();
  }
  /*
  Indirect uses should include self?
  */
}
