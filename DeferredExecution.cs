using System;
using System.Collections.Generic;
using System.Text;

namespace DelegatesLambdasEvents
{
    public static class DeferredExecution
    {

        static IEnumerable<T> Select<T>(this IEnumerable<T> items, Func<T,T> transform)
        {
            Console.WriteLine("Select");
            foreach (T item in items)
            {
                yield return transform(item);
            }
        }
        static IEnumerable<T> Where<T>(this IEnumerable<T> elements,Func<T,bool> gauntlent)
        {
            Console.WriteLine("Where");
            foreach(T item in elements)
            {
                if (gauntlent(item))
                    yield return item;
            }
        }
    

    
    
    
    
    }
}
