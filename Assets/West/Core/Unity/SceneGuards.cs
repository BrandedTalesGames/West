// Path: Assets/West/Core/Unity/SceneGuards.cs
// Assembly: West.Core
// Namespace: West.Core.Unity
// Purpose: Utilities to check if a scene is present in the active build scene list and to load it safely.

#nullable enable
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace West.Core.Unity
{
    /// <summary>Helpers to validate and load scenes robustly.</summary>
    public static class SceneGuards
    {
        /// <summary>
        /// Returns true if a scene name (e.g., "sc_World_Main") exists in the current build scene list.
        /// </summary>
        public static bool IsSceneInBuild(string sceneName, out string scenePath)
        {
            int count = SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < count; i++)
            {
                var path = SceneUtility.GetScenePathByBuildIndex(i);
                if (string.Equals(Path.GetFileNameWithoutExtension(path), sceneName))
                {
                    scenePath = path;
                    return true;
                }
            }
            scenePath = string.Empty;
            return false;
        }

        /// <summary>
        /// Loads a scene additively if it is present in the build scene list. Logs a clear error if missing.
        /// Never throws; returns false on failure.
        /// </summary>
        public static async Task<bool> LoadAdditiveIfListed(string sceneName)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
                return true;

            if (!IsSceneInBuild(sceneName, out var path))
            {
                Debug.LogError(
                    $"[Boot] Scene '{sceneName}' is not in the active Build Profile / Build Settings. " +
                    $"Add it via File→Build Profiles… (or Build Settings).");
                return false;
            }

            var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (op == null)
            {
                Debug.LogError($"[Boot] Failed to start loading scene '{sceneName}' (AsyncOperation was null).");
                return false;
            }

            await op.AsTask(); // Await completion
            return SceneManager.GetSceneByName(sceneName).isLoaded;
        }
    }
}
