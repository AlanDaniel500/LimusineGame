using System;
using UnityEngine;

public class MyStack<T> : IStack<T>
{
    // Clase interna para los nodos de la pila
    private class Node
    {
        public T Data;
        public Node Next;
    }

    private Node top;   // Referencia al nodo de la cima
    private int count;  // Cantidad de elementos

    // Propiedades de la interfaz
    public int Count => count;
    public bool IsEmpty => count == 0;

    // Agrega un elemento a la pila
    public void Push(T item)
    {
        Node newNode = new Node { Data = item, Next = top };
        top = newNode;
        count++;
    }

    // Extrae y remueve el elemento en la cima
    public T Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException("The Stack is Empty");

        T item = top.Data;
        top = top.Next;
        count--;
        return item;
    }

    // Devuelve el elemento en la cima sin removerlo
    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("The Stack is Empty");

        return top.Data;
    }
}
