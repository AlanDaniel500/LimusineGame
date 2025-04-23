using UnityEngine;

public class EnemyCarAI : MonoBehaviour
{
    [SerializeField] private Transform targetPositionTransform;
    private TopDownCarController carController;
    private Vector2 targetPosition;

    private void Awake()
    {
        carController = GetComponent<TopDownCarController>();
    }

    private void Update()
    {
        if (targetPositionTransform == null) return;

        SetTargetPosition(targetPositionTransform.position);

        // Dirección hacia el objetivo
        Vector2 dirToMovePosition = (targetPosition - (Vector2)transform.position).normalized;

        // Calcula cuánta rotación necesita (usamos transform.up en 2D)
        float angleToTarget = Vector2.SignedAngle(transform.up, dirToMovePosition);

        // Convertimos a valor de giro normalizado (-1 a 1)
        float turnAmount = Mathf.Clamp(angleToTarget / 45f, -1f, 1f);

        // Decidimos si ir hacia adelante o en reversa
        float forwardAmount = Vector2.Dot(transform.up, dirToMovePosition) > 0 ? 1f : -1f;

        // Detener el auto si ya llegó
        float distance = Vector2.Distance(transform.position, targetPosition);
        if (distance < 0.5f)
        {
            forwardAmount = 0f;
            turnAmount = 0f;
        }

        // Enviar inputs al auto (x = giro, y = aceleración)
        carController.SetInputVector(new Vector2(turnAmount, forwardAmount));
    }

    public void SetTargetPosition(Vector2 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}



