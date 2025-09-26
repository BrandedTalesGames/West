// Path: Assets/West/Core/Config/IDebugConfig.cs
// Assembly: West.Core
// Namespace: West.Core.Config
#nullable enable
namespace West.Core.Config
{
    /// <summary>Read-only debug flags and time overrides for dev/tests.</summary>
    public interface IDebugConfig
    {
        bool EnableDebug { get; }
        bool OverrideTime { get; }
        int  TickRateHzOverride { get; }          // e.g., 30, 60, 120 for tests
        int  MaxCatchUpPerFrameOverride { get; }  // e.g., 4
        int  StartSpeedIndex { get; }             // 0=Pause, 1=1x, etc.
        bool PauseOnPlay { get; }
        int  FastForwardTicksOnPlay { get; }      // 0 for none
    }
}
