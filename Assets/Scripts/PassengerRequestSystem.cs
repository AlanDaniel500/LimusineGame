using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI; 

public class PassengerRequestSystem : MonoBehaviour
{
    [SerializeField] private GameObject requestMenu; // Imagen con botones
    [SerializeField] private TextMeshProUGUI acceptText; // Texto de "Aceptar"
    [SerializeField] private GameObject passengerObject; // El objeto del pasajero que se activará
    [SerializeField] private Button acceptButton; // Botón "Aceptar" en el menú

    private bool isMenuOpen = false;

    void Start()
    {
        requestMenu.SetActive(false); 
        acceptText.gameObject.SetActive(false); 
        passengerObject.SetActive(false); 

        // Asegurándonos de que el botón de "Aceptar" esté asociado al evento
        acceptButton.onClick.AddListener(OnAcceptButtonPressed);
    }

    // Método que se ejecuta al presionar el botón o tecla definida en el Input System
    public void OnRequestMenu(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            Debug.Log("¡Botón de menú presionado!"); 
            ToggleMenu(); // Alternamos el estado del menú
        }
    }

    // Método que se llama cuando se presiona el botón "Aceptar" del menú
    private void OnAcceptButtonPressed()
    {
        Debug.Log("¡Botón de 'Aceptar' presionado!"); 
        ToggleMenu(); 
    }

    // Método que abre y cierra el menú
    private void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen; // Cambia el estado del menú (abierto/cerrado)
        requestMenu.SetActive(isMenuOpen); 
        acceptText.gameObject.SetActive(isMenuOpen); 

        if (!isMenuOpen) // Si el menú se cierra Activamos el pasajero
        {
            ActivatePassenger();
        }
    }

    private void ActivatePassenger()
    {
        passengerObject.SetActive(true); 
        Debug.Log("¡Pasajero activado!");
    }
}