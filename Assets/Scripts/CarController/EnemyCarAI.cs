using System.Collections;
using UnityEngine;

public class EnemyCarAI : MonoBehaviour
{
    private TopDownCarController carController;
    private Transform currentTarget;
    private Transform destinationTarget = null;
    private CambiarEscena cambiarEscena;

    private GameObject currentPassenger = null;
    private MyQueue<GameObject> passengerQueue = new MyQueue<GameObject>();

    private bool HasPassenger => !passengerQueue.IsEmpty;

    private void Awake()
    {
        carController = GetComponent<TopDownCarController>();
        cambiarEscena = FindFirstObjectByType<CambiarEscena>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        FindPassengerOrDestination();
    }

    private void Update()
    {
        //  Si le robaron el pasajero en el camino
        if (!HasPassenger && destinationTarget != null && currentTarget == destinationTarget)
        {
            Debug.Log("Enemy: Me robaron el pasajero. Cambio de objetivo.");
            destinationTarget = null;
            currentTarget = null;
            FindPassengerOrDestination();
            return;
        }

        if (currentTarget == null)
        {
            FindPassengerOrDestination();
            return;
        }

        //  Si el passenger fue tomado antes de que llegue
        if (!HasPassenger && currentPassenger != null)
        {
            PassengerTag tag = currentPassenger.GetComponent<PassengerTag>();

            if (tag == null || tag.isTaken)
            {
                if (tag != null && tag.takenBy != gameObject && tag.destination != null)
                {
                    Debug.Log("Enemy: Me robaron el pasajero. Cambio de objetivo.");
                    destinationTarget = tag.destination.transform;
                    currentTarget = destinationTarget;
                    currentPassenger = null;
                    return;
                }

                // Si el tag no existe o el destino ya fue destruido
                currentPassenger = null;
                currentTarget = null;
                FindPassengerOrDestination();
                return;
            }
        }


        // Movimiento hacia el target
        Vector2 dirToTarget = ((Vector2)currentTarget.position - (Vector2)transform.position).normalized;
        float angleToTarget = Vector2.SignedAngle(transform.up, dirToTarget);
        float turnAmount = Mathf.Clamp(angleToTarget / 45f, -1f, 1f);
        float forwardAmount = Vector2.Dot(transform.up, dirToTarget) > 0 ? 1f : -1f;

        float distance = Vector2.Distance(transform.position, currentTarget.position);
        if (distance < 0.5f)
        {
            forwardAmount = 0f;
            turnAmount = 0f;

            if (HasPassenger)
            {
                DeliverPassenger();
            }
            else
            {
                TryPickupPassenger();
            }
        }

        carController.SetInputVector(new Vector2(turnAmount, forwardAmount));
    }

    private void TryPickupPassenger()
    {
        if (currentPassenger == null) return;

        PassengerTag tag = currentPassenger.GetComponent<PassengerTag>();
        if (tag == null || tag.isTaken) return;

        tag.isTaken = true;
        tag.takenBy = gameObject;

        passengerQueue.Enqueue(currentPassenger);
        currentPassenger.SetActive(false);
        Debug.Log("Enemy: Passenger recogido");

        destinationTarget = tag.destination != null ? tag.destination.transform : null;
        currentTarget = destinationTarget;
    }

    private void DeliverPassenger()
    {
        if (passengerQueue.IsEmpty) return;

        GameObject deliveredPassenger = passengerQueue.Dequeue();

        PassengerManager manager = FindFirstObjectByType<PassengerManager>();
        if (manager != null)
        {
            manager.NotifyPassengerDelivered(deliveredPassenger);
        }

        PassengerTag tag = deliveredPassenger.GetComponent<PassengerTag>();
        if (tag != null && tag.destination != null)
        {
            Destroy(tag.destination); 
        }

        Destroy(deliveredPassenger);
        Debug.Log("Enemy: Passenger entregado");

        currentPassenger = null;
        currentTarget = null;
        destinationTarget = null;

        FinishGame();
        StartCoroutine(DelayedSearch());
    }

    private IEnumerator DelayedSearch()
    {
        yield return new WaitForSeconds(0.5f);
        FindPassengerOrDestination();
    }

    private void FindPassengerOrDestination()
    {
        GameObject[] passengers = GameObject.FindGameObjectsWithTag("Passenger");

        foreach (GameObject p in passengers)
        {
            PassengerTag tag = p.GetComponent<PassengerTag>();
            if (tag != null && !tag.isTaken)
            {
                currentPassenger = p;
                currentTarget = p.transform;
                Debug.Log("Enemy: Encontré Passenger libre.");
                return;
            }
        }

        // Fallback: ir a cualquier destino si no queda passenger
        GameObject[] destinations = GameObject.FindGameObjectsWithTag("Destination");
        if (destinations.Length > 0)
        {
            currentTarget = destinations[0].transform;
            destinationTarget = currentTarget;
            Debug.Log("Enemy: No hay Passenger libre, voy a un destino.");
        }
        else
        {
            Debug.LogWarning("Enemy: No hay Passenger ni Destination en la escena.");
            currentTarget = null;
        }
    }

    public bool HasPassengers() => !passengerQueue.IsEmpty;

    public GameObject StealPassenger()
    {
        if (!passengerQueue.IsEmpty)
        {
            return passengerQueue.Dequeue();
        }
        return null;
    }

    private void FinishGame()
    {
        PassengerManager manager = FindFirstObjectByType<PassengerManager>();
        if (manager != null && manager.AllDelivered())
        {
            Debug.Log("Enemy: Todos los pasajeros fueron entregados. Derrota para el jugador.");
            if (cambiarEscena != null)
            {
                cambiarEscena.IrADerrota();
            }
        }
    }
}