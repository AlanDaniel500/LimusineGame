using UnityEngine;
using UnityEngine.InputSystem;

public class LimuController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float brakeSpeed;

    public InputAction playerControls;

    Rigidbody2D rb2D;

    Vector2 moveDirection = Vector2.zero;  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = playerControls.ReadValue<Vector2>();
    }


    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void LimusineSprint(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            rb2D.linearVelocity = new Vector2(callbackContext.ReadValue<Vector2>().x * maxSpeed, rb2D.linearVelocity.y);
        }
    }



}

