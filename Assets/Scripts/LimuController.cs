using UnityEngine;
using UnityEngine.InputSystem;

public class LimuController : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float acceleration = 2.0f;
    [SerializeField] private float desacceleration = 1.0f;
    [SerializeField] private float brakeSpeed = 5.0f;
    [SerializeField] private float reverseSpeed = 3.0f;  // Velocidad máxima en reversa
    [SerializeField] private float reverseAcceleration = 1.5f; // Aceleración en reversa

    private Rigidbody2D rb2D;
    private float currentSpeed = 0f;
    private float throttleInput = 0f; // Para la aceleración (R2)
    private float brakeInput = 0f;    // Para el freno (L2)

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (throttleInput > 0)
        {
            // Acelera si presionas R2
            currentSpeed += acceleration * throttleInput * Time.fixedDeltaTime;
        }
        else if (brakeInput > 0)
        {

            if (currentSpeed > 0)
            {
                // Frena si presionas L2
                currentSpeed -= brakeSpeed * brakeInput * Time.fixedDeltaTime;
            }
            else
            {
                // Si el auto está detenido o en reversa, acelera marcha atrás con L2
                currentSpeed -= reverseAcceleration * brakeInput * Time.fixedDeltaTime;
            }
            
        }
        else
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= desacceleration * Time.fixedDeltaTime;
            }
            else if (currentSpeed < 0)
            {
                currentSpeed += desacceleration * Time.fixedDeltaTime; // Reduce la reversa
            }
        }

        //La velocidad esta clampeada para no baje de 0 ni suepre el maximo
        currentSpeed = Mathf.Clamp(currentSpeed, -reverseSpeed, maxSpeed);

        //Mueve el auto en la direccion que esta mirando
        rb2D.linearVelocity = transform.up * currentSpeed;
    }

    // Aceleración con R2
    public void LimusineSprint(InputAction.CallbackContext callbackContext)
    {
        throttleInput = callbackContext.ReadValue<float>(); // R2 devuelve un float (0 a 1)
    }

    // Freno con L2
    public void BrakeAction(InputAction.CallbackContext callbackContext)
    {
        brakeInput = callbackContext.ReadValue<float>(); // L2 también devuelve un float (0 a 1)
    }
}




