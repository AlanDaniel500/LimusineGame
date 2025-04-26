using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [Header("Configuraciones")]
    [SerializeField] private GameObject passengerPrefab;
    [SerializeField] private Vector2 rangoSpawnMin;
    [SerializeField] private Vector2 rangoSpawnMax;

    public void SpawnNewPassenger()
    {
        Vector2 randomPos = new Vector2(
            Random.Range(rangoSpawnMin.x, rangoSpawnMax.x),
            Random.Range(rangoSpawnMin.y, rangoSpawnMax.y)
        );

        GameObject newPassenger = Instantiate(passengerPrefab, randomPos, Quaternion.identity);
        newPassenger.tag = "Passenger";
        if (newPassenger.GetComponent<PassengerTag>() == null)
            newPassenger.AddComponent<PassengerTag>();
    }
}


