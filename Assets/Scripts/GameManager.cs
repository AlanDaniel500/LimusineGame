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

    private void Start()
    {
        playerSystem = FindFirstObjectByType<LimuPassengerSystem>();
        enemySystem = FindFirstObjectByType<EnemyCarAI>();
    }

    private void Update()
    {
        bool playerHasPassengers = playerSystem != null && playerSystem.GetPassengerCount() > 0;
        bool enemyHasPassengers = enemySystem != null && enemySystem.HasPassengers();

        bool noPassengersInScene = GameObject.FindGameObjectsWithTag("Passenger").Length == 0;

        if (noPassengersInScene && !playerHasPassengers && !enemyHasPassengers)
        {
            // ¡Realmente terminó el juego!
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


