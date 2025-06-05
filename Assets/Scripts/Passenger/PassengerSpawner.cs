using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject passengerPrefab;
    [SerializeField] private GameObject destinationPrefab;

    private Nodo[] nodos;

    private void Awake()
    {
        nodos = FindObjectsOfType<Nodo>();
    }

    public GameObject SpawnPassengerWithDestination()
    {
        if (nodos.Length < 2)
        {
            Debug.LogError("Se necesitan al menos 2 nodos para spawnear.");
            return null;
        }

        Nodo nodoPasajero, nodoDestino;
        do
        {
            nodoPasajero = nodos[Random.Range(0, nodos.Length)];
            nodoDestino = nodos[Random.Range(0, nodos.Length)];
        } while (nodoPasajero == nodoDestino);

        GameObject passenger = Instantiate(passengerPrefab, nodoPasajero.Posicion, Quaternion.identity);
        passenger.tag = "Passenger";

        GameObject destination = Instantiate(destinationPrefab, nodoDestino.Posicion, Quaternion.identity);
        destination.tag = "Destination";

        PassengerTag tag = passenger.GetComponent<PassengerTag>();
        if (tag == null) tag = passenger.AddComponent<PassengerTag>();
        tag.destination = destination;

        return passenger;
    }
}

