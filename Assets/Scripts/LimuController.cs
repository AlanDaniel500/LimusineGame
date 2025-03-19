using UnityEngine;
using UnityEngine.InputSystem;

public class LimuController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float acceleration = 2.0f;
    [SerializeField] private float brakeSpeed = 3.0f;

    private Rigidbody2D rb2D;
    private float currentSpeed = 0f;
    private float throttleInput = 0f; // Para leer la presión del gatillo

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Acelera progresivamente si se presiona R2
        if (throttleInput > 0)
        {
            currentSpeed += acceleration * throttleInput * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        }
        else
        {
            // Desacelera si no se presiona
            currentSpeed -= brakeSpeed * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        }

        // Aplica el movimiento en la dirección del auto
        rb2D.linearVelocity = transform.up * currentSpeed; // ? Ahora usa linearVelocity
    }

    // Método llamado cuando se presiona R2
    public void LimusineSprint(InputAction.CallbackContext callbackContext)
    {
        throttleInput = callbackContext.ReadValue<float>(); // R2 devuelve un float (0 a 1)
    }
}



