using System;
using System.Runtime.InteropServices;

namespace UnityLib.Data
{
    public class LanguageTable : Table<int, LanguageCfg>
    {
        public LanguageTable(LanguageCfg[] array) : base(array)
        {
        }

        public LanguageType Local { get; set; } 
    }

    public enum LanguageType
    {
        zh_cn,
    }

    public class TableManager
    {
        private static TableManager _instance;
        public static TableManager Instance => _instance ?? (_instance = new TableManager());

        //public LanguageTable LanguageTb { get; } = new LanguageTable();
    }
}
