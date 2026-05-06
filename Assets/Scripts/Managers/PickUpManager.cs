using LevelMech.PickUps;
using Misc;
using Player;
using UnityEngine;

namespace Managers
{
    public class PickUpManager : Singleton<PickUpManager>
    {
        private PlayerStats playerStats;

        private void Start()
        {
            playerStats = FindAnyObjectByType<PlayerStats>();
            if (!playerStats) Debug.LogError("PlayerStats not found in PickUpManager.");
        }

        public static void ApplyPickUpEffect(IPickUps pickUp, GameObject player)
        {
            if (!pickUp.CanPickUp(player)) return;
            
            pickUp.OnPickUp(player);
            pickUp.HandleAnimPickUpEvent();
        }
    }
}
