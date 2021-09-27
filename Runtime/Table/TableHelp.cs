using System;
using System.Runtime.InteropServices;

namespace UnityLib.Data
{
    public static class TableUtility
    {
        public static T[] AnalysisData<T>(byte[] data) 
            where T : struct
        { 
            if(data==null)
                throw new ArgumentNullException("data is a null !");

            var size = Marshal.SizeOf<T>();
            var l = data.Length / size; 
            var result = new T[l];
            for (int i = 0; i < l; i++)
            {
                var p = Marshal.UnsafeAddrOfPinnedArrayElement<byte>(data, i * size);
                result[i] = Marshal.PtrToStructure<T>(p);
            }
            return result;
        }
    }
}

