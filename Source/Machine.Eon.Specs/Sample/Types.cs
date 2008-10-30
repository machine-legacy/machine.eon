using System;
using System.Collections.Generic;

namespace Machine.Eon.Specs.Sample
{
  public class EmptyType
  {
  }

  public class DerrivedFromEmptyType : EmptyType
  {
  }

  public class TypeThatOnlyReferencesStrings
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

  public class TypeThatHasOnlyStringsType
  {
    private readonly TypeThatOnlyReferencesStrings _other;

    public TypeThatHasOnlyStringsType(TypeThatOnlyReferencesStrings other)
    {
      _other = other;
    }

    public override string ToString()
    {
      return _other.ToString();
    }
  }

  public interface IDoStuff
  {
    string Name { get; }
  }

  public class TypeImplementsInterface : IDoStuff
  {
    public string Name
    {
      get { throw new NotImplementedException(); }
    }
  }
}
