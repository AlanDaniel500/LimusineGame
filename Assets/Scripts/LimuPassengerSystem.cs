using UnityEngine;
using System.Collections.Generic;

public class LimuPassengerSystem : MonoBehaviour
{
    [SerializeField] private int maxPassengers = 4;
    private List<Passenger> passengers = new List<Passenger>();

    public void AddPassenger(Passenger passenger)
    {
        if (passengers.Count < maxPassengers)
        {
            passenger.Pickup();
            passengers.Add(passenger);
            Debug.Log("Pasajero aceptado.");
        }
        else
        {
            Debug.Log("La limusina está llena");
        }
    }
}


