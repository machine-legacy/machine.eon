using System;
using System.Collections.Generic;
using System.Linq;

using Machine.Specifications;

namespace Machine.Eon.Specs.DirectUses
{
  [Subject("Direct Uses")]
  public class with_empty_type : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr[typeof(EmptyType)];

    It should_directly_use_object_void_and_base_type_constructor = () =>
      type.DirectlyUses.ShouldContainOnly(qr.SystemObject, systemVoid, qr.SystemObject.Constructors.First());

    It should_have_direct_use_of_base_type_constructor = () =>
      type.DirectlyUses.ShouldContain(qr.SystemObject.Constructors.First());

    It should_have_direct_use_of_base_type = () =>
      type.DirectlyUses.ShouldContain(qr.SystemObject);

    It should_have_direct_use_of_system_void = () =>
      type.DirectlyUses.ShouldContain(systemVoid);
  }

  [Subject("Direct Uses")]
  public class with_type_derrived_from_empty_type : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr[typeof(DerrivedFromEmptyType)];

    It should_directly_use_base_type_void_and_base_type_constructor = () =>
      type.DirectlyUses.ShouldContainOnly(qr[typeof(EmptyType)], systemVoid, qr[typeof(EmptyType)].Constructors.First());

    It should_have_direct_use_of_base_type = () =>
      type.DirectlyUses.ShouldContain(qr.FromSystemType(typeof(EmptyType)));

    It should_have_direct_use_of_base_type_constructor = () =>
      type.DirectlyUses.ShouldContain(qr[typeof(EmptyType)].Constructors.First());

    It should_have_direct_use_of_system_void = () =>
      type.DirectlyUses.ShouldContain(systemVoid);
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

  [Subject("Direct Uses")]
  public class with_type_that_only_references_strings : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr[typeof(Type6)];

    It should_directly_use_void_string_object_object_ctor = () =>
      type.DirectlyUses.ShouldContainOnly(systemVoid, systemString, systemObject, systemObject.Constructors.First());

    It should_directly_use_of_base_type_object = () =>
      type.DirectlyUses.ShouldContain(qr.SystemObject);

    It should_have_direct_use_of_base_type_constructor = () =>
      type.DirectlyUses.ShouldContain(qr.SystemObject.Constructors.First());

    It should_directly_use_of_system_void = () =>
      type.DirectlyUses.ShouldContain(systemVoid);

    It should_directly_use_of_system_string = () =>
      type.DirectlyUses.ShouldContain(systemString);
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

  [Subject("Direct Uses")]
  public class with_type_that_has_a_field_of_another_type : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr[typeof(Type5)];

    It should_directly_use_void_string_object_object_ctor_field_types_and_type_to_string = () =>
      type.DirectlyUses.ShouldContainOnly(systemVoid, systemString, systemObject, systemObject.Constructors.First(), qr[typeof(Type6)], systemObject["ToString"].First());

    It should_directly_use_of_base_type_object = () =>
      type.DirectlyUses.ShouldContain(qr.SystemObject);

    It should_have_direct_use_of_base_type_constructor = () =>
      type.DirectlyUses.ShouldContain(qr.SystemObject.Constructors.First());

    It should_directly_use_of_system_void = () =>
      type.DirectlyUses.ShouldContain(systemVoid);

    It should_directly_use_fields_type = () =>
      type.DirectlyUses.ShouldContain(qr.FromSystemType(typeof(Type6)));
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

  [Subject("Direct Uses")]
  public class with_type_that_implements_an_interface_with_a_property_returning_a_string : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr[typeof(Type4)];

    It should_directly_use_void_string_base_type_base_type_ctor_interfaces_and_types_and_ctors_in_methods = () =>
      type.DirectlyUses.ShouldContainOnly(systemVoid, systemString, systemObject, systemObject.Constructors.First(), qr[typeof(IAmInterfaceReturningString)], qr[typeof(NotImplementedException)], qr[typeof(NotImplementedException)].Constructors.First());

    It should_directly_use_the_interface = () =>
      type.DirectlyUses.ShouldContain(qr[typeof(IAmInterfaceReturningString)]);
  }

  [Subject("Direct Uses")]
  public class with_type_that_is_an_interface_with_a_property_returning_a_string : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr[typeof(IAmInterfaceReturningString)];

    It should_directly_use_string = () =>
      type.DirectlyUses.ShouldContainOnly(qr.FromSystemType(typeof(String)));
  }

  [Subject("Direct Uses")]
  public class with_type_that_creates_another_type_in_a_method : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr[typeof(Type3)];

    It should_directly_use_void_base_type_base_type_and_created_types_and_its_ctor = () =>
      type.DirectlyUses.ShouldContainOnly(systemVoid, systemObject, systemObject.Constructors.First(), qr[typeof(Type1)], qr[typeof(Type1)].Constructors.First());

    It should_directly_use_the_type_that_it_creates = () =>
      type.DirectlyUses.ShouldContain(qr[typeof(Type1)]);

    It should_directly_use_the_types_ctor_that_it_creates = () =>
      type.DirectlyUses.ShouldContain(qr[typeof(Type1)].Constructors.First());
  }

  public class Type1
  {
    public void AMethod(Type2 type)
    {
    }
  }

  [Subject("Direct Uses")]
  public class with_type_that_takes_another_type_as_a_parameter : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr[typeof(Type1)];

    It should_directly_use_void_base_type_base_type_and_parameter_types = () =>
      type.DirectlyUses.ShouldContainOnly(systemVoid, systemObject, systemObject.Constructors.First(), qr[typeof(Type2)]);
  }

  public class Type7
  {
#pragma warning disable 169
    private string _name;
#pragma warning restore 169
  }

  [Subject("Direct Uses")]
  public class with_type_that_has_a_single_field : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr[typeof(Type7)];

    It should_directly_use_standard_types_and_field_type = () =>
      type.DirectlyUses.ShouldContainOnly(systemVoid, systemObject, systemObject.Constructors.First(), qr[typeof(String)]);
  }

  public abstract class Type8
  {
    public abstract string Name { get; }
  }

  [Subject("Direct Uses")]
  public class with_type_that_has_an_abstract_property : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr[typeof(Type8)];

    It should_directly_use_standard_types_and_property_type = () =>
      type.DirectlyUses.ShouldContainOnly(systemVoid, systemObject, systemObject.Constructors.First(), qr[typeof(String)]);
  }

  public class Type9 : Type1
  {
  }

  [Subject("Direct Uses")]
  public class with_type_that_extends_another_type : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr[typeof(Type9)];

    It should_directly_use_void_and_base_type_and_its_ctor = () =>
      type.DirectlyUses.ShouldContainOnly(systemVoid, qr[typeof(Type1)], qr[typeof(Type1)].Constructors.First());
  }

  public abstract class Sample1
  {
    public abstract string Name { get; }
  }

  public class Sample2<T> where T : Sample1
  {
    private readonly T _sample;

    public Sample2(T sample)
    {
      _sample = sample;
    }

    public void SayHello()
    {
      Console.WriteLine(_sample.Name);
    }
  }

  [Subject("Direct Uses")]
  public class with_a_class_that_has_a_generic_parameter_with_a_constraint : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr[typeof(Sample2<Sample1>)];
    
    It should_directly_use_immediate_types_and_methods = () =>
      type.DirectlyUses.ShouldContainOnly(systemVoid, systemObject, systemObject.Constructors.First(),
                                          qr[typeof(Console)], qr[typeof(Sample1)],
                                          qr[typeof(Console)]["WriteLine"].First(),
                                          qr[typeof(Sample1)]["get_Name"].First());

    It should_directly_use_the_constrained_type = () =>
      type.DirectlyUses.ShouldContain(qr[typeof(Sample1)]);
  }

  public class EmptyType
  {
  }

  public class DerrivedFromEmptyType : EmptyType
  {
  }

  public interface IAmInterfaceReturningString
  {
    string Name { get; }
  }

  public class Type2
  {
  }

  public class Type3
  {
    public void AMethod()
    {
      new Type1();
    }
  }
}
