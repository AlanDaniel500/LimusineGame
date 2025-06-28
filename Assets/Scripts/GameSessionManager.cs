using UnityEngine;

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager Instance;

    public int Entregados { get; set; } = 0;
    public int NivelMejora { get; set; } = 0;

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
}
