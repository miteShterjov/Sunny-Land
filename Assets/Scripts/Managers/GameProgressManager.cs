using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LevelMech;
using Misc;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameProgressManager : MonoBehaviour
    {
        public int CurrentLevel
        {
            get => currentLevel;
            set => currentLevel = value;
        }

        [Header("Game Progress Settings")] 
        [SerializeField] private LevelCollection levelsCollection;
        [SerializeField] private MapPoint[] mapPoints;
        [SerializeField] private int currentLevel;
        [SerializeField] private GameObject playerIcon;

        private void Start()
        {
            // gather all the assets we gonna need for this scene
            GetLevelCollection();
            GetAllMapPoints();
            GetPlayerIcon();
            
            // check if we have all the assets we need, very important for the scene to work
            if (levelsCollection == null) Debug.LogError("No levels collection found in the scene");
            if (mapPoints == null) Debug.LogError("No map points found in the scene");
            if (playerIcon == null) Debug.LogError("Player icon not found in the scene");
            
            // test if UpdateLevelIconColorsCo() works properly
            levelsCollection.levels[0].isCompleted = true;
            levelsCollection.levels[1].isCompleted = true;
            levelsCollection.levels[2].isCompleted = true;
            currentLevel = 1;
            
            // sync: any completed level should also be marked unlocked
            foreach (var level in levelsCollection.levels)
            {
                if (level.isCompleted) level.isUnlocked = true;
            }
            
            StartCoroutine(UpdateLevelIconColorsCo());
        }
        
        private IEnumerator UpdateLevelIconColorsCo()
        {
            yield return null;

            foreach (var mapPoint in mapPoints)
            {
                if (mapPoint.LevelData == null)
                {
                    Debug.LogWarning($"MapPoint '{mapPoint.name}' has no LevelData assigned — skipping.", mapPoint);
                    continue;
                }

                if (mapPoint.LevelData.isUnlocked || mapPoint.LevelData.isCompleted)
                {
                    mapPoint.ColorLevelIconActive();
                }
            }

            yield return StartCoroutine(UpdatePlayerIconPosition());
        }

        private IEnumerator UpdatePlayerIconPosition()
        {
            playerIcon.transform.position = GetCurrentMapPoint().transform.position;
            yield return null;
        }
        
        
        
        
        
        
        
        private MapPoint GetCurrentMapPoint()
        {
            return mapPoints.FirstOrDefault(mapPoint =>
                levelsCollection.levels[currentLevel].sceneName == mapPoint.LevelData.sceneName);
        }

        private MapPoint GetNextLevelMapPoint()
        {
            return mapPoints.FirstOrDefault(mapPoint =>
                levelsCollection.levels[currentLevel+1].sceneName == mapPoint.LevelData.sceneName);
        }

        
        
        
        
        
        
        private void GetAllMapPoints()
        {
            const int maxPoints = 10; // guard
    
            var points = new List<MapPoint>();
            MapPoint current = GameObject.Find("Point (1)").GetComponent<MapPoint>();
    
            while (current != null && points.Count < maxPoints)
            {
                points.Add(current);
                current = current.NextMapPoint;
            }
    
            if (points.Count >= maxPoints)
                Debug.LogWarning("GetAllMapPoints hit the loop guard — check for a circular reference in your MapPoints chain.");
    
            mapPoints = points.ToArray();
        }
        
        private void GetPlayerIcon() => playerIcon = GameObject.FindWithTag("Player");

        private void GetLevelCollection()
        {
            levelsCollection = levelsCollection = Resources.Load<LevelCollection>("LevelCollection");
        }
    }
}