using UnityEngine;

public interface IPickUps
{
    public string PickUpName { get; }
    void PickUpEffect(GameObject player);
    void HandleAnimPickUpEvent();
    void DestroyObject();
}
