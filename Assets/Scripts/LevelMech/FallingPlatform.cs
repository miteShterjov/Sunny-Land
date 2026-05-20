using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 1f; 
    public float resetDelay = 5f; 
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody2D rb;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static; // Make the platform static at the start
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("Fall", fallDelay);
            Invoke("ResetPlatform", fallDelay + resetDelay);
        }
    }

    void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void ResetPlatform()
    {
        transform.position = initialPosition; // Reset position
        transform.rotation = initialRotation; // Reset rotation
        rb.linearVelocity = Vector2.zero; // Reset velocity
        rb.angularVelocity = 0f; // Reset angular velocity
        rb.bodyType = RigidbodyType2D.Static; // Make the platform static again
    }
}
