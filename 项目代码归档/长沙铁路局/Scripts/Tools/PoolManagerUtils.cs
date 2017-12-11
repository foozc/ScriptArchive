using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Scripts.Tools
{
    public static class PoolManagerUtils
    {
        internal static void SetActive(GameObject obj, bool state)
        {
            obj.SetActive(state);
        }
        internal static bool activeInHierarchy(GameObject obj)
        {
            return obj.activeInHierarchy;
        }
    }
}
