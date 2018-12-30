using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicBuilder.RulesDirector
{
    public static class ListConverter<T>
    {
        public static T[] ToArray(IList<T> list)
        {
            if (list.GetType() == typeof(T[]))
                return (T[])list;

            return list.ToArray();
        }

        public static List<T> ToList(IList<T> list)
        {
            if (list.GetType() == typeof(List<T>))
                return (List<T>)list;

            return new List<T>(list);
        }
    }
}
