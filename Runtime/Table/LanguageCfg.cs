namespace UnityLib.Data
{
    public class LanguageCfg : IConfig<int>
    {
        private int id;
        private string content;
        public int Id => id;
        public string Content => content;
    }
}