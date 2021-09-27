using System.Collections.Generic;

namespace UnityLib.Data
{
    public struct LanguageCfg : IConfig<int>
    {
        private int id;
        private string content;
        public int Id => id;
        public string Content => content;
    }
}