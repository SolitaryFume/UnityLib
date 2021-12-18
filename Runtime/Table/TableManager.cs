namespace UnityLib.Data
{
    public class TableManager
    {
        private static TableManager _instance;
        public static TableManager Instance => _instance ?? (_instance = new TableManager());
    }
}
