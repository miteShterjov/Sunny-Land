using LevelMech;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Levels/LevelData")]
    public class LevelData : ScriptableObject
    {
        public string sceneName;
        public int levelIndex;
        public bool isUnlocked;
        public bool isCompleted;
    }
}