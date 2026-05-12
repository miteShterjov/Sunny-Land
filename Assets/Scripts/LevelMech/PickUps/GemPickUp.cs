using Managers;
using Player;
using UnityEngine;

namespace LevelMech.PickUps
{
    [RequireComponent(typeof(Animator))]
    public class GemPickUps : MonoBehaviour, IPickUps
    {
        private Animator anim;
        private static readonly int PickUpAnim = Animator.StringToHash("isPickedUp");

        private void Awake() 
        {
            anim = GetComponent<Animator>();
        }

        public void OnPickUp(GameObject player) => GameManager.Instance.GemsCollected++;

        public bool CanPickUp(GameObject player) => true;
    
        public void HandleAnimPickUpEvent() => anim.SetBool(PickUpAnim, true);
    
        public void DestroyObject() => Destroy(gameObject);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            PickUpManager.ApplyPickUpEffect(this, collision.gameObject);
        }
    }
}
