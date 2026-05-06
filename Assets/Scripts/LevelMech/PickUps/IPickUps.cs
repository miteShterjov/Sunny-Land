using Player;
using UnityEngine;

namespace LevelMech.PickUps
{
    public interface IPickUps
    {
        void OnPickUp(GameObject player);
        bool CanPickUp(GameObject player);
        void HandleAnimPickUpEvent();
        void DestroyObject();
    }
}
