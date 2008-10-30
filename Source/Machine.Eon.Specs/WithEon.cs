using System;
using System.Collections.Generic;

using Machine.Eon.Mapping;

using Machine.Specifications;

namespace Machine.Eon.Specs
{
  public class with_eon
  {
    protected static QueryRoot qr;
    protected static Machine.Eon.Mapping.Type systemVoid;
    protected static Machine.Eon.Mapping.Type systemString;

    Establish context = () =>
    {
      Mapper mapper = new Mapper();
      mapper.Include(typeof(with_eon).Assembly.Location);
      qr = mapper.ToQueryRoot();
      systemVoid = qr.FromSystemType(typeof(void));
      systemString = qr.FromSystemType(typeof(string));
    };
  }
}
