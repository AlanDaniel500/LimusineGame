using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI; 

public class PassengerRequestSystem : MonoBehaviour
{
    [SerializeField] private GameObject requestMenu; // Imagen con botones
    [SerializeField] private TextMeshProUGUI acceptText; // Texto de "Aceptar"
    [SerializeField] private GameObject passengerObject; // El objeto del pasajero que se activar�
    [SerializeField] private Button acceptButton; // Bot�n "Aceptar" en el men�

    private bool isMenuOpen = false;

    void Start()
    {
        requestMenu.SetActive(false); 
        acceptText.gameObject.SetActive(false); 
        passengerObject.SetActive(false); 

        // Asegur�ndonos de que el bot�n de "Aceptar" est� asociado al evento
        acceptButton.onClick.AddListener(OnAcceptButtonPressed);
    }

    // Metodo que se ejecuta al presionar el bot�n o tecla definida en el Input System
    public void OnRequestMenu(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            Debug.Log("�El bot�n de men� fue presionado!"); 
            ToggleMenu(); // Alternamos el estado del menu
        }
    }

    // Metodo que se llama cuando se presiona el bot�n "Aceptar" del men�
    private void OnAcceptButtonPressed()
    {
        Debug.Log("�El boton de 'Aceptar' fue presionado!"); 
        ToggleMenu(); 
    }

    // Metodo que abre y cierra el menu
    private void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen; // Cambia el estado del menu (abierto/cerrado)
        requestMenu.SetActive(isMenuOpen); 
        acceptText.gameObject.SetActive(isMenuOpen); 

        if (!isMenuOpen) // Si el menu se cierra Activamos el pasajero
        {
            ActivatePassenger();
        }
    }

    private void ActivatePassenger()
    {
        passengerObject.SetActive(true); 
        Debug.Log("�Pasajero activado!");
    }
}