using UnityEngine;
public class EnemyCarAI : MonoBehaviour
{
    [Header("Asignación Manual")]
    [SerializeField] private GameObject passengerObject;
    [SerializeField] private GameObject destinationObject;

    private TopDownCarController carController;
    private Transform currentTarget;
    private bool hasPassenger = false;

    private void Awake()
    {
        carController = GetComponent<TopDownCarController>();
    }

    private void Start()
    {
        if (passengerObject != null)
        {
            currentTarget = passengerObject.transform;
        }
    }

    private void Update()
    {
        if (currentTarget == null) return;

        Vector2 dirToTarget = ((Vector2)currentTarget.position - (Vector2)transform.position).normalized;
        float angleToTarget = Vector2.SignedAngle(transform.up, dirToTarget);
        float turnAmount = Mathf.Clamp(angleToTarget / 45f, -1f, 1f);
        float forwardAmount = Vector2.Dot(transform.up, dirToTarget) > 0 ? 1f : -1f;

        float distance = Vector2.Distance(transform.position, currentTarget.position);
        if (distance < 0.5f)
        {
            forwardAmount = 0f;
            turnAmount = 0f;

            if (!hasPassenger)
            {
                // Lo "recoge"
                hasPassenger = true;
                if (passengerObject != null)
                {
                    passengerObject.SetActive(false); // Simula recogida
                }

                currentTarget = destinationObject.transform;
                Debug.Log("Pasajero recogido por IA");
            }
            else
            {
                // Lo "entrega"
                Debug.Log("IA entrego al pasajero.");
                currentTarget = null;
                hasPassenger = false;

            }
        }

        carController.SetInputVector(new Vector2(turnAmount, forwardAmount));
    }
}