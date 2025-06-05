using System.Collections.Generic;
using UnityEngine;

public class Nodo : MonoBehaviour
{
    [Header("Vecinos conectados (manual o automático)")]
    public List<Nodo> vecinos = new List<Nodo>();

    public Vector2 Posicion => transform.position;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.15f);

        if (vecinos != null)
        {
            Gizmos.color = Color.cyan;
            foreach (Nodo vecino in vecinos)
            {
                if (vecino != null)
                    Gizmos.DrawLine(transform.position, vecino.transform.position);
            }
        }
    }
}
