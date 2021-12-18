using System;
using System.Runtime.InteropServices;

namespace UnityLib.Data
{
    public class LanguageTable : AbstractTable<int, LanguageCfg>
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
}
