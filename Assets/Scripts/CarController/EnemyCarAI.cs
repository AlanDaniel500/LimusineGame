using System.Collections;
using UnityEngine;

public class EnemyCarAI : MonoBehaviour
{
    private TopDownCarController carController;
    private Transform currentTarget;
    private bool hasPassenger = false;
    private CambiarEscena cambiarEscena;

    private GameObject currentPassenger = null;
    private Transform destinationTarget = null;
    private MyQueue<GameObject> passengerQueue = new MyQueue<GameObject>();

    private void Awake()
    {
        carController = GetComponent<TopDownCarController>();
        cambiarEscena = FindFirstObjectByType<CambiarEscena>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.3f);
        FindPassengerOrDestination();
    }

    private void Update()
    {
        if (!hasPassenger)
        {
            if (currentPassenger != null)
            {
                PassengerTag tag = currentPassenger.GetComponent<PassengerTag>();

                if (tag == null || tag.isTaken)
                {
                    if (tag != null && tag.takenBy != gameObject && tag.destination != null)
                    {
                        destinationTarget = tag.destination.transform;
                        currentTarget = destinationTarget;
                        currentPassenger = null;
                        Debug.Log("Enemy: Me robaron el pasajero. Voy al destino.");
                    }
                    else
                    {
                        currentPassenger = null;
                        currentTarget = null;
                        Debug.Log("Enemy: Passenger ya no es válido. Busco uno nuevo.");
                        FindPassengerOrDestination();
                    }
                }
            }
            else
            {
                if (currentTarget == null || !currentTarget.CompareTag("Destination"))
                {
                    Debug.Log("Enemy: Sin passenger ni destino. Busco uno nuevo.");
                    FindPassengerOrDestination();
                }
            }
        }

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
                TryPickupPassenger();
            }
            else
            {
                DeliverPassenger();
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

        hasPassenger = true;
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

        hasPassenger = false;
        currentPassenger = null;
        currentTarget = null;

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

        GameObject[] destinations = GameObject.FindGameObjectsWithTag("Destination");
        if (destinations.Length > 0)
        {
            currentTarget = destinations[0].transform;
            Debug.Log("Enemy: No hay Passenger libre, voy a un destino.");
        }
        else
        {
            Debug.LogWarning("Enemy: No hay passenger ni destino.");
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
