// Path: Assets/West/Core/Time/DebugHooksAdapter.cs
// Assembly: West.Core
// Namespace: West.Core.Time
// Summary: Adapts your existing DebugConfig service/seam to TickSystem.IDebugHooks.

#nullable enable
namespace West.Core.Time
{
    public sealed class DebugHooksAdapter : IDebugHooks
    {
        private readonly West.Core.Config.IDebugConfig _cfg;
        public DebugHooksAdapter(West.Core.Config.IDebugConfig cfg) { _cfg = cfg; }

        public bool Enable => _cfg.EnableDebug && _cfg.OverrideTime;
        public int? OverrideTickRateHz => _cfg.TickRateHzOverride;
        public int? OverrideMaxCatchUp => _cfg.MaxCatchUpPerFrameOverride;
        public int? StartSpeedIndex => _cfg.StartSpeedIndex;
        public bool PauseOnPlay => _cfg.PauseOnPlay;
        public int FastForwardTicksOnPlay => _cfg.FastForwardTicksOnPlay;
    }
}
