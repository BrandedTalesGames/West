// Path: Assets/West/Core/Time/TimeConfig.cs
// Assembly: West.Core
// Namespace: West.Core.Time
// Summary: Authoring ScriptableObject for time/tick defaults. Ingested at boot into a runtime DTO;
// never mutated at runtime. Holds tick rate, ticks/day, speed slots, and catch-up cap.

#nullable enable
using UnityEngine;

namespace West.Core.Time
{
    /// <summary>
    /// Authoring-time time configuration. Place an instance in Resources as "TimeConfig" for M1.2.
    /// </summary>
    [CreateAssetMenu(menuName = "West/Configs/Time Config", fileName = "TimeConfig")]
    public sealed class TimeConfig : ScriptableObject
    {
        [Header("Tick Model")]
        [Tooltip("Fixed simulation tick rate in Hz (default 30).")]
        [Min(1)] public int tickRateHz = 30;

        [Tooltip("Number of discrete sim ticks in one in-world day.")]
        [Min(1)] public int ticksPerDay = 2880; // 48 ticks/min @ 30Hz gives ~60 seconds per in-world minute; adjust later if AD differs.

        [Header("Speed Slots (index-based)")]
        [Tooltip("Mapping for SpeedIndex → multiplier; index 0 must be 0 for Pause.")]
        public int[] speedMultipliers = { 0, 1, 2, 3, 5, 10 }; // 10x is dev-only; HUD exposes up to 5x in normal builds.

        [Header("Catch-up")]
        [Tooltip("Max number of sim ticks processed per render frame (prevents hitches).")]
        [Min(1)] public int maxCatchUpPerFrame = 4;
    }

    /// <summary>
    /// Runtime copy of the config used by systems (plain DTO, no Unity refs).
    /// </summary>
    public sealed class TimeConfigRuntime
    {
        public readonly int TickRateHz;
        public readonly int TicksPerDay;
        public readonly int[] SpeedMultipliers;
        public readonly int MaxCatchUpPerFrame;

        public TimeConfigRuntime(TimeConfig src)
        {
            TickRateHz = Mathf.Max(1, src.tickRateHz);
            TicksPerDay = Mathf.Max(1, src.ticksPerDay);
            SpeedMultipliers = (int[])src.speedMultipliers.Clone();
            MaxCatchUpPerFrame = Mathf.Max(1, src.maxCatchUpPerFrame);
        }
    }
}
