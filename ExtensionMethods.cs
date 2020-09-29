using System;
using System.Collections.Generic;
using System.Text;

namespace DelegatesLambdasEvents
{
    public static class ExtensionMethods
    {
        public static DateTime Combine(this DateTime datePart, DateTime timePart)
        {
            return new DateTime(datePart.Year, datePart.Month, datePart.Day, timePart.Hour, timePart.Minute, timePart.Second);
        }

        public static IEnumerable<int> Where(this IEnumerable<int> input, Func<int,bool> predicate)
        {
            foreach (int t in input)
                if (predicate(t))
                {
                    Console.WriteLine("GOT IT !");
                    yield return t;
                }
                    
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> items, Func<T,bool> gauntlet)
        {
            foreach (T item in items)
                if (gauntlet(item))
                    yield return item;
        }

        //public static IEnumerable<T> Select<T,R>(this IEnumerable<T> items, Func<T,R> transform)
        //{
        //    foreach(T item in items)
        //        yield return transform
        //}
    }
}
