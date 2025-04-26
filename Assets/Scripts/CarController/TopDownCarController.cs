using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownCarController : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] private float maxSpeed = 20.0f;
    [SerializeField] private float accelerationFactor = 25.0f;
    [SerializeField] private float driftFactor = 0.95f;
    [SerializeField] private float turnFactor = 3.5f;

    [Header("Configuración")]
    [SerializeField] private int maxPassengers = 4; // <- Sigue private

    private Rigidbody2D rb2D;
    private float accelerationInput = 0f;
    private float steerInput = 0f;
    private float velocityVsUp = 0f;
    private float rotationAngle = 0f;

    // 🔥 Nueva propiedad pública
    public int MaxPassengers => maxPassengers;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        ApplyEngineForces();
        killOrthogonalVelocity();
        ApplySteering();
    }

    private void ApplyEngineForces()
    {
        velocityVsUp = Vector2.Dot(transform.up, rb2D.linearVelocity);

        if (velocityVsUp > maxSpeed && accelerationInput > 0)
            return;

        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
            return;

        if (rb2D.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
            return;

        if (accelerationInput == 0)
            rb2D.linearDamping = Mathf.Lerp(rb2D.linearDamping, 3.0f, Time.fixedDeltaTime * 3);
        else
            rb2D.linearDamping = 0;

        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;
        rb2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void ApplySteering()
    {
        float minSpeedBeforeAllowTurningFactor = (rb2D.linearVelocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        rotationAngle -= steerInput * turnFactor * minSpeedBeforeAllowTurningFactor;
        rb2D.MoveRotation(rotationAngle);
    }

    private void killOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb2D.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb2D.linearVelocity, transform.right);

        rb2D.linearVelocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steerInput = inputVector.x;
        accelerationInput = inputVector.y;
    }
}
