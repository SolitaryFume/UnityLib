using System;

namespace UnityLib.Data
{
    public class LanguageTable : Tb<int, LanguageCfg>
    {
        private void Init()
        { 
            
        }
    }

    public enum LanguageType
    { 
        zh_cn,
    }

    public class TableManager
    {
        private static TableManager _instance;
        public static TableManager Instance => _instance ?? (_instance = new TableManager());

        public LanguageTable LanguageTb { get; } = new LanguageTable();
    }
}

