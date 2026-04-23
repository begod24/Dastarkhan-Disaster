using UnityEngine;

namespace DastarkhanDisaster.Core
{
    /// <summary>
    /// Generic MonoBehaviour singleton that survives scene loads.
    /// Pattern: Singleton (controlled, lazy-bound, no static constructor side-effects).
    /// </summary>
    public abstract class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this as T;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}
