using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public Nodo[] todosLosNodos;

    private void Awake()
    {
        todosLosNodos = FindObjectsOfType<Nodo>();
    }

    public Nodo GetNodoCercano(Vector2 posicion)
    {
        Nodo nodoMasCercano = null;
        float distanciaMin = Mathf.Infinity;

        foreach (Nodo nodo in todosLosNodos)
        {
            float distancia = Vector2.Distance(posicion, nodo.Posicion);
            if (distancia < distanciaMin)
            {
                distanciaMin = distancia;
                nodoMasCercano = nodo;
            }
        }

        return nodoMasCercano;
    }
}
