using UnityEngine;

namespace pt.dportela.PlanetGame.Utils
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : class
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                else
                    Debug.LogError($"Instance of {typeof(T).Name} not set.");
                return default;
            }
        }

        protected void Awake()
        {
            if (instance == null)
                instance = this as T;
            else
            {
                Debug.LogError($"Instance of {typeof(T).Name} already set.");
                Destroy(gameObject);
                return;
            }
        }

        protected void OnDestroy()
        {
            if(this as T == instance)
            {
                instance = null;
                Debug.LogWarning($"Instance of {typeof(T).Name} destroyed.");
            }
        }
    }
}
