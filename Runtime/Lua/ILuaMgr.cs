using System;
using UnityEngine;

namespace UnityLib.Lua
{
    public interface ILuaMgr:IDisposable
    {
        object[] DoString(string code);
        object[] Require(string file);
        byte[] Loader(ref string key);
        void GC();
        void Tick();
    }
}

