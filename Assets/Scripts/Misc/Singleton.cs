using UnityEngine;

namespace Misc
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        public static T Instance { get { return _instance; } }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = (T)this;

            if (transform.parent == null)
                DontDestroyOnLoad(gameObject);
        }
    }
}
