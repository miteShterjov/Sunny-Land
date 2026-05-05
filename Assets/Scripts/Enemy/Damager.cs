using Player;
using UnityEngine;

namespace Enemy
{
    public class Damager : MonoBehaviour
    {
        [Header("Damage Settings")]
        [SerializeField] private float damageAmount = 10;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            other.GetComponent<PlayerHealthController>().Damage(transform, damageAmount);
        }
    }
}
