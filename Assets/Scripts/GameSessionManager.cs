using UnityEngine;

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager Instance;

    public int passengerDelivered = 0;
    public int upgradePoints = 0;
    public float speedUpgrade = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persiste entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddDeliveredPassenger()
    {
        passengerDelivered++;
        upgradePoints += 10; // 10 puntos por pasajero entregado
    }

    public void ApplySpeedUpgrade()
    {
        speedUpgrade += 1f;
    }

    public void ResetSession()
    {
        passengerDelivered = 0;
        upgradePoints = 0;
        speedUpgrade = 0f;
    }
}

