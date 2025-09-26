// Path: Assets/West/Tools/Editor/BuildSceneValidator.cs
// Assembly: West.Tools.Editor
// Namespace: West.Tools.Editor
// Purpose: On entering Play Mode from sc_Core_Boot, verify that required scenes are in the active build scene list.

#if UNITY_EDITOR
#nullable enable
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using West.Core.Unity;

namespace West.Tools.Editor
{
    [InitializeOnLoad]
    public static class BuildSceneValidator
    {
        private static readonly string[] RequiredScenes = { "sc_Core_Boot", "sc_World_Main", "sc_UI_HUD" };

        static BuildSceneValidator()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingEditMode) return;

            // Only warn if we're about to run from the Boot scene (common workflow).
            var active = EditorSceneManager.GetActiveScene();
            if (!active.IsValid() || active.name != "sc_Core_Boot") return;

            foreach (var s in RequiredScenes)
            {
                if (!SceneGuards.IsSceneInBuild(s, out _))
                {
                    Debug.LogWarning($"[BuildSceneValidator] Required scene '{s}' is not in the active Build Profile / Build Settings.");
                }
            }
        }
    }
}
#endif
