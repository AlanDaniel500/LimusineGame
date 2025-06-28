using UnityEngine;

public class LimuPassengerSystem : MonoBehaviour
{
    [SerializeField] private PassengerSpawner passengerSpawner;
    [SerializeField] private GameObject destinationObject;

    private MyQueue<GameObject> passengerQueue = new MyQueue<GameObject>();
    private MyStack<PowerUpSpeed> powerUpStack = new MyStack<PowerUpSpeed>();
    private TopDownCarController carController;
    private CambiarEscena cambiarEscena;

    private float originalMaxSpeed;
    private bool isBoosted = false;
    private float boostTimer = 0f;

    private void Start()
    {
        carController = GetComponent<TopDownCarController>();
        originalMaxSpeed = carController.maxSpeed;

        carController.maxSpeed *= GameSessionManager.Instance.speedMultiplier;
        cambiarEscena = FindFirstObjectByType<CambiarEscena>();
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
            PassengerTag tag = collision.GetComponent<PassengerTag>();
            if (tag != null && !tag.isTaken)
            {
                tag.isTaken = true;
                tag.takenBy = gameObject;

                passengerQueue.Enqueue(collision.gameObject);
                collision.gameObject.SetActive(false);
                Debug.Log("Player: Pasajero recogido.");
            }
        }

        if (collision.CompareTag("Destination"))
        {
            if (!passengerQueue.IsEmpty)
            {
                DeliverPassenger();
                FinishGame();
            }
        }

        if (collision.CompareTag("PowerUp"))
        {
            PowerUpSpeed powerUp = collision.GetComponent<PowerUpSpeed>();
            if (powerUp != null)
            {
                powerUpStack.Push(powerUp);
                Debug.Log("Player: PowerUp recogido.");
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
                    PassengerTag tag = stolenPassenger.GetComponent<PassengerTag>();
                    if (tag != null)
                    {
                        tag.isTaken = true;
                        tag.takenBy = gameObject;
                    }

                    passengerQueue.Enqueue(stolenPassenger);
                    Debug.Log("Player: ¡Robé un Pasajero al Enemigo!");
                }
            }
        }
    }

    private void ApplyNextPowerUp()
    {
        if (!powerUpStack.IsEmpty)
        {
            PowerUpSpeed powerUp = powerUpStack.Pop();

            carController.maxSpeed = originalMaxSpeed * powerUp.speedMultiplier;
            boostTimer = powerUp.duration;
            isBoosted = true;

            Debug.Log($"Player: PowerUp x{powerUp.speedMultiplier} por {powerUp.duration} seg.");
        }
    }

    public void DeliverPassenger()
    {
        if (!passengerQueue.IsEmpty)
        {
            GameObject passenger = passengerQueue.Dequeue();

            PassengerManager manager = FindFirstObjectByType<PassengerManager>();
            if (manager != null)
            {
                manager.NotifyPassengerDelivered(passenger);
            }

            PassengerTag tag = passenger.GetComponent<PassengerTag>();
            if (tag != null && tag.destination != null)
            {
                Destroy(tag.destination); // destruir destino
            }

            Destroy(passenger); //  destruir pasajero
            Debug.Log("Player: Passenger entregado");
        }
    }

    private void FinishGame()
    {
        PassengerManager manager = FindFirstObjectByType<PassengerManager>();

        if (manager != null && manager.AllDelivered())
        {
            Debug.Log("Todos los pasajeros fueron entregados!");

            if (cambiarEscena != null)
            {
                GameSessionManager.Instance.upgradePoints = GameSessionManager.Instance.deliveredPassengers;
                cambiarEscena.FinishLevel(); // va a UpgradeScene
            }
        }
    }

    public int GetPassengerCount() => passengerQueue.Count;

    public bool HasPassengers() => !passengerQueue.IsEmpty;

    public GameObject StealPassenger()
    {
        if (!passengerQueue.IsEmpty)
        {
            return passengerQueue.Dequeue();
        }
        return null;
    }
}

