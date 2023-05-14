using UnityEngine;

namespace Kit.Utility
{
    public static class LogExtension
    {
        public static void Log(this object any, object data) => Debug.Log($"[{any.GetType().Name}] {data}");
    }
}