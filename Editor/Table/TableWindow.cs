using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;

public class TableWindow : EditorWindow
{
    [Serializable]
    public class TableEditorCfg
    {
        public TableEditorCfg()
        {
            ExcelPath = Application.dataPath;
            SaveDataPath = Application.dataPath;
        }

        public string ExcelPath;
        public string SaveDataPath;

        public TableItemCfg[] TableItemCfg;
    }

    [Serializable]
    public class TableItemCfg
    {
        public string excel;
        public TextAsset data;
        public string type;
    }

    [MenuItem("Window/Table")]
    public static void OpenWindow()
    {
        var window = EditorWindow.GetWindow<TableWindow>();
        window.Show();
    }

    public static TableEditorCfg cfg;

    private void OnGUI()
    {
        if (cfg == null)
        {
            var json = EditorPrefs.GetString(nameof(TableEditorCfg), string.Empty);
            cfg = JsonUtility.FromJson<TableEditorCfg>(json);
            if (cfg == null)
            { 
                cfg= new TableEditorCfg();
            }
        
        }
        GUILayout.BeginHorizontal("选择ExcelPath",GUILayout.Height(30));
        cfg.ExcelPath = EditorGUILayout.TextField("ExcelPath:", cfg.ExcelPath, GUILayout.MinWidth(500));
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("选择目录",GUILayout.Width(100)))
        {
            var temp = EditorUtility.OpenFolderPanel("选择导出目录", cfg.ExcelPath, "");
            if (!string.IsNullOrEmpty(temp))
            {
                cfg.ExcelPath = temp;
                SaveSetting();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal("选择SavePath", GUILayout.Height(30));
        cfg.SaveDataPath = EditorGUILayout.TextField("SavePath:", cfg.SaveDataPath, GUILayout.MinWidth(500));
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("选择目录", GUILayout.Width(100)))
        {
            var temp = EditorUtility.OpenFolderPanel("选择导出目录", cfg.SaveDataPath, "");
            if (!string.IsNullOrEmpty(temp))
            {
                cfg.SaveDataPath = temp;
                SaveSetting();
            }
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal("选择SavePath", GUILayout.Height(30));
        cfg.SaveDataPath = EditorGUILayout.TextField("SavePath:", cfg.SaveDataPath, GUILayout.MinWidth(500));
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("选择目录", GUILayout.Width(100)))
        {
            var temp = EditorUtility.OpenFolderPanel("选择导出目录", cfg.SaveDataPath, "");
            if (!string.IsNullOrEmpty(temp))
            {
                cfg.SaveDataPath = temp;
                SaveSetting();
            }
        }
        GUILayout.EndHorizontal();
    }

    private void SaveSetting()
    {
        var json = JsonUtility.ToJson(cfg);
        EditorPrefs.SetString(nameof(TableEditorCfg), json);
    }
}
