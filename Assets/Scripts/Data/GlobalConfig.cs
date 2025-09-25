using UnityEngine;
using Sirenix.OdinInspector;

namespace West.Data
{
    /// <summary>
    /// Central game configuration and debug switches.
    /// Designers tweak values here; Systems read them at runtime.
    /// Odin creates clean tabs for organization.
    /// </summary>
    [CreateAssetMenu(menuName = "West/Configs/GlobalConfig", fileName = "GlobalConfig")]
    public class GlobalConfig : ScriptableObject
    {
        [TabGroup("Game")]
        [LabelText("Target TPS (Ticks per Second)")]
        [MinValue(1)]
        public int TargetTicksPerSecond = 10;

        [TabGroup("Game")]
        [LabelText("Paused")]
        [ReadOnly]
        public bool IsPaused;

        [TabGroup("Time")]
        [LabelText("Start Day")]
        [MinValue(0)]
        public int StartDay = 0;

        [TabGroup("Time")]
        [LabelText("Minutes Per Tick")]
        [MinValue(1)]
        public int MinutesPerTick = 10;

        [TabGroup("Movement")]
        [LabelText("Base Move Speed (units/sec)")]
        [MinValue(0)]
        public float BaseMoveSpeed = 2f;

        [TabGroup("Debugging")]
        [LabelText("Enable All Debug")]
        public bool Debug_All;

        [TabGroup("Debugging")]
        [LabelText("TimeSystem")]
        public bool Debug_Time;

        [TabGroup("Debugging")]
        [LabelText("MovementSystem")]
        public bool Debug_Movement;

        [TabGroup("Debugging")]
        [LabelText("JobsSystem")]
        public bool Debug_Jobs;

        [TabGroup("Debugging")]
        [LabelText("WeatherSystem")]
        public bool Debug_Weather;

        /// <summary>
        /// Unified gate for system logs to check.
        /// </summary>
        public bool IsDebugEnabled(West.Runtime.DebugChannel channel)
        {
            if (Debug_All) return true;
            return channel switch
            {
                West.Runtime.DebugChannel.Time => Debug_Time,
                West.Runtime.DebugChannel.Movement => Debug_Movement,
                West.Runtime.DebugChannel.Jobs => Debug_Jobs,
                West.Runtime.DebugChannel.Weather => Debug_Weather,
                _ => false
            };
        }
    }
}
