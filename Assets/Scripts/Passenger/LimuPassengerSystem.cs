using UnityEngine;

public class LimuPassengerSystem : MonoBehaviour
{
    [SerializeField] private PassengerSpawner passengerSpawner;
    [SerializeField] private GameObject destinationObject;

    private MyQueue<GameObject> passengerQueue = new MyQueue<GameObject>();
    private MyStack<PowerUpSpeed> powerUpStack = new MyStack<PowerUpSpeed>();
    private TopDownCarController carController;

    private float originalMaxSpeed;
    private bool isBoosted = false;
    private float boostTimer = 0f;

    private void Start()
    {
        carController = GetComponent<TopDownCarController>();
        originalMaxSpeed = carController.maxSpeed;
    }

    private void Update()
    {

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
        if (collision.CompareTag("Passenger"))
        {
            PassengerTag passengerTag = collision.GetComponent<PassengerTag>();
            if (passengerTag != null && !passengerTag.isTaken)
            {
                passengerTag.isTaken = true;
                passengerTag.takenBy = gameObject;

                passengerQueue.Enqueue(collision.gameObject);
                collision.gameObject.SetActive(false);
                Debug.Log("Player: Passenger recogido");
            }
        }

        if (collision.gameObject.CompareTag("Destination"))
        {
            if (!passengerQueue.IsEmpty)
            {
                DeliverPassenger();
                passengerSpawner.SpawnNewPassenger();
            }
        }


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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyCarAI enemySystem = collision.gameObject.GetComponent<EnemyCarAI>();
            if (enemySystem != null && enemySystem.HasPassengers())
            {
                GameObject stolenPassenger = enemySystem.StealPassenger();
                if (stolenPassenger != null)
                {
                    PassengerTag passengerTag = stolenPassenger.GetComponent<PassengerTag>();
                    if (passengerTag != null)
                    {
                        passengerTag.isTaken = true;
                        passengerTag.takenBy = gameObject;
                    }

                    passengerQueue.Enqueue(stolenPassenger);
                    Debug.Log("Player: ¡Robé un Passenger al Enemy!");
                }
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

    public void DeliverPassenger()
    {
        if (!passengerQueue.IsEmpty)
        {
            GameObject passenger = passengerQueue.Dequeue();
            Destroy(passenger);
            Debug.Log("Player: Passenger entregado");
        }
    }

    public int GetPassengerCount()
    {
        return passengerQueue.Count;
    }
}

