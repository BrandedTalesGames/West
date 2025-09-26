// Path: Assets/West/Data/Configs/DebugConfig.cs
// Assembly: West.Data
// Namespace: West.Data
// NOTE: Authoring-only; do not mutate at runtime. Lives in West.Data per AD.

#nullable enable
using UnityEngine;

namespace West.Data
{
    // If you still get CS1739 after moving to West.Data, try the minimal attribute [CreateAssetMenu]
    // as a temporary fallback. But Unity 2021+ supports menuName/fileName/order.
    [CreateAssetMenu(fileName = "DebugConfig", menuName = "West/Configs/Debug Config", order = 0)]
    public sealed class DebugConfig : ScriptableObject
    {
        [Header("Gates")]
        public bool enableDebug = true;
        public bool overrideTime = true;

        [Header("Time Overrides")]
        [Min(1)] public int tickRateHzOverride = 30;
        [Min(1)] public int maxCatchUpPerFrameOverride = 4;
        [Range(0,5)] public int startSpeedIndex = 1;
        public bool pauseOnPlay = false;
        [Min(0)] public int fastForwardTicksOnPlay = 0;
    }
}
