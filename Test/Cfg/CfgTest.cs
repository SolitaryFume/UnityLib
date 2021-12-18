using NPOI.SS.UserModel;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityLib.Data;
using System.Linq;
using System.Runtime.Serialization;

public enum A:int
{ 
    A = 10,
    B,
    C,
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class TestCfg : IConfig<int>
{
    [SerializeField] public int id;
    [SerializeField] public byte count;
    [SerializeField] public bool tbool;
    [SerializeField] public A aenum;
    // [MarshalAs(UnmanagedType.ByValArray,SizeConst = 2)]
    // [SerializeField] public byte[] arr;

    public int Id => id;

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}


public class TestTb : AbstractFileTable<int, TestCfg>
{
    public TestTb(string filePtah,TableOption option) : base(filePtah,option)
    {
    }
}

public class CfgTest_Frame
{
    // [Test]
    public void TableTest()
    {
        ReadXLS();
        ReadData();
    }

    // [Test]
    public void ReadXLS()
    {
        var path = @"H:\共享\Test.xls";
        var savePath = @"H:\共享\Test.data";
        UnityLibEditor.ExcelParseHelp.Parse<TestCfg>(path, savePath);
    }

    // [Test]
    public void ReadData()
    {
        var savePath = @"H:\共享\Test.data";
        var data = File.ReadAllBytes(savePath);
        var arr = TableUtility.AnalysisData<TestCfg>(data);
    }

    [Test]
    public void FileTableWrite()
    {
        var path = @"C:\Git\tb.data";
        var list = new List<TestCfg>(){
            new TestCfg(){
                id = 1,
                aenum = A.A,
            },
            new TestCfg(){
                id = 2,
                aenum = A.B
            },
            new TestCfg()
            {
                id = 3,
                aenum = A.C
            }
        };
        File.Delete(path);
        var fileSteam = File.Create(path);
        foreach (var item in list)
        {
            var data = UnityLib.SerializeHelp.StructToBytes(item);
            fileSteam.Write(data,0,data.Length);
            
        }
        fileSteam.Close();
    }

    [Test]
    public void FileTableRead()
    {
        var path = @"C:\Git\tb.data";
        var db = new TestTb(path,TableOption.AutoSave);
        var cfg = db[1];
        Debug.Log(cfg.ToString());
    }
};