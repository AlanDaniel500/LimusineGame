using System.Collections.Generic;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject passengerPrefab;
    [SerializeField] private GameObject destinationPrefab;

    private Nodo[] nodos;
    private Transform playerTransform;

    private void Awake()
    {
        nodos = FindObjectsOfType<Nodo>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("No se encontró el objeto con tag 'Player'");
        }
    }

    public GameObject SpawnPassengerWithDestination()
    {
        if (nodos.Length < 2 || playerTransform == null)
        {
            Debug.LogError("Se necesitan al menos 2 nodos y una referencia al jugador.");
            return null;
        }

        // 1. Crear lista de nodos y ordenarla por distancia al jugador
        List<GameObject> nodoList = new List<GameObject>();
        foreach (var nodo in nodos)
        {
            nodoList.Add(nodo.gameObject);
        }

        QuickSortUtility.QuickSortByDistance(nodoList, playerTransform);

        // 2. Elegir un nodo cercano pero no el más cercano
        int rangoMin = 1;
        int rangoMax = Mathf.Min(4, nodoList.Count - 1); // Hasta el 4º más cercano
        int indexPasajero = Random.Range(rangoMin, rangoMax + 1);
        Nodo nodoPasajero = nodoList[indexPasajero].GetComponent<Nodo>();

        // 3. Elegir un destino suficientemente alejado del pasajero
        Nodo nodoDestino;
        int intentos = 0;
        do
        {
            nodoDestino = nodos[Random.Range(0, nodos.Length)];
            intentos++;

            if (intentos > 10) break; // evitar bucle infinito
        } while (nodoDestino == nodoPasajero ||
                 Vector2.Distance(nodoDestino.Posicion, nodoPasajero.Posicion) < 5f);

        // 4. Instanciar pasajero y destino
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

