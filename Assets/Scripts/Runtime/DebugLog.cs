using UnityEngine;
using West.Data;

namespace West.Runtime
{
    /// <summary>
    /// Channels for selective debug output. Add channels as you add Systems.
    /// </summary>
    public enum DebugChannel { Time, Movement, Jobs, Weather }

    /// <summary>
    /// Centralized debug logging that checks GlobalConfig toggles.
    /// </summary>
    public static class DebugLog
    {
        private static GlobalConfig _cfg;

        /// <summary>
        /// Provide the active GlobalConfig (once, at startup).
        /// </summary>
        public static void Init(GlobalConfig cfg)
        {
            _cfg = cfg;
        }

        /// <summary>
        /// Conditional log: only prints if the channel is enabled in GlobalConfig.
        /// </summary>
        public static void Log(DebugChannel channel, string message, Object ctx = null)
        {
            if (_cfg == null) { Debug.LogWarning("[DebugLog] GlobalConfig not set."); return; }
            if (_cfg.IsDebugEnabled(channel))
            {
                if (ctx != null) Debug.Log($"[{channel}] {message}", ctx);
                else Debug.Log($"[{channel}] {message}");
            }
        }

        /// <summary>
        /// Unconditional important warning (still allowed even if debug is off).
        /// </summary>
        public static void Warn(string message, Object ctx = null)
        {
            if (ctx != null) Debug.LogWarning(message, ctx);
            else Debug.LogWarning(message);
        }

        /// <summary>
        /// Unconditional error (still allowed even if debug is off).
        /// </summary>
        public static void Error(string message, Object ctx = null)
        {
            if (ctx != null) Debug.LogError(message, ctx);
            else Debug.LogError(message);
        }
    }
}
