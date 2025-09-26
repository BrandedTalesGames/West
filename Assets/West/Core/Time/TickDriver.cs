// Path: Assets/West/Core/Time/TickDriver.cs
// Assembly: West.Core
// Namespace: West.Core.Time
// Summary: Tiny MonoBehaviour that bridges Unity's render loop to TickSystem.StepFromRender().
// Lives on GameRoot (DontDestroyOnLoad).

#nullable enable
using UnityEngine;

namespace West.Core.Time
{
    /// <summary>Unity driver for the TickSystem.</summary>
    [DisallowMultipleComponent]
    public sealed class TickDriver : MonoBehaviour
    {
        private TickSystem? _tick;

        private void Awake()
        {
            // Pull services
            var bus = ServiceRegistry.Get<EventBus>();
            var timeSvc = ServiceRegistry.Get<TimeService>();
            var cfg = ServiceRegistry.Get<TimeConfigRuntime>();

            _tick = new TickSystem(bus, timeSvc, cfg);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            // Try to get any debug seam the project registered
            var dbg = ServiceRegistry.TryGet<IDebugHooks>();
            _tick.ApplyDebugOverrides(dbg);
#endif
        }

        private void Update()
        {
            _tick?.StepFromRender(UnityEngine.Time.unscaledDeltaTime);
        }
    }
}
