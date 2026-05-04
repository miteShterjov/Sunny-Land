using UnityEngine;

public interface IPickUps
{
    void ApplyPickUpEffect(GameObject player);
    void HandleAnimPickUpEvent();
    void DestroyObject();
}
