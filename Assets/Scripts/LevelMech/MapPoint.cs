using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelMech
{
    public class MapPoint : MonoBehaviour
    {
        [FormerlySerializedAs("levelIcon")]
        [Header("Map Point Settings")]
        [SerializeField] private SpriteRenderer levelIconSpRend;
        [SerializeField] private MapPoint nextMapPoint;
        [SerializeField] private LevelData levelData;

        public MapPoint NextMapPoint => nextMapPoint;
        public LevelData LevelData => levelData;
        public bool IsLevelPoint => levelData != null;

        private void Awake()
        {
            if (!levelIconSpRend) Debug.LogError("SpriteRenderer component not found in " + gameObject.name);
        }

        private void Start()
        {
            if (levelData.levelIndex == 0) return;
            ColorLevelIconInactive();
        }

        public void ColorLevelIconActive() => levelIconSpRend.color = Color.white;

        private void ColorLevelIconInactive() => levelIconSpRend.color = Color.gray2;
    }
}
