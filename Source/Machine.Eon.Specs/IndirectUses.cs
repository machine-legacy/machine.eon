using System;
using System.Collections.Generic;
using System.Linq;

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

    It should_have_three_indirect_uses = () =>
      type.IndirectlyUses.Count().ShouldEqual(3);
  }
}
