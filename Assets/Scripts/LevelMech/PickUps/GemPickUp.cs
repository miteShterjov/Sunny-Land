using UnityEngine;

public class GemPickUps : MonoBehaviour, IPickUps
{
    [Header("Pick Up Settings")]
    [SerializeField] private float destroyDelay = 0f;

    private static readonly int PickUpAnim = Animator.StringToHash("isPickedUp");

    public void ApplyPickUpEffect(GameObject player) => GameManager.Instance.AddGemCollected();
    
    public void DestroyObject() => Destroy(gameObject, destroyDelay);
    
    public void HandleAnimPickUpEvent() => GetComponent<Animator>().SetBool(PickUpAnim, true);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyPickUpEffect(collision.gameObject);
            HandleAnimPickUpEvent();
        }
    }
}
