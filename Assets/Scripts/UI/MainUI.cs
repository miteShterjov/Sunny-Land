using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainUI : MonoBehaviour
    {
        [Header("Lives UI")]
        [SerializeField] private Image haveLifeSprite;
        [SerializeField] private Image noLifeSprite;
        [SerializeField] private Transform livesContainer;

        private Player.PlayerStats player;

        private void Awake()
        {
            player = FindFirstObjectByType<Player.PlayerStats>();
            if (!player) Debug.LogError("PlayerStats component not found in the scene. Please ensure there is a GameObject with PlayerStats attached.");

            UpdateLivesUI(player.CurrentLives, player.MaxLives);
        }

        private void Update()
        {
            // !!! when i do some propper logic in a player controller this will be removed
            UpdateLivesUI(player.CurrentLives, player.MaxLives);
        }

        private void UpdateLivesUI(int currentLives, int maxLives)
        {
            foreach (Transform child in livesContainer)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < maxLives; i++)
            {
                Image lifeIcon = Instantiate(i < currentLives ? haveLifeSprite : noLifeSprite, livesContainer);
                lifeIcon.transform.localScale = Vector3.one;
            }
        }
    }
}
