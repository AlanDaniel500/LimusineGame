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

    // M�todo que se ejecuta al presionar el bot�n o tecla definida en el Input System
    public void OnRequestMenu(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            Debug.Log("�Bot�n de men� presionado!"); 
            ToggleMenu(); // Alternamos el estado del men�
        }
    }

    // M�todo que se llama cuando se presiona el bot�n "Aceptar" del men�
    private void OnAcceptButtonPressed()
    {
        Debug.Log("�Bot�n de 'Aceptar' presionado!"); 
        ToggleMenu(); 
    }

    // M�todo que abre y cierra el men�
    private void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen; // Cambia el estado del men� (abierto/cerrado)
        requestMenu.SetActive(isMenuOpen); 
        acceptText.gameObject.SetActive(isMenuOpen); 

        if (!isMenuOpen) // Si el men� se cierra Activamos el pasajero
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