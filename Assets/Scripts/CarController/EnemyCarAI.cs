using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarAI : MonoBehaviour
{
    private TopDownCarController carController;
    private CambiarEscena cambiarEscena;
    private GraphManager graphManager;

    private GameObject currentPassenger = null;
    private Transform destinationTarget = null;

    private Queue<Nodo> currentPath = new Queue<Nodo>();
    private Nodo currentNodoObjetivo = null;

    private MyQueue<GameObject> passengerQueue = new MyQueue<GameObject>();
    private bool hasPassenger = false;

    private void Awake()
    {
        carController = GetComponent<TopDownCarController>();
        cambiarEscena = FindFirstObjectByType<CambiarEscena>();
        graphManager = FindFirstObjectByType<GraphManager>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.3f);
        FindNewObjective();
    }

    private void Update()
    {
        if (currentPath.Count == 0)
        {
            FindNewObjective();
            return;
        }

        Nodo nodoDestino = currentPath.Peek();
        Vector2 dir = ((Vector2)nodoDestino.Posicion - (Vector2)transform.position).normalized;
        float angle = Vector2.SignedAngle(transform.up, dir);
        float turnAmount = Mathf.Clamp(angle / 45f, -1f, 1f);
        float forwardAmount = Vector2.Dot(transform.up, dir) > 0 ? 1f : -1f;

        float distancia = Vector2.Distance(transform.position, nodoDestino.Posicion);
        if (distancia < 0.5f)
        {
            currentPath.Dequeue();

            if (currentPath.Count == 0)
            {
                if (!hasPassenger)
                {
                    TryPickupPassenger();
                }
                else
                {
                    DeliverPassenger();
                }
                return;
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
        hasPassenger = true;

        destinationTarget = tag.destination?.transform;
        FindPathTo(destinationTarget.position);
        Debug.Log("Enemy: Passenger recogido");
    }

    private void DeliverPassenger()
    {
        if (passengerQueue.IsEmpty) return;

        GameObject passenger = passengerQueue.Dequeue();

        PassengerManager manager = FindFirstObjectByType<PassengerManager>();
        if (manager != null)
        {
            manager.NotifyPassengerDelivered(passenger);
        }

        PassengerTag tag = passenger.GetComponent<PassengerTag>();
        if (tag != null && tag.destination != null)
        {
            Destroy(tag.destination);
        }

        Destroy(passenger);
        Debug.Log("Enemy: Passenger entregado");

        hasPassenger = false;
        currentPassenger = null;
        destinationTarget = null;

        FinishGame();
        StartCoroutine(DelayedSearch());
    }

    private IEnumerator DelayedSearch()
    {
        yield return new WaitForSeconds(0.3f);
        FindNewObjective();
    }

    private void FindNewObjective()
    {
        if (!hasPassenger)
        {
            GameObject[] passengers = GameObject.FindGameObjectsWithTag("Passenger");
            foreach (GameObject p in passengers)
            {
                PassengerTag tag = p.GetComponent<PassengerTag>();
                if (tag != null && !tag.isTaken)
                {
                    currentPassenger = p;
                    FindPathTo(p.transform.position);
                    Debug.Log("Enemy: Encontré passenger libre.");
                    return;
                }
            }

            // No hay pasajeros, buscar destinos para interceptar
            GameObject[] destinations = GameObject.FindGameObjectsWithTag("Destination");
            if (destinations.Length > 0)
            {
                FindPathTo(destinations[0].transform.position);
                Debug.Log("Enemy: No hay Passenger, voy a un destino.");
            }
            else
            {
                Debug.LogWarning("Enemy: No hay Passenger ni destino.");
            }
        }
        else if (destinationTarget != null)
        {
            FindPathTo(destinationTarget.position);
        }
    }

    private void FindPathTo(Vector2 objetivo)
    {
        Nodo origen = graphManager.GetNodoCercano(transform.position);
        Nodo destino = graphManager.GetNodoCercano(objetivo);

        if (origen == null || destino == null)
        {
            Debug.LogWarning("Enemy: Nodos inválidos para pathfinding.");
            return;
        }

        List<Nodo> path = Dijkstra(origen, destino);
        currentPath = new Queue<Nodo>(path);
    }

    private List<Nodo> Dijkstra(Nodo inicio, Nodo destino)
    {
        Dictionary<Nodo, float> distancias = new Dictionary<Nodo, float>();
        Dictionary<Nodo, Nodo> anteriores = new Dictionary<Nodo, Nodo>();
        List<Nodo> nodosNoVisitados = new List<Nodo>(graphManager.todosLosNodos);

        foreach (Nodo nodo in nodosNoVisitados)
        {
            distancias[nodo] = float.MaxValue;
        }

        distancias[inicio] = 0;

        while (nodosNoVisitados.Count > 0)
        {
            nodosNoVisitados.Sort((a, b) => distancias[a].CompareTo(distancias[b]));
            Nodo actual = nodosNoVisitados[0];
            nodosNoVisitados.RemoveAt(0);

            if (actual == destino)
                break;

            foreach (Nodo vecino in actual.vecinos)
            {
                float nuevaDist = distancias[actual] + Vector2.Distance(actual.Posicion, vecino.Posicion);
                if (nuevaDist < distancias[vecino])
                {
                    distancias[vecino] = nuevaDist;
                    anteriores[vecino] = actual;
                }
            }
        }

        List<Nodo> camino = new List<Nodo>();
        Nodo nodoActual = destino;
        while (anteriores.ContainsKey(nodoActual))
        {
            camino.Insert(0, nodoActual);
            nodoActual = anteriores[nodoActual];
        }

        if (camino.Count == 0) camino.Add(destino);
        return camino;
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
