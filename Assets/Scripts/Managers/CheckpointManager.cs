using System;
using LevelMech;
using Player;
using UnityEngine;
using Misc;

namespace Managers
{
    public class CheckpointManager : Singleton<CheckpointManager>
    {
        private Checkpoint[] respawnpoints;
        private Checkpoint currentActiveRespawnPoint;
        private Vector3 currentRespawnPoint;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            respawnpoints = UnityEngine.Object.FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
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
        
        private void SaveActiveRespawnPoint(Checkpoint respawnpoint)
        {
            if (currentActiveRespawnPoint) currentActiveRespawnPoint.DeactivateRespawnPoint();
            currentActiveRespawnPoint = respawnpoint;
            currentRespawnPoint = respawnpoint.transform.position;
            respawnpoint.ActivateRespawnPoint();
        }
    }
}