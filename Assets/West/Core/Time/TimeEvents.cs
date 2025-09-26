// Path: Assets/West/Core/Time/TimeEvents.cs
// Assembly: West.Core
// Namespace: West.Core.Time
// Summary: Event payloads for the fixed-tick pipeline. Published via EventBus at well-defined points.

#nullable enable
namespace West.Core.Time
{
    /// <summary>Raised for each fixed sim step (pre-commit work happens here).</summary>
    public readonly struct FixedSimTickEvent
    {
        public readonly long Tick;
        public FixedSimTickEvent(long tick) { Tick = tick; }
        public override string ToString() => $"FixedSimTick {Tick}";
    }

    /// <summary>Raised immediately after FixedSimTick processing to signal Commit boundary.</summary>
    public readonly struct LateSimTickEvent
    {
        public readonly long Tick;
        public LateSimTickEvent(long tick) { Tick = tick; }
        public override string ToString() => $"LateSimTick {Tick}";
    }

    /// <summary>Raised when gameplay speed slot changes (e.g., Pause/1x/2x...).</summary>
    public readonly struct GameSpeedChangedEvent
    {
        public readonly int SpeedIndex;
        public GameSpeedChangedEvent(int speedIndex) { SpeedIndex = speedIndex; }
        public override string ToString() => $"GameSpeedChanged {SpeedIndex}";
    }
}
