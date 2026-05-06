using Managers;
using Player;
using UnityEngine;

namespace LevelMech.PickUps
{
    [RequireComponent(typeof(Animator))]
    public class CherryPickUp : MonoBehaviour, IPickUps
    {
        [Header("Cherry Settings")]
        [SerializeField] private float healAmount = 10;

        private Animator anim;
        private static readonly int PickUpAnim = Animator.StringToHash("isPickedUp");

        private void Awake() 
        {
            anim = GetComponent<Animator>();
        }

        public void OnPickUp(GameObject player) => player.GetComponent<PlayerHealthController>().Heal(healAmount);

        public bool CanPickUp(GameObject player)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            return playerStats.CurrentHealth >= playerStats.MaxHealth;
        }
    
        public void HandleAnimPickUpEvent() => anim.SetBool(PickUpAnim, true);
    
        public void DestroyObject() => Destroy(gameObject);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            PickUpManager.ApplyPickUpEffect(this, collision.gameObject);
        }
    }
}
