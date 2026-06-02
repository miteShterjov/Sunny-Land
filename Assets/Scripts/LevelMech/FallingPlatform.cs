using UnityEngine;

namespace LevelMech
{
    public class FallingPlatform : MonoBehaviour
    {
        public float fallDelay = 1f; 
        public float resetDelay = 5f; 
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private Rigidbody2D rb;

        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static; // Make the platform static at the start
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            Invoke(nameof(Fall), fallDelay);
            Invoke(nameof(ResetPlatform), fallDelay + resetDelay);
        }

        private void Fall() => rb.bodyType = RigidbodyType2D.Dynamic;

        private void ResetPlatform()
        {
            transform.SetPositionAndRotation(initialPosition, initialRotation);
            rb.linearVelocity = Vector2.zero; // Reset velocity
            rb.angularVelocity = 0f; // Reset angular velocity
            rb.bodyType = RigidbodyType2D.Static; // Make the platform static again
        }
    }
}
