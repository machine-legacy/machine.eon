using System;
using System.Linq;
using Machine.Eon.Mapping;
using Machine.Specifications;

namespace Machine.Eon.Specs.IndirectUses
{
  public class Type1
  {
  }

  [Subject("Indirect Uses")]
  public class with_empty_type : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type1));

    It should_have_indirect_uses_of_object_void_and_itself = () =>
      type.IndirectlyUses.ExtractTypes().ShouldContainOnly(qr.FromSystemType(typeof(Object), typeof(void), typeof(Type1)));
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
  public class with_type_that_creates_other_types : with_eon
  {
    static Machine.Eon.Mapping.Type type;

    Because of = () =>
      type = qr.FromSystemType(typeof(Type2));

    It should_have_indirect_uses_of_object_void_itself_and_those_types = () =>
      type.IndirectlyUses.ExtractTypes().ShouldContainOnly(qr.FromSystemType(typeof(Object), typeof(void), typeof(Type2), typeof(InvalidOperationException), typeof(ArgumentNullException)));
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
      type.IndirectlyUses.ExtractTypes().ShouldContainOnly(qr.FromSystemType(typeof(Object), typeof(void), typeof(Type2), typeof(Type3), typeof(InvalidOperationException), typeof(ArgumentNullException)));
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
      type.IndirectlyUses.ExtractTypes().ShouldContainOnly(qr.FromSystemType(typeof(Object), typeof(void), typeof(Type2), typeof(Type4), typeof(InvalidOperationException), typeof(ArgumentNullException)));
  }

  [Subject("Indirect Uses")]
  public class with_namespace_that_uses_another_type_we_tested_above : with_eon
  {
    static Machine.Eon.Mapping.Namespace ns;

    Because of = () =>
      ns = qr.NamespacesNamed("Sample.HasTypeWithType4").Single();

    It should_have_indirect_uses_of_object_void_itself_and_those_types = () =>
      ns.IndirectlyUses.ExtractTypes().ShouldContainOnly(qr.FromSystemType(typeof(Object), typeof(void), typeof(Type2), typeof(Type4), typeof(InvalidOperationException), typeof(ArgumentNullException), typeof(Sample.HasTypeWithType4.Type5)));
  }
}

namespace Sample.HasTypeWithType4
{
  public class Type5 : Machine.Eon.Specs.IndirectUses.Type4
  {
  }
}