using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Machine.Eon.Specs
{
  public static class MyExtensionMethods
  {
    public static void MyShouldContainOnly<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
    {
      var actualList = new List<T>(actual);
      var remainingList = new List<T>(actualList);
      foreach (var item in expected)
      {
        Assert.Contains(item, actualList);
        remainingList.Remove(item);
      }
      Assert.IsEmpty(remainingList, "Actual collection has unexpected items.");
    }

    public static void MyShouldContainOnly<T>(this IEnumerable<T> actual, params T[] expected)
    {
      MyShouldContainOnly(actual, new List<T>(expected));
    }

    public static bool ContainsAny<T>(this IEnumerable<T> collection, IEnumerable<T> values)
    {
      foreach (T item in values)
      {
        if (collection.Contains(item))
        {
          return true;
        }
      }
      return false;
    }
  }
}