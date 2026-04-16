using System;
using System.Collections.Generic;

namespace DastarkhanDisaster.Core.Events
{
    /// <summary>
    /// Type-safe static publish/subscribe bus.
    /// Decouples senders from receivers. Use struct event payloads to avoid GC.
    /// Pattern: Observer.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _subscribers = new();

        public static void Subscribe<T>(Action<T> handler) where T : struct
        {
            var type = typeof(T);
            _subscribers[type] = _subscribers.TryGetValue(type, out var existing)
                ? Delegate.Combine(existing, handler)
                : handler;
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : struct
        {
            var type = typeof(T);
            if (!_subscribers.TryGetValue(type, out var existing)) return;

            var updated = Delegate.Remove(existing, handler);
            if (updated == null) _subscribers.Remove(type);
            else _subscribers[type] = updated;
        }

        public static void Raise<T>(T evt) where T : struct
        {
            if (_subscribers.TryGetValue(typeof(T), out var d))
                ((Action<T>)d)?.Invoke(evt);
        }

        public static void Clear() => _subscribers.Clear();
    }
}
