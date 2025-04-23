using UnityEngine;
using System.Collections.Generic;

public class LimuPassengerSystem : MonoBehaviour
{
    [SerializeField] private int maxPassengers = 4;
    private List<Passenger> passengers = new List<Passenger>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Passenger") && passengers.Count < maxPassengers)
        {
            Passenger passenger = other.GetComponent<Passenger>();
            if (passenger != null)
            {
                passenger.Pickup();
                passengers.Add(passenger);
                Debug.Log("Pasajero recogido. Total: " + passengers.Count);
            }
        }

        if (other.CompareTag("DropOffPoint"))
        {
            DropOffPassengers(other.transform);
        }
    }

    private void DropOffPassengers(Transform dropOffPoint)
    {
        for (int i = passengers.Count - 1; i >= 0; i--)
        {
            if (passengers[i].destination == dropOffPoint)
            {
                passengers[i].DropOff();
                passengers.RemoveAt(i);
                Debug.Log("Pasajero dejado en destino. Restantes: " + passengers.Count);
            }
        }
    }
}

