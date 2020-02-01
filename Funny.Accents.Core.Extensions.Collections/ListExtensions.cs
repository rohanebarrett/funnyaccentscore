using System;
using System.Collections.Generic;
using System.Linq;

namespace Funny.Accents.Core.Collections
{
    public static class ListExtensions
    {
        public static T RemoveAndGet<T>(this IList<T> list, int index)
        {
            lock (list)
            {
                var value = list[index];
                list.RemoveAt(index);
                return value;
            }
        }/*End of RemoveAndGet method*/

        public static IEnumerable<T> RemoveWhile<T>(this IList<T> list,
            Func<T, bool> predicate, bool remove)
        {
            lock (list)
            {
                var value = list.TakeWhile(predicate).ToList();

                if (!remove) { return value; }

                for (var i = 0; i < value.Count; i++)
                {
                    list.RemoveAt(0);
                }

                return value;
            }
        }/*End of RemoveWhile method*/

        public static IEnumerable<T> RemoveWhile<T>(this IList<T> list,
            Func<T, bool> predicate)
        {
            lock (list)
            {
                var tempList = new List<T>();
                if (list.Count <= 0) { return tempList; }

                var results = true;

                while (results)
                {
                    results = predicate.Invoke(list[0]);

                    if (!results) { return tempList; }

                    tempList.Add(list[0]);
                    list.RemoveAt(0);

                    results = list.Count > 0;
                }

                return tempList;
            }
        }/*End of RemoveWhile method*/
    }/*End of ListExtensions class*/
}/*End of Funny.Accents.Core.Collections namepsace*/
