using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Configuraciones")]
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private float delayBeforeRestart = 5f;

    private void Update()
    {
        int remainingPassengers = GameObject.FindGameObjectsWithTag("Passenger").Length;

        if (remainingPassengers == 0)
        {
            if (victoryText != null)
            {
                victoryText.gameObject.SetActive(true);
                victoryText.text = "¡Todos los pasajeros fueron recogidos!";
            }

            Invoke(nameof(RestartGame), delayBeforeRestart);
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

