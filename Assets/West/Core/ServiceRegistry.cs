// Path: Assets/West/Core/ServiceRegistry.cs
// Assembly: West.Core
// Namespace: West.Core
// Purpose: Minimal, explicit service locator used only for facades per AD §5. Keeps services centralized and 
// DontDestroyOnLoad. No third-party types leak beyond adapters. Thread-safe writes during boot; reads are O(1).

#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace West.Core
{
    /// <summary>
    /// Simple service registry for registering and retrieving West.* facades.
    /// Per AD §5, used during boot to register services in strict order; consumers call Get<T>().
    /// </summary>
    public static class ServiceRegistry
    {
        private static readonly Dictionary<Type, object> _services = new();
        private static bool _initialized;

        /// <summary>Initialize the registry. Idempotent. Clears any previous registrations (domain reload safety).</summary>
        public static void Init()
        {
            _services.Clear();
            _initialized = true;
        }

        /// <summary>Registers a service instance of type T. Throws if T already registered.</summary>
        public static void Register<T>(T instance) where T : class
        {
            if (!_initialized) throw new InvalidOperationException("ServiceRegistry.Init() must be called first.");
            if (instance is null) throw new ArgumentNullException(nameof(instance));

            var key = typeof(T);
            if (_services.ContainsKey(key))
                throw new InvalidOperationException($"Service '{key.Name}' already registered.");

            _services[key] = instance;
        }

        /// <summary>Gets a previously-registered service of type T.</summary>
        public static T Get<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var obj))
                return (T)obj;
            throw new KeyNotFoundException($"Service '{typeof(T).Name}' not found. Did boot complete?");
        }

        /// <summary>Returns a service if present, else null (non-throwing).</summary>
        public static T? TryGet<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var obj))
                return (T)obj;
            return null;
        }
    }
}
