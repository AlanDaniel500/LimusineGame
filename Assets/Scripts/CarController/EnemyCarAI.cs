using UnityEngine;


public class EnemyCarAI : MonoBehaviour
{
    private TopDownCarController carController;
    private Transform currentTarget;
    private bool hasPassenger = false;
    private CambiarEscena cambiarEscena;


    private MyQueue<GameObject> passengerQueue = new MyQueue<GameObject>();

    private void Awake()
    {
        carController = GetComponent<TopDownCarController>();
        cambiarEscena = FindObjectOfType<CambiarEscena>();

    }

    private void Start()
    {
        FindPassenger();
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

                if (currentTarget != null && currentTarget.CompareTag("Passenger"))
                {
                    passengerQueue.Enqueue(currentTarget.gameObject);
                }

                FindDestination();
            }
            else
            {
                if (!passengerQueue.IsEmpty)
                {
                    GameObject deliveredPassenger = passengerQueue.Dequeue();
                    Destroy(deliveredPassenger);
                    Debug.Log("Enemy: Passenger entregado");
                    FinishGame();
                }

                currentTarget = null;
                hasPassenger = false;
            }
        }

        carController.SetInputVector(new Vector2(turnAmount, forwardAmount));
    }

    private void FindPassenger()
    {
        GameObject passenger = GameObject.FindGameObjectWithTag("Passenger");
        if (passenger != null)
        {
            currentTarget = passenger.transform;
        }
        else
        {
            Debug.LogWarning("Enemy: No se encontró Passenger en la escena.");
        }
    }

    private void FindDestination()
    {
        GameObject destination = GameObject.FindGameObjectWithTag("Destination");
        if (destination != null)
        {
            currentTarget = destination.transform;
        }
        else
        {
            Debug.LogWarning("Enemy: No se encontró Destination en la escena.");
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

    private void FinishGame()
    {
        Debug.Log("Enemy: Todos los pasajeros fueron entregados! Derrota para el jugador.");

        if (cambiarEscena != null)
        {
            cambiarEscena.IrADerrota();
        }
        else
        {
            Debug.LogError("Enemy: No se encontró el script CambiarEscena en la escena.");
        }
    }
}



