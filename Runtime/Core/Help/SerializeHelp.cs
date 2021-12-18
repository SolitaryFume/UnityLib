using System;
using System.Runtime.InteropServices;

namespace UnityLib
{
    public static class SerializeHelp
    {
        public static byte[] StructToBytes<T>(in T structure)
        {
            var size = Marshal.SizeOf<T>();
            var buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, buffer, false);
                byte[] result = new byte[size];
                Marshal.Copy(buffer, result, 0, size);
                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public static bool BytesToStruct<T>(byte[] bytes, out T structure)
        {
            var size = Marshal.SizeOf<T>();
            var buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                structure = Marshal.PtrToStructure<T>(buffer);
                return true;
            }
            catch (Exception ex)
            {
                structure = default(T);
                Debug.LogException(ex);
                return false;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public static T[] BytesToStructArray<T>(byte[] bytes)
        {
            var size = Marshal.SizeOf<T>();
            var count = bytes.Length / size;
            var array = new T[count];
            for (var i = 0; i < count; i++)
            {
                var p = Marshal.UnsafeAddrOfPinnedArrayElement<byte>(bytes, i * size);
                try
                {
                    array[i] = Marshal.PtrToStructure<T>(p);
                }
                finally
                {
                    Marshal.FreeHGlobal(p);
                }
            }
            return array;
        }

        public static byte[] StructArrayToBytes<T>(T[] array)
        {
            var size = Marshal.SizeOf<T>();
            var buffer = Marshal.AllocHGlobal(size);
            byte[] result = new byte[size * array.Length];
            try
            {
                for (int i = 0; i < array.Length; i++)
                {
                    var structure = array[i];
                    Marshal.StructureToPtr(structure, buffer, false);
                    Marshal.Copy(buffer, result, i * size, size);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
            return result;
        }
    }
}

