using UnityEngine;

public class Passenger : MonoBehaviour
{
    public Transform destination;
    private bool isPickedUp = false;

    void Start()
    {
        gameObject.SetActive(false); // El pasajero empieza desactivado
    }

    public void Pickup()
    {
        if (!isPickedUp)
        {
            isPickedUp = true;
            gameObject.SetActive(false); // Ocultamos al pasajero después de recogerlo
        }
    }

    public void DropOff()
    {
        if (isPickedUp)
        {
            Destroy(gameObject); // El pasajero desaparece al llegar al destino
        }
    }
}