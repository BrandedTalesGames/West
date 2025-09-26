// Path: Assets/West/Core/ServiceRegistry.cs
// Assembly: West.Core
// Namespace: West.Core
// Purpose: Minimal, explicit service locator used only for facades per AD §5. Adds "Seal" to lock the registry
// after Bootstrap, plus small test/editor helpers (Has/Remove/TryReplace/DebugList). No third-party types leak.

#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace West.Core
{
    /// <summary>
    /// Simple service registry for registering and retrieving West.* facades.
    /// Per AD §5, used during boot to register services in strict order; consumers call Get&lt;T&gt;().
    /// </summary>
    public static class ServiceRegistry
    {
        // NOTE: Unity's scripting domain reload resets static fields; we re-init in GameRoot/Bootstrap.
        private static readonly Dictionary<Type, object> _services = new();
        private static bool _initialized;
        private static bool _sealed;
        private static string _sealedBy = string.Empty;

        /// <summary>
        /// Initialize the registry. Idempotent. Clears previous registrations (domain reload safety).
        /// </summary>
        public static void Init()
        {
            _services.Clear();
            _initialized = true;
            _sealed = false;
            _sealedBy = string.Empty;
        }

        /// <summary>
        /// Locks the registry against further registrations. Call at end of Bootstrap (AD strict boot order).
        /// </summary>
        /// <param name="reason">Optional note for debugging (e.g., "Bootstrap").</param>
        public static void Seal(string? reason = null)
        {
            _sealed = true;
            _sealedBy = reason ?? "unknown";
        }

        /// <summary>True if <see cref="Seal"/> has been called.</summary>
        public static bool IsSealed => _sealed;

        /// <summary>Registers a service instance of type T. Throws if already registered or registry is sealed.</summary>
        public static void Register<T>(T instance) where T : class
        {
            if (!_initialized) throw new InvalidOperationException("ServiceRegistry.Init() must be called first.");
            if (_sealed) throw new InvalidOperationException($"ServiceRegistry is sealed (by '{_sealedBy}'); cannot register {typeof(T).Name}.");
            if (instance is null) throw new ArgumentNullException(nameof(instance));

            var key = typeof(T);
            if (_services.ContainsKey(key))
                throw new InvalidOperationException($"Service '{key.Name}' already registered.");

            _services[key] = instance;
        }

        /// <summary>Returns a service if present; throws with a helpful message if missing.</summary>
        public static T Get<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var obj))
                return (T)obj;
            throw new KeyNotFoundException($"Service '{typeof(T).Name}' not found. Did Bootstrap complete and Seal? Registered: {DebugList()}");
        }

        /// <summary>Returns a service if present, else null (non-throwing).</summary>
        public static T? TryGet<T>() where T : class
        {
            return _services.TryGetValue(typeof(T), out var obj) ? (T)obj : null;
        }

        /// <summary>True if a service of type T is registered.</summary>
        public static bool Has<T>() where T : class => _services.ContainsKey(typeof(T));

        /// <summary>
        /// Removes a service of type T if present. Useful in tests or when hot-reloading editor tools.
        /// No-op if not present.
        /// </summary>
        public static void Remove<T>() where T : class
        {
            _services.Remove(typeof(T));
        }

        /// <summary>
        /// Replaces an existing service if present, otherwise registers it.
        /// In Release builds, respects Seal; in Editor/Dev builds, allows swap for flexibility.
        /// </summary>
        public static void TryReplace<T>(T instance) where T : class
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (!_initialized) throw new InvalidOperationException("ServiceRegistry.Init() must be called first.");
            _services[typeof(T)] = instance ?? throw new ArgumentNullException(nameof(instance));
#else
            // In non-dev builds, only allow replace if not sealed and not already present
            if (_sealed) throw new InvalidOperationException($"ServiceRegistry is sealed (by '{_sealedBy}'); cannot replace {typeof(T).Name}.");
            if (_services.ContainsKey(typeof(T)))
                throw new InvalidOperationException($"Service '{typeof(T).Name}' already registered.");
            Register(instance);
#endif
        }

        /// <summary>
        /// Lists currently registered service type names. Helpful in error messages and diagnostics.
        /// </summary>
        public static string DebugList()
        {
            if (_services.Count == 0) return "(none)";
            var sb = new StringBuilder();
            bool first = true;
            foreach (var kv in _services)
            {
                if (!first) sb.Append(", ");
                sb.Append(kv.Key.Name);
                first = false;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Test-only hard reset. Useful for PlayMode/EditMode tests that need clean state.
        /// No effect in Player builds.
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void ResetForTests()
        {
            Init();
        }
    }
}
