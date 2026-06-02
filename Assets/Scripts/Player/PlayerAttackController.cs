using SpecialAttacks;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerData))]
    public class PlayerAttackController : MonoBehaviour
    {
        [Header("Attack Settings")]
        [SerializeField] GameObject fireballPrefab;

        private PlayerData playerData;

        private void Awake()
        {
            playerData = GetComponent<PlayerData>();
        }

        public void BasicAttack()
        {
            // Implement basic attack logic here
            print("Player performs a basic attack!");
        }

        public void SpecialAttack()
        {
            float manaCost = fireballPrefab.GetComponent<Fireball>().ManaCost;
            if (playerData.CurrentMana < manaCost) return;
            
            playerData.CurrentMana -= manaCost;
            Instantiate(fireballPrefab, transform.position + transform.forward, Quaternion.identity);
        }
    }
}