using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityLib.Lua
{
    public static class LuaMgr
    {
        static ILuaMgr m_core;

        private static ILuaMgr core
        {
            get
            {
                if (m_core == null)
                {
                    m_core = ServiceMgr.GetService<ILuaMgr>();
                }
                return m_core;
            }
        }

        public static object[] Require(string file)
        {
            return core.Require(file);
        }

        public static object[] DoString(string code)
        {
            return core.DoString(code);
        }

        public static void Disable()
        {
            core?.Dispose();
            m_core = null;
        }
    }
}