using UnityEngine;
using TMPro;

public class PassengerCounterUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private LimuPassengerSystem playerPassengerSystem;
    [SerializeField] private TMP_Text passengerCounterText;

    private int lastPassengerCount = -1;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = passengerCounterText.transform.localScale;
    }

    private void Update()
    {
        if (playerPassengerSystem != null && passengerCounterText != null)
        {
            int currentCount = playerPassengerSystem.GetPassengerCount();
            if (currentCount != lastPassengerCount)
            {
                lastPassengerCount = currentCount;
                passengerCounterText.text = $"x{currentCount}";

                // Animación sutil
                StopAllCoroutines();
                StartCoroutine(PopEffect());
            }
        }
    }

    private System.Collections.IEnumerator PopEffect()
    {
        passengerCounterText.transform.localScale = originalScale * 1.2f;
        yield return new WaitForSeconds(0.1f);
        passengerCounterText.transform.localScale = originalScale;
    }
}


