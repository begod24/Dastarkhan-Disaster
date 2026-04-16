using System.Collections.Generic;
using UnityEngine;

namespace DastarkhanDisaster.Core.Pooling
{
    /// <summary>
    /// Generic component pool. Avoids Instantiate/Destroy churn for frequent spawns
    /// (food items, particles, UI markers).
    /// Pattern: Object Pool.
    /// </summary>
    public class GenericPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Queue<T> _pool = new();

        public GenericPool(T prefab, int initialSize = 0, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
            for (int i = 0; i < initialSize; i++)
                _pool.Enqueue(CreateNew());
        }

        private T CreateNew()
        {
            var instance = Object.Instantiate(_prefab, _parent);
            instance.gameObject.SetActive(false);
            return instance;
        }

        public T Get()
        {
            var instance = _pool.Count > 0 ? _pool.Dequeue() : CreateNew();
            instance.gameObject.SetActive(true);
            return instance;
        }

        public void Release(T instance)
        {
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(_parent);
            _pool.Enqueue(instance);
        }
    }
}
