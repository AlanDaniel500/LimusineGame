using UnityEngine;
using UnityEngine.InputSystem;

public class CarInoputHandler : MonoBehaviour
{

    TopDownCarController topDownCarController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        topDownCarController = GetComponent<TopDownCarController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        topDownCarController.SetInputVector(inputVector);
    }

    private void FixedUpdate()
    {
       
    }
}
