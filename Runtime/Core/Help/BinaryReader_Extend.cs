using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityLib
{
    public static class BinaryReader_Extend
    {
        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            return new Vector2()
            {
                x = reader.ReadSingle(),
                y = reader.ReadSingle(),
            };
        }

        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            return new Vector3()
            {
                x = reader.ReadSingle(),
                y = reader.ReadSingle(),
                z = reader.ReadSingle(),
            };
        }

        public static Vector4 ReadVector4(this BinaryReader reader)
        {
            return new Vector4()
            {
                x = reader.ReadSingle(),
                y = reader.ReadSingle(),
                w = reader.ReadSingle(),
                z = reader.ReadSingle(),
            };
        }

        public static Vector2Int ReadVector2Int(this BinaryReader reader)
        {
            return new Vector2Int()
            {
                x = reader.ReadInt32(),
                y = reader.ReadInt32(),
            };
        }

        public static Vector3Int ReadVector3Int(this BinaryReader reader)
        {
            return new Vector3Int()
            {
                x = reader.ReadInt32(),
                y = reader.ReadInt32(),
                z = reader.ReadInt32(),
            };
        }

        public static Quaternion ReadQuaternion(this BinaryReader reader)
        {
            return new Quaternion()
            {
                x = reader.ReadSingle(),
                y = reader.ReadSingle(),
                z = reader.ReadSingle(),
                w = reader.ReadSingle(),
            };
        }

        public static Color ReadColor(this BinaryReader reader)
        {
            return new Color()
            {
                r = reader.ReadSingle(),
                g = reader.ReadSingle(),
                b = reader.ReadSingle(),
                a = reader.ReadSingle(),
            };
        }

        public static void Read(this BinaryReader reader, ref Vector2 value)
        {
            value.Set(reader.ReadSingle(), reader.ReadSingle());
        }

        public static void Read(this BinaryReader reader, ref Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            transform.SetPositionAndRotation(reader.ReadVector3(), reader.ReadQuaternion());
            transform.localScale = reader.ReadVector3();
        }

        public static Vector3[] ReadVector3Array(this BinaryReader reader)
        {
            var isnull = reader.ReadBoolean();
            if(isnull)
                return null;
            var l = reader.ReadInt32();
            var array = new Vector3[l];
            for (int i = 0; i < l; i++)
            {
                array[i] = reader.ReadVector3();
            }
            return array;
        }

        public static Vector2[] ReadVector2Array(this BinaryReader reader)
        {
            var isnull = reader.ReadBoolean();
            if (isnull)
                return null;
            var l = reader.ReadInt32();
            var array = new Vector2[l];
            for (int i = 0; i < l; i++)
            {
                array[i] = reader.ReadVector2();
            }
            return array;
        }

        public static int[] ReadIntArray(this BinaryReader reader)
        {
            var isnull = reader.ReadBoolean();
            if (isnull)
                return null;
            var l = reader.ReadInt32();
            var array = new int[l];
            for (int i = 0; i < l; i++)
            {
                array[i] = reader.ReadInt32();
            }
            return array;
        }
        public static Color[] ReadColorArray(this BinaryReader reader)
        {
            var isnull = reader.ReadBoolean();
            if (isnull)
                return null;
            var l = reader.ReadInt32();
            var array = new Color[l];
            for (int i = 0; i < l; i++)
            {
                array[i] = reader.ReadColor();
            }
            return array;
        }

        public static Mesh ReadMesh(this BinaryReader reader)
        { 
            var mesh = new Mesh();
            mesh.vertices = reader.ReadVector3Array();
            mesh.triangles = reader.ReadIntArray();
            mesh.uv = reader.ReadVector2Array();
            mesh.colors = reader.ReadColorArray();
            return mesh;
        }

        #region 保留方案
        public static T Read<T>(this BinaryReader reader)
            where T : struct
        {
            var size = Marshal.SizeOf<T>();
            var buffer = reader.ReadBytes(size);
            if (SerializeHelp.BytesToStruct<T>(buffer, out var result))
                return result;
            else
                throw new InvalidOperationException();

        }

        public static T[] ReadArray<T>(this BinaryReader reader)
            where T : struct
        {
            var isnull = reader.ReadBoolean();
            if (isnull)
                return null;
            var l = reader.ReadInt32();
            var size = Marshal.SizeOf<T>();
            var buffer = reader.ReadBytes(size*l);
            return SerializeHelp.BytesToStructArray<T>(buffer);
        }
        #endregion
    }
}

