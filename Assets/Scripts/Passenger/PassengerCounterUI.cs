using UnityEngine;
using TMPro; // Asegurate de tener TextMeshPro importado

public class PassengerCounterUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private LimuPassengerSystem playerPassengerSystem;
    [SerializeField] private TMP_Text passengerCounterText;

    private void Update()
    {
        if (playerPassengerSystem != null && passengerCounterText != null)
        {
            passengerCounterText.text = "Pasajeros recogidos: " + playerPassengerSystem.GetPassengerCount();
        }
    }
}
