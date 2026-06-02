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
    public class GameProgressManager : Singleton<GameProgressManager>
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
            if (SceneManager.GetActiveScene().name != "Overworld") return;
            
            GetLevelCollection();
            GetAllMapPoints();
            GetPlayerIcon();
            
            if (levelsCollection == null) Debug.LogError("No levels collection found in the scene");
            if (mapPoints == null) Debug.LogError("No map points found in the scene");
            if (playerIcon == null) Debug.LogError("Player icon not found in the scene");
            // mark first level as unlocked from start of the game
            levelsCollection.levels[0].isUnlocked = true;
            LoadNextLevelSequence();
        }

        private void LoadNextLevelSequence() => StartCoroutine(LoadNextLevelSequenceCo());

        private IEnumerator LoadNextLevelSequenceCo()
        {
            //mark current level completed
            CurrentLevelIsCompleted();
            // unlock next level + highlight next level icon and move playerIcon to next level
            UnlockNextLevel();
            yield return StartCoroutine(LerpIconColor(Color.white, Color.gray2, 0.3f));
            yield return StartCoroutine(MovePlayerIconToNextLevelCo(playerIcon.transform));
            //when playerIcon is next level icon -> load next level
        }

        private void CurrentLevelIsCompleted() => levelsCollection.levels[currentLevel].isCompleted = true;
        private void UnlockNextLevel() => levelsCollection.levels[currentLevel + 1].isUnlocked = true;

        private IEnumerator LerpIconColor(Color from, Color to, float duration)
        {
            SpriteRenderer icon = GetNextLevelMapPoint().GetComponentInChildren<SpriteRenderer>();
            if (!icon)
            {
                Debug.LogError("SpriteRenderer component not found in " + GetNextLevelMapPoint().gameObject.name);
                yield break;
            }
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                icon.color = Color.Lerp(from, to, elapsed / duration);
                yield return null;
            }

            icon.color = to;
        }
        
        private IEnumerator MovePlayerIconToNextLevelCo(Transform icon)
        {
            MapPoint currentMapPoint = GetCurrentMapPoint();

            while (currentMapPoint != null)
            {
                currentMapPoint = currentMapPoint.NextMapPoint;
                yield return StartCoroutine(MoveIconTo(icon, currentMapPoint.transform.position));
                
                if (currentMapPoint.IsLevelPoint) break;
            }
        }
        
        private IEnumerator MoveIconTo(Transform icon, Vector3 target)
        {
            float speed = 5f;
    
            while (Vector3.Distance(icon.position, target) > 0.01f)
            {
                icon.position = Vector3.MoveTowards(icon.position, target, speed * Time.deltaTime);
                yield return null;
            }
    
            icon.position = target;
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