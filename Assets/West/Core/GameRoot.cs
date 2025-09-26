// Path: Assets/West/Core/GameRoot.cs
// Assembly: West.Core
// Namespace: West.Core
// Purpose: The only MonoBehaviour in sc_Core_Boot. Initializes the ServiceRegistry, runs Bootstrap, and persists across scene loads.

#nullable enable
using System.Threading.Tasks;
using UnityEngine;

namespace West.Core
{
    /// <summary>
    /// Root behaviour for the boot scene. Lives in sc_Core_Boot. Kicks off Bootstrap and survives scene loads.
    /// </summary>
    public sealed class GameRoot : MonoBehaviour
    {
        private async void Awake()
        {
            // Ensure this survives into additively loaded scenes (AD §5).
            DontDestroyOnLoad(gameObject);

            // Init service registry (clears on domain reload).
            ServiceRegistry.Init();

            // Run the bootstrap pipeline (loads World + HUD).
            await Bootstrap.RunAsync();
        }
    }
}
