using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class EnumerableExtenstions
    {
        public static T FindMatch<T>(this IEnumerable<T> enumerable, Func<T, bool> comparison)
        {
            T retVal = default(T);
            foreach (T item in enumerable)
            {
                if (comparison(item))
                {
                    retVal = item;
                    break;
                }
            }
            return retVal;
        }

        public static List<T> FindMatchAll<T>(this IEnumerable<T> enumerable, Func<T, bool> comparison)
        {
            List<T> match = new List<T>();
            if (enumerable != null)
            {
                foreach (var item in enumerable)
                {
                    if (comparison(item))
                    {
                        match.Add(item);
                    }
                }
            }
            return match;
        }
    }
}
