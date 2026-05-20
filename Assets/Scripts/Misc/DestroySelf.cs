using UnityEngine;

namespace Misc
{
    public class DestroySelf : MonoBehaviour
    {
        [SerializeField] private float delay = 0f;

        public void DestroyObject() => Destroy(gameObject, delay);
        public void DestroyParent() => Destroy(transform.parent.gameObject, delay);
    }
}
