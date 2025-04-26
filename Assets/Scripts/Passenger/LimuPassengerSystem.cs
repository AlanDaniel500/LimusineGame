using UnityEngine;

public class LimuPassengerSystem : MonoBehaviour
{
    [SerializeField] private PassengerSpawner passengerSpawner;
    [SerializeField] private GameObject passengerObject;
    [SerializeField] private GameObject destinationObject;

    private MyQueue<GameObject> passengerQueue = new MyQueue<GameObject>();
    private MyStack<PowerUpSpeed> powerUpStack = new MyStack<PowerUpSpeed>();
    private TopDownCarController carController;

    private Transform currentTarget;
    private bool hasPassenger = false;

    private float originalMaxSpeed;
    private bool isBoosted = false;
    private float boostTimer = 0f;

    private void Start()
    {
        carController = GetComponent<TopDownCarController>();
        originalMaxSpeed = carController.maxSpeed;

        if (passengerObject != null)
        {
            currentTarget = passengerObject.transform;
        }
    }

    private void Update()
    {
        if (currentTarget == null) return;

        Vector2 dirToTarget = ((Vector2)currentTarget.position - (Vector2)transform.position).normalized;
        float angleToTarget = Vector2.SignedAngle(transform.up, dirToTarget);
        float turnAmount = Mathf.Clamp(angleToTarget / 45f, -1f, 1f);
        float forwardAmount = Vector2.Dot(transform.up, dirToTarget) > 0 ? 1f : -1f;

        float distance = Vector2.Distance(transform.position, currentTarget.position);
        if (distance < 0.5f)
        {
            forwardAmount = 0f;
            turnAmount = 0f;

            if (!hasPassenger)
            {
                hasPassenger = true;
                if (passengerObject != null)
                {
                    PassengerTag passengerTag = passengerObject.GetComponent<PassengerTag>();
                    if (passengerTag != null && !passengerTag.isTaken)
                    {
                        passengerTag.isTaken = true;
                        passengerTag.takenBy = gameObject;
                    }

                    passengerQueue.Enqueue(passengerObject);
                    passengerObject.SetActive(false);
                    Debug.Log("Player: Passenger recogido");
                }

                currentTarget = destinationObject.transform;
            }
            else
            {
                if (!passengerQueue.IsEmpty)
                {
                    GameObject passenger = passengerQueue.Dequeue();
                    Destroy(passenger);
                    Debug.Log("Player: Passenger entregado");

                    passengerSpawner.SpawnNewPassenger();
                }

                currentTarget = null;
                hasPassenger = false;
            }
        }

        carController.SetInputVector(new Vector2(turnAmount, forwardAmount));


        if (isBoosted)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0)
            {
                carController.maxSpeed = originalMaxSpeed;
                isBoosted = false;
                Debug.Log("Player: Se terminó el boost de velocidad.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("PowerUp"))
        {
            PowerUpSpeed powerUp = collision.GetComponent<PowerUpSpeed>();
            if (powerUp != null)
            {
                powerUpStack.Push(powerUp);
                Debug.Log("Player: PowerUp recogido y agregado a la pila.");

                ApplyNextPowerUp();
                Destroy(collision.gameObject);
            }
        }
    }

    private void ApplyNextPowerUp()
    {
        if (!powerUpStack.IsEmpty)
        {
            PowerUpSpeed powerUp = powerUpStack.Pop();

            carController.maxSpeed *= powerUp.speedMultiplier;
            boostTimer = powerUp.duration;
            isBoosted = true;

            Debug.Log($"Player: Aplicado PowerUp de velocidad x{powerUp.speedMultiplier} por {powerUp.duration} segundos.");
        }
    }

    public bool HasPassengers()
    {
        return !passengerQueue.IsEmpty;
    }

    public GameObject StealPassenger()
    {
        if (!passengerQueue.IsEmpty)
        {
            return passengerQueue.Dequeue();
        }
        return null;
    }

    public int GetPassengerCount()
    {
        return passengerQueue.Count;
    }
}






