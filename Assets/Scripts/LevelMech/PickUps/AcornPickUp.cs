using Managers;
using Player;
using UnityEngine;

namespace LevelMech.PickUps
{
    public class AcornPickUp : MonoBehaviour, IPickUps
    {
        [Header("Acorn Settings")]
        [SerializeField] private float value = 10f;

        private PlayerData playerData;
        private Animator anim;
        private static readonly int PickUpAnim = Animator.StringToHash("isPickedUp");

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
            if (playerData == null) Debug.LogError("PlayerData component not found on the player object.");
        }

        public bool CanPickUp(GameObject player) => playerData != null && playerData.CurrentStamina < playerData.MaxStamina;

        public void DestroyObject() => Destroy(gameObject);

        public void HandleAnimPickUpEvent() => anim.SetBool(PickUpAnim, true);

        public void OnPickUp(GameObject player) => playerData.RecoverStamina(value);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            PickUpManager.ApplyPickUpEffect(this, collision.gameObject);
        }
    }
}
