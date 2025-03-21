using UnityEngine;

public class Passenger : MonoBehaviour
{
    public Transform destination; // El punto donde debe ser dejado
    private bool isPickedUp = false;

    public void Pickup()
    {
        if (!isPickedUp)
        {
            isPickedUp = true;
            gameObject.SetActive(false); // Ocultamos al pasajero
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
