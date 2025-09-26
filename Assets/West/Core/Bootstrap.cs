// Path: Assets/West/Core/Bootstrap.cs
// Assembly: West.Core
// Namespace: West.Core
// Summary: Now ingests TimeConfig, registers TimeService, TimeConfigRuntime, and sets up TickDriver.

#nullable enable
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using West.Core.Time;

namespace West.Core
{
    public static class Bootstrap
    {
        public static async Task RunAsync()
        {
            // 1) Core services
            ServiceRegistry.Register(new EventBus());

            // 2) Time config ingest
            var src = Resources.Load<TimeConfig>("TimeConfig"); // place an instance under Assets/Resources/TimeConfig.asset
            if (src == null)
            {
                Debug.LogWarning("[Boot] No TimeConfig found in Resources; using defaults.");
                src = ScriptableObject.CreateInstance<TimeConfig>();
            }
            var cfg = new TimeConfigRuntime(src);
            ServiceRegistry.Register(cfg);

            // 3) Time service (holds clock)
            var timeSvc = new TimeService(cfg.SpeedMultipliers, cfg.TicksPerDay);
            ServiceRegistry.Register(timeSvc);

            // 4) Optional debug seam (Editor/Dev builds)
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            var debugCfg = ServiceRegistry.TryGet<West.Core.Config.IDebugConfig>();
            if (debugCfg != null)
                ServiceRegistry.Register<IDebugHooks>(new DebugHooksAdapter(debugCfg));
#endif

            // 5) Tick driver (bridges render loop)
            // Create a persistent driver under GameRoot at runtime
            var rootGo = GameObject.Find("/GameRoot") ?? new GameObject("GameRoot");
            Object.DontDestroyOnLoad(rootGo);
            if (rootGo.GetComponent<TickDriver>() == null)
                rootGo.AddComponent<TickDriver>();

            // 6) Scenes (same as M1.1; safeguarded loader optional)
            if (!SceneManager.GetSceneByName("sc_World_Main").isLoaded)
                await SceneManager.LoadSceneAsync("sc_World_Main", LoadSceneMode.Additive);
            if (!SceneManager.GetSceneByName("sc_UI_HUD").isLoaded)
                await SceneManager.LoadSceneAsync("sc_UI_HUD", LoadSceneMode.Additive);
        }
    }
}
