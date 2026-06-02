using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelCollection", menuName = "Levels/LevelCollection")]
    public class LevelCollection : ScriptableObject
    {
        public LevelData[] levels;
    }
}