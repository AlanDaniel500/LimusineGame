using UnityEngine;

public class LimuPassengerSystem : MonoBehaviour
{
    [SerializeField] private PassengerSpawner passengerSpawner; // LO DEJAMOS PARA EL FUTURO.
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
            passengerQueue.Enqueue(collision.gameObject);

            collision.gameObject.SetActive(false);
            Debug.Log("Player: Pasajero recogido.");
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


    //  NO BORRAR , DEJARLO PARA FUTURO

    //if (collision.gameObject.CompareTag("Destination"))
    //{
    //    if (!passengerQueue.IsEmpty)
    //    {
    //        DeliverPassenger();
    //        passengerSpawner.SpawnNewPassenger();
    //    }
    //}



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

    private void FinishGame()
    {
        Debug.Log("Todos los pasajeros fueron entregados!");

        if (cambiarEscena != null)
        {
            cambiarEscena.IrAVictoria();
        }
        else
        {
            Debug.LogError("No se encontró CambiarEscena en la escena.");
        }
    }

    public int GetPassengerCount()
    {
        return passengerQueue.Count;
    }
}

