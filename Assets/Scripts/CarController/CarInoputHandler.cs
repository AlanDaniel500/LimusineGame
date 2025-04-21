using UnityEngine;
using UnityEngine.InputSystem;

public class CarInputHandler : MonoBehaviour
{
    private TopDownCarController topDownCarController;
    private float throttleInput = 0f; // R2
    private float brakeInput = 0f;    // L2
    private float steerInput = 0f;    // Stick Izquierdo

    void Start()
    {
        topDownCarController = GetComponent<TopDownCarController>();
    }

    private void FixedUpdate()
    {
        // Combina aceleracion y freno en un solo valor (positivo para avanzar, negativo para reversa)
        float acceleration = throttleInput - brakeInput;

        // Enviar inputs al controlador del auto
        topDownCarController.SetInputVector(new Vector2(steerInput, acceleration));
    }

    // Metodo llamado por Input System cuando mueves el stick izquierdo
    public void OnSteer(InputAction.CallbackContext context)
    {
        steerInput = context.ReadValue<Vector2>().x; // Tomamos solo el eje X (izquierda/derecha)
    }

    // Método llamado por Input System cuando presionas R2
    public void OnAccelerate(InputAction.CallbackContext context)
    {
        throttleInput = context.ReadValue<float>(); // Devuelve un valor entre 0 y 1
    }

    // Método llamado por Input System cuando presionas L2
    public void OnBrake(InputAction.CallbackContext context)
    {
        brakeInput = context.ReadValue<float>(); // Devuelve un valor entre 0 y 1
    }
}
