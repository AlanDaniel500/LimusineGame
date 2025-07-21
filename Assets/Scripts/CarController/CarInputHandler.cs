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
        float acceleration = throttleInput - brakeInput;

        topDownCarController.SetInputVector(new Vector2(steerInput, acceleration));
    }

    public void OnSteer(InputAction.CallbackContext context)
    {
        steerInput = context.ReadValue<Vector2>().x;
    }

    //Acelerar
    public void OnAccelerate(InputAction.CallbackContext context)
    {
        throttleInput = context.ReadValue<float>();
    }

    //Frenar
    public void OnBrake(InputAction.CallbackContext context)
    {
        brakeInput = context.ReadValue<float>();
    }
}
