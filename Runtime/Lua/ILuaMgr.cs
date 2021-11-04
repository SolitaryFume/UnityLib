using System;

namespace UnityLib.Lua
{
    public interface ILuaMgr:IDisposable
    {
        object[] DoString(string code);
        object[] Require(string file);
        byte[] Loader(ref string key);
    }
}