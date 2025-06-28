using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public Transform player;                // Jugador
    public RectTransform arrowUI;          // UI de la flecha
    public Color buscarColor = Color.yellow;
    public Color destinoColor = Color.green;

    private PassengerManager manager;
    private LimuPassengerSystem playerPassengerSystem;
    private Transform target;

    private UnityEngine.UI.Image arrowImage;

    void Start()
    {
        manager = FindObjectOfType<PassengerManager>();
        playerPassengerSystem = player.GetComponent<LimuPassengerSystem>();
        arrowImage = arrowUI.GetComponent<UnityEngine.UI.Image>();
    }

    void Update()
    {
        if (target == null && manager != null)
        {
            GameObject closest = manager.GetClosestPassenger();
            if (closest != null)
                target = closest.transform;
        }

        if (target != null && player != null)
        {
            Vector2 dir = (target.position - player.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrowUI.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            arrowUI.gameObject.SetActive(false); // Oculta la flecha si no hay target
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        arrowUI.gameObject.SetActive(newTarget != null);
    }



}





