using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject passengerPrefab;
    [SerializeField] private GameObject destinationPrefab;

    [Header("Rangos")]
    [SerializeField] private Vector2 rangoSpawnMin;
    [SerializeField] private Vector2 rangoSpawnMax;

    public GameObject SpawnPassengerWithDestination()
    {
        Vector2 passengerPos = new Vector2(
            Random.Range(rangoSpawnMin.x, rangoSpawnMax.x),
            Random.Range(rangoSpawnMin.y, rangoSpawnMax.y)
        );

        Vector2 destinationPos;
        int maxTries = 15;
        float minDistance = 10f;
        int tries = 0;

        do
        {
            destinationPos = new Vector2(
                Random.Range(rangoSpawnMin.x, rangoSpawnMax.x),
                Random.Range(rangoSpawnMin.y, rangoSpawnMax.y)
            );
            tries++;
        }
        while (Vector2.Distance(passengerPos, destinationPos) < minDistance && tries < maxTries);

        GameObject passenger = Instantiate(passengerPrefab, passengerPos, Quaternion.identity);
        passenger.tag = "Passenger";

        GameObject destination = Instantiate(destinationPrefab, destinationPos, Quaternion.identity);
        destination.tag = "Destination";

        PassengerTag tag = passenger.GetComponent<PassengerTag>();
        if (tag == null) tag = passenger.AddComponent<PassengerTag>();
        tag.destination = destination;

        return passenger;
    }
}



