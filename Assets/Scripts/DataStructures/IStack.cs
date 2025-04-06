using UnityEngine;

public interface IStack<T>
{
    void Push(T item);   // Agrega un elemento a la pila
    T Pop();             // Extrae (y remueve) el elemento en la cima
    T Peek();            // Devuelve el elemento en la cima sin removerlo
    int Count { get; }   // Cantidad de elementos en la pila
    bool IsEmpty { get; } // Indica si la pila está vacía
}
