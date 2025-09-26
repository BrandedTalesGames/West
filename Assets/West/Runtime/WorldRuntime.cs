// Path: Assets/West/Runtime/WorldRuntime.cs
// Assembly: West.Runtime
// Namespace: West.Runtime
// Purpose: Scene bridge for sc_World_Main. Holds scene refs; no per-frame gameplay logic. Systems will own sim later.

#nullable enable
using UnityEngine;

namespace West.Runtime
{
    /// <summary>
    /// Minimal world-scene bridge. Use this to keep references to camera(s), STE root(s), audio emitters, etc.
    /// </summary>
    public sealed class WorldRuntime : MonoBehaviour
    {
        [Header("Scene Refs (optional for M1.1)")]
        [SerializeField] private Camera _mainCamera = null!;

        private void Reset()
        {
            _mainCamera = Camera.main;
        }
    }
}
