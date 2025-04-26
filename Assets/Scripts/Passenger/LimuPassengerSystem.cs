using UnityEngine;

public class LimuPassengerSystem : MonoBehaviour
{
    [SerializeField] private PassengerSpawner passengerSpawner;
    [SerializeField] private GameObject destinationObject;

    private MyQueue<GameObject> passengerQueue = new MyQueue<GameObject>();
    private TopDownCarController carController;

    private void Start()
    {
        carController = GetComponent<TopDownCarController>();
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






