// Path: Assets/West/Core/Time/TickSystem.cs
// Assembly: West.Core
// Namespace: West.Core.Time
// Summary: Central fixed-step driver. Accumulates render time and advances sim ticks with speed multipliers,
// clamping to a max catch-up per frame. Emits FixedSimTickEvent then LateSimTickEvent for each step.

#nullable enable
using UnityEngine;

namespace West.Core.Time
{
    /// <summary>
    /// Fixed-step simulation driver. Pure logic lives here; a tiny TickDriver MonoBehaviour calls StepFromRender().
    /// </summary>
    public sealed class TickSystem
    {
        private readonly EventBus _bus;
        private readonly TimeService _time;
        private readonly TimeConfigRuntime _cfg;

        private float _accumulator;
        private float _secondsPerTick;
        private int _maxCatchUpPerFrame;

        private long _pendingFastForward; // from debug config, consumed across frames

        public TickSystem(EventBus bus, TimeService time, TimeConfigRuntime cfg)
        {
            _bus = bus;
            _time = time;
            _cfg = cfg;

            _secondsPerTick = 1f / Mathf.Max(1, cfg.TickRateHz);
            _maxCatchUpPerFrame = cfg.MaxCatchUpPerFrame;
        }

        /// <summary>
        /// Apply debug overrides (Editor/Dev only), e.g., rate/catch-up/speed/pause/fast-forward.
        /// </summary>
        public void ApplyDebugOverrides(IDebugHooks? dbg)
        {
            if (dbg == null || !dbg.Enable) return;

            if (dbg.OverrideTickRateHz.HasValue)
                _secondsPerTick = 1f / Mathf.Max(1, dbg.OverrideTickRateHz.Value);

            if (dbg.OverrideMaxCatchUp.HasValue)
                _maxCatchUpPerFrame = Mathf.Max(1, dbg.OverrideMaxCatchUp.Value);

            if (dbg.StartSpeedIndex.HasValue)
                _time.SetSpeedIndex(dbg.StartSpeedIndex.Value);

            if (dbg.PauseOnPlay) _time.SetSpeedIndex(0);

            _pendingFastForward = dbg.FastForwardTicksOnPlay;
        }

        /// <summary>Advance simulation based on render delta time (unscaled). Call once per frame from driver.</summary>
        public void StepFromRender(float unscaledDeltaTime)
        {
            // Accumulate real time scaled by current speed multiplier.
            var speedMul = _time.SpeedMultiplier;
            if (speedMul <= 0)
            {
                // Still consume any scheduled fast-forward while paused (if designer asked for it).
                ConsumeFastForwardBudget(_maxCatchUpPerFrame);
                return;
            }

            _accumulator += unscaledDeltaTime * speedMul;

            // Process pending fast-forward ticks first (if any), capped per frame.
            int frameBudget = _maxCatchUpPerFrame;
            frameBudget -= ConsumeFastForwardBudget(frameBudget);
            if (frameBudget <= 0) return;

            // Convert accumulated seconds to discrete steps.
            int steps = Mathf.Min(frameBudget, Mathf.FloorToInt(_accumulator / _secondsPerTick));
            if (steps <= 0) return;

            _accumulator -= steps * _secondsPerTick;

            // Run steps
            for (int i = 0; i < steps; i++)
            {
                _time.AdvanceOneTick();                           // pure state change
                _bus.Publish(new FixedSimTickEvent(_time.TotalTicks)); // pre-commit work window

                // Commit boundary — publish after-the-fact events here
                _bus.Publish(new LateSimTickEvent(_time.TotalTicks));
            }
        }

        private int ConsumeFastForwardBudget(int frameBudget)
        {
            if (_pendingFastForward <= 0 || frameBudget <= 0) return 0;
            int toRun = (int)Mathf.Min(_pendingFastForward, frameBudget);
            for (int i = 0; i < toRun; i++)
            {
                _time.AdvanceOneTick();
                _bus.Publish(new FixedSimTickEvent(_time.TotalTicks));
                _bus.Publish(new LateSimTickEvent(_time.TotalTicks));
            }
            _pendingFastForward -= toRun;
            return toRun;
        }
    }

    /// <summary>
    /// Simple interface so we can inject debug hooks from your DebugConfig seam without hard-coupling.
    /// </summary>
    public interface IDebugHooks
    {
        bool Enable { get; }
        int? OverrideTickRateHz { get; }
        int? OverrideMaxCatchUp { get; }
        int? StartSpeedIndex { get; }
        bool PauseOnPlay { get; }
        int FastForwardTicksOnPlay { get; }
    }
}
