using UnityEngine;

public interface IQueue<T>
{
    void Enqueue(T item); // Agrega un elemento al final de la cola
    T Dequeue();          // Extrae (y remueve) el elemento del frente de la cola
    T Peek();             // Devuelve el elemento del frente sin removerlo
    int Count { get; }    // Cantidad de elementos en la cola
    bool IsEmpty { get; } // Indica si la cola está vacía
}
