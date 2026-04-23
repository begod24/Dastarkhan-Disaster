using System;
using System.Collections.Generic;

namespace DastarkhanDisaster.Core
{
    /// <summary>
    /// Lightweight service registry. Prefer this over scattered static singletons.
    /// Pattern: Service Locator. Replaces direct FindObjectOfType / hard references.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();

        public static void Register<T>(T service) where T : class
            => _services[typeof(T)] = service;

        public static void Unregister<T>() where T : class
            => _services.Remove(typeof(T));

        public static T Get<T>() where T : class
            => _services.TryGetValue(typeof(T), out var s) ? (T)s : null;

        public static bool TryGet<T>(out T service) where T : class
        {
            if (_services.TryGetValue(typeof(T), out var s))
            {
                service = (T)s;
                return true;
            }
            service = null;
            return false;
        }

        public static void Clear() => _services.Clear();
    }
}
