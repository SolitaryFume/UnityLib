using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Data;
using NPOI.SS.UserModel;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityLib.Data;

namespace UnityLibEditor
{

    public class TableException : Exception
    {
        public TableException(string message) : base(message)
        {

        }
    }

    public static class ExcelParseHelp
    {
        public static void Parse<T>(string filePath, string savePath)
            where T : struct
        {
            var dt = ReadExcel(filePath);
            var list = DataTableParse<T>(dt);
            var data = ListToData(list);
            Save(data, savePath);
            Debug.Log($"{typeof(T)} Parse Succeed : from {filePath} >> to {savePath}");
        }

        private static DataTable ReadExcel(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException($"{nameof(filePath)} is null or empty !");

            if (!File.Exists(filePath))
                throw new TableException($"No Find File : {filePath}");

            var dt = new DataTable();
            IWorkbook workbook;
            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = WorkbookFactory.Create(fs);
                fs.Close();
            }

            ISheet sheet = workbook.GetSheetAt(0);
            var rows = sheet.GetRowEnumerator();
            bool isMove = rows.MoveNext();
            if (isMove)
            {
                var heading = rows.Current as IRow; //表头

                var columnNum = 0;
                for (int i = 0; i < heading.LastCellNum; i++)
                {
                    var cell = heading.GetCell(i);
                    if (cell == null)
                        break;

                    columnNum++;
                    var str = cell.ToString();
                    dt.Columns.Add(str);
                }

                while (rows.MoveNext())
                {
                    var row = rows.Current as IRow;
                    var dr = dt.NewRow();
                    for (int i = 0; i < columnNum; i++)
                    {
                        var cell = row.GetCell(i);
                        dr[i] = cell == null ? string.Empty : cell.ToString();
                    }
                    dt.Rows.Add(dr);
                }
                workbook.Close();
                workbook = null;
            }
            return dt;
        }

        private static List<T> DataTableParse<T>(DataTable dt)
        {
            if (dt == null)
                throw new ArgumentNullException("dt is a null !");

            var rowCount = dt.Rows.Count;
            var list = new List<T>();
            var ty = typeof(T);
            var fields = ty.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var obj = Activator.CreateInstance(ty);
                foreach (var property in fields)
                {
                    try
                    {
                        if (property.FieldType.IsArray)
                        {
                            var marshalAs = property.GetCustomAttribute<MarshalAsAttribute>();
                            if (marshalAs == null)
                                throw new TableException($"{ty}.{property.Name}没有MarshalAsAttribute标记");
                            var elementType = property.FieldType.GetElementType();
                            var arr = Array.CreateInstance(elementType, marshalAs.SizeConst);
                            for (int j = 0; j < marshalAs.SizeConst; j++)
                            {
                                var key = $"{property.Name}[{j}]";
                                if (row.IsNull(key))
                                    throw new TableException($"没有对应列{key}");
                                else
                                    arr.SetValue(ChangeType(row[key], elementType), j);
                            }
                            property.SetValue(obj, arr);
                        }
                        else
                        {
                            property.SetValue(obj, ChangeType(row[property.Name], property.FieldType));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new TableException(ex.ToString());
                    }
                }
                list.Add((T)obj);
            }
            return list;
        }

        private static byte[] ListToData<T>(List<T> list)
                where T : struct
        {
            if (list == null)
                throw new ArgumentNullException("list is null !");
            var size = Marshal.SizeOf<T>();
            var l = size * list.Count;
            var result = new byte[l];
            for (int i = 0; i < list.Count; i++)
            {
                Array.Copy(SerializeHelp.StructToBytes<T>(list[i]), 0, result, i * size, size);
            }
            return result;
        }

        private static void Save(byte[] data, string savePath)
        {
            if (string.IsNullOrEmpty(savePath))
                throw new ArgumentNullException("savePath is null or empty !");
            File.WriteAllBytes(savePath, data);
        }

        public static readonly Dictionary<Type, Func<object, Type, object>> CastDir = new Dictionary<Type, Func<object, Type, object>>() { };

        private static object ChangeType(object data, Type target)
        {
            if (target.IsEnum)
            {
                return Enum.Parse(target, data.ToString());
            }
            else
            {
                return Convert.ChangeType(data, target);
            }
        }
    }
}