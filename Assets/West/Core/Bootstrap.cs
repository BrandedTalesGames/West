// Path: Assets/West/Core/Bootstrap.cs
// Assembly: West.Core
// Namespace: West.Core
// Purpose: Execute strict boot order, register services, and load World + HUD with safety checks.

#nullable enable
using System.Threading.Tasks;
using UnityEngine;
using West.Core.Unity;            // <-- for SceneGuards and AsyncOperationExtensions
using UnityEngine.SceneManagement;

namespace West.Core
{
    /// <summary>
    /// Entry-point bootstrapper. Runs once from GameRoot.Awake(), registers core services, then loads scenes additively.
    /// </summary>
    public static class Bootstrap
    {
        /// <summary>Runs the M1.1 boot subset synchronously/asynchronously as needed.</summary>
        public static async Task RunAsync()
        {
            // Register minimal services (EventBus → TimeService).
            ServiceRegistry.Register(new EventBus());
            ServiceRegistry.Register(new TimeService());

            // Safely load scenes additively. If a scene is missing, we log and keep the app running in Boot.
            bool worldOK = await SceneGuards.LoadAdditiveIfListed("sc_World_Main");
            if (!worldOK)
            {
                Debug.LogError("[Boot] 'sc_World_Main' failed to load. Check Build Profiles / scene list.");
                return; // stop here; HUD depends on World
            }

            bool hudOK = await SceneGuards.LoadAdditiveIfListed("sc_UI_HUD");
            if (!hudOK)
            {
                Debug.LogError("[Boot] 'sc_UI_HUD' failed to load. Check Build Profiles / scene list.");
                // Keep running; world is present even if HUD is missing
            }
        }
    }
}
