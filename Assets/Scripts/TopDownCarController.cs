using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownCarController : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] private float maxSpeed = 20.0f;
    [SerializeField] private float accelerationFactor = 25.0f;
    [SerializeField] private float driftFactor = 0.95f;
    [SerializeField] private float turnFactor = 3.5f;
    //[SerializeField] private float desacceleration = 1.0f;
    //[SerializeField] private float brakeSpeed = 5.0f;
    //[SerializeField] private float reverseSpeed = 3.0f;  // Velocidad máxima en reversa
    //[SerializeField] private float reverseAcceleration = 1.5f; // Aceleración en reversa

    private Rigidbody2D rb2D;

    //Variables locales
    //private float currentSpeed = 0f;
    private float accelerationInput = 0f;
    //private float throttleInput = 0f; // Para la aceleración (R2)
    //private float brakeInput = 0f;    // Para el freno (L2)
    private float steerInput = 0f;
    private float velocityVsUp = 0f;
    //private float turnInput = 10.0f;

    private float rotationAngle = 0f;

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
        //Calcula que tanta fuerza delantera vamos a aplicar en terminos de direccion de nuestra velocidad
        velocityVsUp = Vector2.Dot(transform.up, rb2D.linearVelocity);


        //Limita la velocidad del auto
        if (velocityVsUp > maxSpeed && accelerationInput > 0)
        {
            return;
        }

        //Limita la velocidad del auto en reversa un 50% de la velocidad maxima
        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
        {
            return;
        }

        //Limita asi no va más rapido en cualquier direccion mientras se acelera
        if (rb2D.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
        {
            return;
        }


        if (accelerationInput == 0)
        {
            rb2D.linearDamping = Mathf.Lerp(rb2D.linearDamping, 3.0f, Time.fixedDeltaTime * 3);
        }
        else
        {
            rb2D.linearDamping = 0;
        }


            //Crea una fuerza para el motor
            Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        //Aplica fuerza y un empuje al auto hacia adelante
        rb2D.AddForce(engineForceVector, ForceMode2D.Force);
    }


    private void ApplySteering()
    {
        //Limita la habilidad del autor para girar a bajas velocidades
        float minSpeedBeForeAllowTurningFactor = (rb2D.linearVelocity.magnitude / 8);
        minSpeedBeForeAllowTurningFactor = Mathf.Clamp01(minSpeedBeForeAllowTurningFactor);

        rotationAngle -= steerInput * turnFactor * minSpeedBeForeAllowTurningFactor;

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




