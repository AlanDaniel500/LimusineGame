using UnityEngine;
using UnityEngine.SceneManagement;

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
    private int entregadosTotal = 0;

    private void Start()
    {
        carController = GetComponent<TopDownCarController>();
        cambiarEscena = FindFirstObjectByType<CambiarEscena>();

        // Aplicar mejora de velocidad según PlayerSpeedManager
        originalMaxSpeed = carController.maxSpeed * PlayerSpeedManager.Instance.SpeedMultiplier;
        carController.maxSpeed = originalMaxSpeed;
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

                // Cambiar flecha a destino
                if (tag.destination != null)
                {
                    ArrowIndicator arrow = FindFirstObjectByType<ArrowIndicator>();
                    if (arrow != null)
                    {
                        arrow.SetTarget(tag.destination.transform, true); // true = destino
                    }
                }
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

    public GameObject PeekPassenger()
    {
        return passengerQueue.IsEmpty ? null : passengerQueue.Peek();
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

            Destroy(passenger); // destruir pasajero

            entregadosTotal++;
            Debug.Log("Player: Passenger entregado");

            // Volver a apuntar al siguiente pasajero
            ArrowIndicator arrow = FindFirstObjectByType<ArrowIndicator>();
            if (arrow != null && manager != null)
            {
                GameObject nextPassenger = manager.GetClosestPassenger();
                if (nextPassenger != null)
                {
                    arrow.SetTarget(nextPassenger.transform, false); // false = buscar pasajero
                }
            }
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
                string currentScene = SceneManager.GetActiveScene().name;

                if (currentScene == "SampleScene")
                {
                    GameSessionManager.Instance.Entregados = entregadosTotal;
                    cambiarEscena.FinishLevel();
                }
                else if (currentScene == "SampleScene2")
                {
                    cambiarEscena.IrAVictoria();
                }
                else
                {
                    Debug.LogWarning("No se reconoció la escena actual. No se cambiará de escena.");
                }
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

