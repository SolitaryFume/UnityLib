using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityLib
{
    public static class List_Extend
    {
        public static bool SoltAdd<T>(this List<T> self, T item)
            where T : IComparable<T>
        {
            if (item == null)
                return false;
            for (int i = 0; i < self.Count; i++)
            {
                if (self[i].CompareTo(item) > 0)
                {
                    self.Insert(i, item);
                    return true;
                }
            }
            self.Add(item);
            return true;
        }
    }
}
