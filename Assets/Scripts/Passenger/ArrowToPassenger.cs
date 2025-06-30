using UnityEngine;
using UnityEngine.UI;

public class ArrowIndicator : MonoBehaviour
{
    public Transform player;                // Jugador
    public RectTransform arrowUI;          // UI de la flecha
    public Color buscarColor = Color.yellow;
    public Color destinoColor = Color.blue; // Color al ir al destino

    private PassengerManager manager;
    private LimuPassengerSystem playerPassengerSystem;
    private Transform target;
    private Image arrowImage;

    private bool haciaDestino = false;

    void Start()
    {
        manager = FindFirstObjectByType<PassengerManager>();
        playerPassengerSystem = player.GetComponent<LimuPassengerSystem>();
        arrowImage = arrowUI.GetComponent<Image>();
        arrowImage.color = buscarColor;
    }

    void Update()
    {
        if (!haciaDestino && target == null && manager != null)
        {
            GameObject closest = manager.GetClosestPassenger();
            if (closest != null)
            {
                SetTarget(closest.transform, false); // Apunta al pasajero
            }
        }

        if (target != null && player != null)
        {
            Vector2 dir = (target.position - player.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrowUI.rotation = Quaternion.Euler(0, 0, angle);
            arrowUI.gameObject.SetActive(true);
        }
        else
        {
            arrowUI.gameObject.SetActive(false);
        }
    }

    // Usá este método para actualizar el objetivo de la flecha
    public void SetTarget(Transform newTarget, bool esDestino)
    {
        target = newTarget;
        haciaDestino = esDestino;

        if (arrowImage != null)
        {
            arrowImage.color = esDestino ? destinoColor : buscarColor;
        }

        arrowUI.gameObject.SetActive(newTarget != null);
    }
}
