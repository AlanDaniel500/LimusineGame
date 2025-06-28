using UnityEngine;

public class PlayerSpeedManager : MonoBehaviour
{
    public float baseSpeed = 5f;
    private float currentSpeed;

    void Start()
    {
        float upgrade = GameSessionManager.Instance?.speedUpgrade ?? 0f;
        currentSpeed = baseSpeed + upgrade;
    }

    public float GetSpeed()
    {
        return currentSpeed;
    }
}

