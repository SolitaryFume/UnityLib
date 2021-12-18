using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityLib
{
    public static class List_Extend
    {
        public static bool SoltAdd<T>(this List<T> self, T item)
        {
            var index = self.BinarySearch(item);
            index = index>0?index:-index;
            self.Insert(index,item);
            return true;
        }
    }
}
