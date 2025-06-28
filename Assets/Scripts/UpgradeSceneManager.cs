using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UpgradeSceneManager : MonoBehaviour
{
    public TMP_Text pointsText;

    private void Start()
    {
        UpdateUI();
    }

    public void UpgradeSpeed()
    {
        GameSessionManager.Instance.ApplySpeedUpgrade();
        UpdateUI();
    }

    public void ContinueToNextLevel()
    {
        SceneManager.LoadScene("SampleScene 1"); // o el nombre real de tu segunda escena
    }

    private void UpdateUI()
    {
        int puntos = GameSessionManager.Instance.upgradePoints;
        float velocidad = GameSessionManager.Instance.speedMultiplier;
        pointsText.text = $"Puntos disponibles: {puntos}\nVelocidad actual: x{velocidad:F1}";
    }
}

