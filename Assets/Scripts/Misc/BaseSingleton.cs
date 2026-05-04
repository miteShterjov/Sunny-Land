using UnityEngine;

namespace Misc
{
    public class BaseSingleton : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}