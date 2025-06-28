using UnityEngine;

public class PlayerSpeedManager : MonoBehaviour
{
    public static PlayerSpeedManager Instance { get; private set; }

    public float SpeedMultiplier { get; private set; } = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        SpeedMultiplier = multiplier;
    }
}


