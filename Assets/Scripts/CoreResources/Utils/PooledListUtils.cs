using System.Collections.Generic;
using System.ComponentModel;
using CoreResources.Pool;
using GameResources;

namespace CoreResources.Utils
{
    public static class PooledListUtils
    {
        public static PooledList<T> ToPooledList<T>(this IEnumerable<T> container)
        {
            PooledList<T> temp = AppHandler.AppPool.Get<PooledList<T>>();
            foreach (var variable in container)
            {
                temp.Add(variable);
            }

            return temp;
        }
    }
}