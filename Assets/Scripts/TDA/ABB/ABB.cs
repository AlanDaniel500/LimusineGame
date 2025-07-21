using UnityEngine;

public class ABBVelocidad : IABB
{
    private class Nodo
    {
        public int puntos;
        public float mejora;
        public Nodo izquierda;
        public Nodo derecha;

        public Nodo(int puntos, float mejora)
        {
            this.puntos = puntos;
            this.mejora = mejora;
        }
    }

    private Nodo raiz;

    public void Insertar(int puntos, float mejora)
    {
        raiz = InsertarRecursivo(raiz, puntos, mejora);
    }

    private Nodo InsertarRecursivo(Nodo nodo, int puntos, float mejora)
    {
        if (nodo == null) return new Nodo(puntos, mejora);

        if (puntos < nodo.puntos)
            nodo.izquierda = InsertarRecursivo(nodo.izquierda, puntos, mejora);
        else if (puntos > nodo.puntos)
            nodo.derecha = InsertarRecursivo(nodo.derecha, puntos, mejora);
        else
            nodo.mejora = mejora;

        return nodo;
    }

    public float BuscarMejora(int puntos)
    {
        return BuscarMejoraRec(raiz, puntos, 1f);
    }

    private float BuscarMejoraRec(Nodo nodo, int puntos, float mejorHastaAhora)
    {
        if (nodo == null) return mejorHastaAhora;

        if (puntos == nodo.puntos) return nodo.mejora;

        if (puntos < nodo.puntos)
            return BuscarMejoraRec(nodo.izquierda, puntos, mejorHastaAhora);
        else
            return BuscarMejoraRec(nodo.derecha, puntos, nodo.mejora);
    }
}

