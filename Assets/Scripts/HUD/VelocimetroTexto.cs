using UnityEngine;
using TMPro;

public class VelocimetroTexto : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private TMP_Text velocidadTexto;

    private void Update()
    {
        if (playerRb == null || velocidadTexto == null) return;

        float velocidad = playerRb.linearVelocity.magnitude;
        velocidadTexto.text = $"{velocidad:F1}";
    }
}

