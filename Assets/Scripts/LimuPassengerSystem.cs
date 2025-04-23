using UnityEngine;
using System.Collections.Generic;

public class LimuPassengerSystem : MonoBehaviour
{
 
    private List<GameObject> passengers = new List<GameObject>();
    private List<Transform> destinations = new List<Transform>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Passenger"))
        {
            GameObject passenger = collision.gameObject;

            // Recoger pasajero
            passenger.SetActive(false); // Lo ocultamos
            Debug.Log("Pasajero recogido");
        }


        if (collision.CompareTag("Destination"))
        {
            DropOffPassenger(collision.transform);
        }
    }

    private void DropOffPassenger(Transform dropOffPoint)
    {
        for (int i = passengers.Count - 1; i >= 0; i--)
        {
            if (destinations[i] == dropOffPoint)
            {
                Debug.Log("Pasajero entregado a destino.");
                Destroy(passengers[i]);
                passengers.RemoveAt(i);
                destinations.RemoveAt(i);
            }
        }
    }
}
