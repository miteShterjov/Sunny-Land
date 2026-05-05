using Player;
using UnityEngine;

public class CherryPickUp : MonoBehaviour, IPickUps
{
    public string PickUpName => "Cherry";
    
    [Header("Cherry Settings")]
    [SerializeField] private float healAmount = 10;

    private Animator anim;
    private static readonly int PickUpAnim = Animator.StringToHash("isPickedUp");

    private void Awake() 
    {
        anim = GetComponent<Animator>();
    }

    public void PickUpEffect(GameObject player) => player.GetComponent<PlayerHealthController>().Heal(healAmount);
    
    public void HandleAnimPickUpEvent() => anim.SetBool(PickUpAnim, true);
    
    public void DestroyObject() => Destroy(gameObject);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        PickUpManager.Instance.ApplyPickUpEffect(this, collision.gameObject);
    }
}
