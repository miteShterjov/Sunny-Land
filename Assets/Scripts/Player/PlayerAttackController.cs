using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerData))]
    public class PlayerAttackController : MonoBehaviour
    {
        [Header("Attack Settings")]
        [SerializeField] GameObject fireballPrefab;

        private PlayerData playerData;

        void Awake()
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
            float magikaCost = fireballPrefab.GetComponent<Fireball>().MagikaCost;
            if (playerData.CurrentMagika < magikaCost) return;
            
            playerData.CurrentMagika -= magikaCost;
            Instantiate(fireballPrefab, transform.position + transform.forward, Quaternion.identity);
        }
    }
}