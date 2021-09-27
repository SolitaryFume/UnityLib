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

public enum A
{ 
    A,
    B,
    C,
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct TestCfg : IConfig<int>
{
    [SerializeField] private int id;
    [SerializeField] private byte count;
    [SerializeField] private bool tbool;
    [SerializeField] private A aenum;
    [MarshalAs(UnmanagedType.ByValArray,SizeConst = 2)]
    [SerializeField] private byte[] arr;

    public int Id => id;

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}


public class TestTb : Table<int, TestCfg>
{
    public TestTb(TestCfg[] array) : base(array)
    {
        
    }
}

public class CfgTest_Frame
{
    [Test]
    public void TableTest()
    {
        ReadXLS();
        ReadData();
    }

    [Test]
    public void ReadXLS()
    {
        var path = @"H:\共享\Test.xls";
        var savePath = @"H:\共享\Test.data";
        UnityLibEditor.ExcelParseHelp.Parse<TestCfg>(path, savePath);
    }

    [Test]
    public void ReadData()
    {
        var savePath = @"H:\共享\Test.data";
        var data = File.ReadAllBytes(savePath);
        var arr = TableUtility.AnalysisData<TestCfg>(data);
    }
};