// Path: Assets/West/Core/EventBus.cs
// Assembly: West.Core
// Namespace: West.Core
// Purpose: Lightweight event aggregator for game systems. Events are emitted during Commit steps (AD §6).
// For M1.1 we just need basic subscribe/publish; filters, history, and scopes can grow later.

#nullable enable
using System;
using System.Collections.Generic;

namespace West.Core
{
    /// <summary>
    /// Minimal typed event bus. Systems publish plain structs/classes; listeners subscribe by type.
    /// Per AD §6, events are emitted during Commit, but the bus itself is time-agnostic.
    /// </summary>
    public sealed class EventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = new();

        /// <summary>Subscribe to events of type T.</summary>
        public void Subscribe<T>(Action<T> handler)
        {
            var t = typeof(T);
            if (!_handlers.TryGetValue(t, out var list))
                _handlers[t] = list = new List<Delegate>();
            list.Add(handler);
        }

        /// <summary>Unsubscribe from events of type T.</summary>
        public void Unsubscribe<T>(Action<T> handler)
        {
            var t = typeof(T);
            if (_handlers.TryGetValue(t, out var list))
                list.Remove(handler);
        }

        /// <summary>Publish an event instance to all subscribers of its runtime type.</summary>
        public void Publish<T>(T evt)
        {
            var t = typeof(T);
            if (_handlers.TryGetValue(t, out var list))
            {
                // Iterate over snapshot to allow handlers to unsubscribe safely during callbacks.
                var snapshot = list.ToArray();
                for (int i = 0; i < snapshot.Length; i++)
                    ((Action<T>)snapshot[i]).Invoke(evt!);
            }
        }
    }
}
