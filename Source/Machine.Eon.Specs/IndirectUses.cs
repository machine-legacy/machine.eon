using System;
using System.Linq;
using System.Runtime.InteropServices;

using Machine.Eon.Mapping;
using Machine.Specifications;

namespace Machine.Eon.Specs.IndirectUses
{
  public class Type1
  {
  }

  [Subject("Indirect Uses")]
  public class with_system_object : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = systemObject;

    It should_indirectly_use_itself = () =>
      type.IndirectlyUses.ExtractTypes().ShouldContainOnly(
        systemObject, qr[typeof(ComVisibleAttribute)], qr[typeof(ClassInterfaceAttribute)]
      );

    It should_indirectly_use_its_methods = () =>
      type.IndirectlyUses.ExtractMembers().ShouldContainOnly(
        systemObject.Members
      );
  }

  [Subject("Indirect Uses")]
  public class with_empty_type : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type1));

    It should_indirectly_use_void_attributes_and_base_type = () =>
      type.IndirectlyUses.ExtractTypes().ShouldContainOnly(qr.FromSystemType(
        typeof(Object), typeof(void), typeof(ComVisibleAttribute), typeof(ClassInterfaceAttribute),
        typeof(Type1)
      ));

    It should_indirectly_use_only_base_type_ctor = () =>
      type.IndirectlyUses.ExtractMethods().ShouldContainOnly(
        systemObject.Constructors.First(),
        type.Constructors.First()
      );

    It should_indirectly_use_no_properties = () =>
      type.IndirectlyUses.Properties.ShouldBeEmpty();
  }

  public class Type2
  {
    public void IsCalled()
    {
      throw new InvalidOperationException();
    }

    public void NotCalled()
    {
      throw new ArgumentNullException();
    }
  }

  [Subject("Indirect Uses")]
  public class with_method_that_creates_exception : with_eon
  {
    static Method method;

    Because of = () =>
      method = (Method)qr[typeof(Type2)]["IsCalled"].First();

    It should_indirectly_use_itself_and_exception = () =>
      method.IndirectlyUses.ExtractTypes().ShouldContainOnly(
        method.Type,
        qr[typeof(InvalidOperationException)]
      );

    It should_indirectly_use_exception_ctor = () =>
      method.IndirectlyUses.ExtractMembers().ShouldContainOnly(
        qr[typeof(InvalidOperationException)].Constructors.First()
      );
  }

  [Subject("Indirect Uses")]
  public class with_type_that_creates_other_types : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type2));

    It should_indirectly_use_void_attributes_base_type_and_exceptions = () =>
      type.IndirectlyUses.ExtractTypes().ShouldContainOnly(qr.FromSystemType(
        typeof(Object), typeof(void), typeof(ComVisibleAttribute), typeof(ClassInterfaceAttribute),
        typeof(Type2), typeof(InvalidOperationException), typeof(ArgumentNullException)
      ));

    It should_indirectly_use_members_and_ctors_for_base_and_exceptions = () =>
      type.IndirectlyUses.ExtractMembers().ShouldContainOnly(
        type.Constructors.First(),
        type["IsCalled"].First(),
        type["Notcalled"].First(),
        systemObject.Constructors.First(),
        qr[typeof(InvalidOperationException)].Constructors.First(),
        qr[typeof(ArgumentNullException)].Constructors.First()
      );
  }

  public class Type3
  {
    private readonly Type2 _type;

    public Type3()
    {
      _type = new Type2();
    }

    public void AMethod()
    {
      _type.IsCalled();
    }
  }

  [Subject("Indirect Uses")]
  public class with_type_that_has_a_reference_to_another_type : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type3));

    It should_have_indirect_uses_of_object_void_itself_and_those_types = () =>
      type.IndirectlyUses.ExtractTypes().ShouldContainOnly(qr.FromSystemType(
        typeof(Object), typeof(void), typeof(ComVisibleAttribute), typeof(ClassInterfaceAttribute),
        typeof(Type2), typeof(Type3), typeof(InvalidOperationException), typeof(ArgumentNullException)
      ));

    It should_indirectly_use_exception_ctors_base_type_ctor_and_method = () =>
      type.IndirectlyUses.ExtractMembers().ShouldContainOnly(
        qr[typeof(Type3)]["AMethod"].First(),
        type.Constructors.First(),
        qr[typeof(Type2)].Constructors.First(),
        qr[typeof(Type2)]["IsCalled"].First(),
        systemObject.Constructors.First(),
        qr[typeof(InvalidOperationException)].Constructors.First()
      );
  }

  public class Type4
  {
    public void AMethod()
    {
      new Type2();
    }
  }

  [Subject("Indirect Uses")]
  public class with_type_that_creates_another_type_in_a_method : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type4));

    It should_have_indirect_uses_of_object_void_itself_and_those_types = () =>
      type.IndirectlyUses.ExtractTypes().ShouldContainOnly(qr.FromSystemType(
        typeof(Object), typeof(void), typeof(ComVisibleAttribute), typeof(ClassInterfaceAttribute),
        typeof(Type2), typeof(Type4), typeof(InvalidOperationException), typeof(ArgumentNullException)
      ));

    It should_indirectly_use_exception_ctors_base_type_ctor_and_method = () =>
      type.IndirectlyUses.ExtractMembers().ShouldContainOnly(
        type.Constructors.First(),
        type["AMethod"].First(),
        qr[typeof(Type2)].Constructors.First(),
        systemObject.Constructors.First()
      );
  }

  [Subject("Indirect Uses")]
  public class with_namespace_that_uses_another_type_we_tested_above : with_eon
  {
    static Machine.Eon.Mapping.Namespace ns;

    Because of = () =>
      ns = qr.NamespacesNamed("Sample.HasTypeWithType4").Single();

    It should_have_indirect_uses_of_object_void_itself_and_those_types = () =>
      ns.IndirectlyUses.ExtractTypes().ShouldContainOnly(qr.FromSystemType(
        typeof(Object), typeof(void), typeof(ComVisibleAttribute), typeof(ClassInterfaceAttribute),
        typeof(Type1), typeof(Type2), typeof(Type4), typeof(InvalidOperationException),
        typeof(ArgumentNullException), typeof(Sample.HasTypeWithType4.Type5)
      ));
  }

  public class Type6
  {
    public void AnotherMethod()
    {
      Type4 type4 = new Type4();
      type4.AMethod();
    }
  }

  public class Type7 : Type4
  {
  }
}

namespace Sample.HasTypeWithType4
{
  public class Type5 : Machine.Eon.Specs.IndirectUses.Type4
  {
  }
}