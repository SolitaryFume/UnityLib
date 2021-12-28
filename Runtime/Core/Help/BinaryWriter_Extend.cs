using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace UnityLib
{
    public static class BinaryWriter_Extend
    {
        public static void Write(this BinaryWriter writer, Vector2 value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
        }
        public static void Write(this BinaryWriter writer, Vector3 value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
        }
        public static void Write(this BinaryWriter writer, Vector4 value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
            writer.Write(value.w);
        }
        public static void Write(this BinaryWriter writer, Quaternion value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
            writer.Write(value.w);
        }
        public static void Write(this BinaryWriter writer, Vector2Int value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
        }
        public static void Write(this BinaryWriter writer, Vector3Int value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
        }

        public static void Write(this BinaryWriter writer, Color value)
        {
            writer.Write(value.r);
            writer.Write(value.g);
            writer.Write(value.b);
            writer.Write(value.a);
        }

        public static void Write(this BinaryWriter writer,Color32 value)
        {
            writer.Write(value.r);
            writer.Write(value.g);
            writer.Write(value.b);
            writer.Write(value.a);
        }

        public static void Write(this BinaryWriter writer, Transform value)
        {
            if (value == null)
                throw new ArgumentException("value");
            writer.Write(value.position);
            writer.Write(value.rotation);
            writer.Write(value.localScale);
        }

        public static void Write(this BinaryWriter writer, AnimationCurve value)
        {
            if (value == null)
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
            }

            writer.Write((int)value.preWrapMode);
            writer.Write((int)value.postWrapMode);
            writer.Write(value.length);

            for (int i = 0; i < value.length; i++)
            {
                Keyframe keyframe = value.keys[i];
                writer.Write(keyframe);
            }
        }

        public static void Write(this BinaryWriter writer, Keyframe value)
        {
            writer.Write(value.time);
            writer.Write(value.value);
            writer.Write(value.inTangent);
            writer.Write(value.outTangent);
            writer.Write(value.inWeight);
            writer.Write(value.outWeight);
            writer.Write((int)value.weightedMode);
        }

        public static void Write(this BinaryWriter writer, MinMaxCurve value)
        {
            writer.Write(value.constant);
            writer.Write(value.constantMax);
            writer.Write(value.constantMin);
            writer.Write(value.curve);
            writer.Write(value.curveMax);
            writer.Write(value.curveMin);
            writer.Write(value.curveMultiplier);
            //writer.Write(value.curveScalar);
        }

        //TODO 序列化粒子系统
        //public static void Write(this BinaryWriter writer, ParticleSystem value) 
        //{
        //    if (value == null)
        //        throw new ArgumentException("value");
        //    writer.Write(value.main);
        //}

        //public static void Write(this BinaryWriter writer, ParticleSystem.MainModule module)
        //{
        //    writer.Write(module.duration);
        //    writer.Write(module.loop);
        //    writer.Write(module.prewarm);
        //}

        //private static List<Vector3> _v3cache;
        //public static List<Vector3> v3cache
        //{
        //    get
        //    {
        //        if (_v3cache == null)
        //            _v3cache = new List<Vector3>();
        //        return _v3cache;
        //    }
        //}
        public static void Write(this BinaryWriter writer, Mesh mesh)
        {
            if (mesh == null)
            {
                writer.Write(false);
                return;
            }
            else
            {
                writer.Write(true);
            }
            Write(writer, mesh.vertices);
            Write(writer, mesh.triangles);
            Write(writer, mesh.uv);
            Write(writer, mesh.colors);
        }

        public static void Write(this BinaryWriter writer, Vector3[] array)
        {
            if (array == null)
            { 
                writer.Write(true);
                return;
            }
            writer.Write(false);
            writer.Write(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                writer.Write(array[i]);
            }
        }

        public static void Write(this BinaryWriter writer, Vector2[] array)
        {
            if (array == null)
            {
                writer.Write(true);
                return;
            }
            writer.Write(false);
            writer.Write(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                writer.Write(array[i]);
            }
        }

        public static void Write(this BinaryWriter writer, int[] array)
        {
            if (array == null)
            {
                writer.Write(true);
                return;
            }
            writer.Write(false);
            writer.Write(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                writer.Write(array[i]);
            }
        }

        public static void Write(this BinaryWriter writer, Color[] array)
        {
            if (array == null)
            {
                writer.Write(true);
                return;
            }
            writer.Write(false);
            writer.Write(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                writer.Write(array[i]);
            }
        }

        #region 保留方案
        public static void Write<T>(this BinaryWriter writer, T value)
            where T : struct
        {
            var bytes = SerializeHelp.StructToBytes(value);
            writer.Write(bytes);
        }

        public static void Write<T>(this BinaryWriter writer, T[] array)
            where T : struct
        {
            if (array == null)
            {
                writer.Write(true);
                return;
            }
            writer.Write(false);
            writer.Write(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                writer.Write<T>(array[i]);
            }
        }
        #endregion
    }
}

