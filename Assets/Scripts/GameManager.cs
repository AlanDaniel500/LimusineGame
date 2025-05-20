using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Configuraciones")]
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private float delayBeforeRestart = 5f;

    private LimuPassengerSystem playerSystem;
    private EnemyCarAI enemySystem;
    private PassengerManager manager;

    private void Start()
    {
        playerSystem = FindFirstObjectByType<LimuPassengerSystem>();
        enemySystem = FindFirstObjectByType<EnemyCarAI>();
        manager = FindFirstObjectByType<PassengerManager>();
    }

    private void Update()
    {
        bool playerHasPassengers = playerSystem != null && playerSystem.GetPassengerCount() > 0;
        bool enemyHasPassengers = enemySystem != null && enemySystem.HasPassengers();

        if (manager != null && manager.AllDelivered() && !playerHasPassengers && !enemyHasPassengers)
        {
            if (victoryText != null)
            {
                victoryText.gameObject.SetActive(true);
                victoryText.text = "¡Todos los pasajeros fueron recogidos y entregados!";
            }

            Invoke(nameof(RestartGame), delayBeforeRestart);
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


