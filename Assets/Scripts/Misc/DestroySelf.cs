using UnityEngine;

namespace Misc
{
    public class DestroySelf : MonoBehaviour
    {
        public void Start()
        {
            Destroy(gameObject);
        }
    }
}
