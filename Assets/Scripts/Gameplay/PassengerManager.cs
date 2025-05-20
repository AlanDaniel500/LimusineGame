using System.Collections.Generic;
using UnityEngine;

public class PassengerManager : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private PassengerSpawner spawner;
    [SerializeField] private int totalPassengers = 5;

    private int deliveredCount = 0;
    private List<GameObject> activePassengers = new List<GameObject>();
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        SpawnNextPassenger();
    }

    public void SpawnNextPassenger()
    {
        if (deliveredCount >= totalPassengers) return;

        GameObject newPassenger = spawner.SpawnPassengerWithDestination();
        activePassengers.Add(newPassenger);
    }

    public void NotifyPassengerDelivered(GameObject passenger)
    {
        deliveredCount++;
        activePassengers.Remove(passenger);

        if (deliveredCount < totalPassengers)
        {
            SpawnNextPassenger();
        }
        else
        {
            Debug.Log("Todos los pasajeros fueron entregados.");
        }
    }

    public GameObject GetClosestPassenger()
    {
        if (player == null || activePassengers.Count == 0) return null;

        QuickSortUtility.QuickSortByDistance(activePassengers, player);
        return activePassengers[0];
    }

    public bool AllDelivered()
    {
        return deliveredCount >= totalPassengers;
    }
}
