using UnityEngine;
using UnityEngine.Serialization;

namespace LevelMech
{
    public class LootDropHandler : MonoBehaviour
    {
        [SerializeField] private GameObject[] possibleLoot;
        [SerializeField] private int minLootAmount = 1;
        [SerializeField] private int maxLootAmount = 2;

        public void DropLoot()
        {
            int dropCount = Random.Range(minLootAmount, maxLootAmount + 1);
            
            for (int i = 0; i < dropCount; i++)
            {
                int index = Random.Range(0, possibleLoot.Length);
                Instantiate(possibleLoot[index], transform.position, Quaternion.identity);
            }
        }
    }
}
