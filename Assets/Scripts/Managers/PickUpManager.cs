using Misc;
using Player;
using UnityEngine;

public class PickUpManager : Singleton<PickUpManager>
{
    PlayerStats playerStats;

    private void Start()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
        if (!playerStats) Debug.LogError("PlayerStats not found in PickUpManager.");
    }


    public void ApplyPickUpEffect(IPickUps pickUp, GameObject player)
    {
        if (pickUp.PickUpName == "Cherry")
        {
            Debug.Log($"Health: {playerStats.Health} | MaxHealth: {playerStats.MaxHealth}");
            print("its cherry if statement working");
            if (playerStats.Health >= playerStats.MaxHealth)
            {
                print("Player health is already full. Cannot pick up cherry.");
                Debug.Log($"Health: {playerStats.Health} | MaxHealth: {playerStats.MaxHealth}");
                return;
            }
        }

        pickUp.PickUpEffect(player);
        pickUp.HandleAnimPickUpEvent();
    }

}
