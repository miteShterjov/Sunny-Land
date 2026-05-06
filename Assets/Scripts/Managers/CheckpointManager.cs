using LevelMech;
using Player;
using UnityEngine;
using Misc;

namespace Managers
{
    public class CheckpointManager : Singleton<CheckpointManager>
    {
        private Checkpoint currentActiveRespawnPoint;
        private Vector3 currentRespawnPoint;

        private void Start()
        {
            currentRespawnPoint = FindFirstObjectByType<PlayerStats>().transform.position;
        }

        private void OnEnable()
        {
            Checkpoint.OnSpawnStatusChanged += SaveActiveRespawnPoint;
        }

        private void OnDisable()
        {
            Checkpoint.OnSpawnStatusChanged -= SaveActiveRespawnPoint;
        }

        public Vector3 GetCurrentRespawnPoint() => currentRespawnPoint;
        
        private void SaveActiveRespawnPoint(Checkpoint respawnPoint)
        {
            if (currentActiveRespawnPoint) currentActiveRespawnPoint.DeactivateRespawnPoint();
            currentActiveRespawnPoint = respawnPoint;
            currentRespawnPoint = respawnPoint.transform.position;
            respawnPoint.ActivateRespawnPoint();
        }
    }
}