using System;
using UnityEngine;

public class MyStack<T> : IStack<T>
{
   
    private class Node
    {
        public T Data;
        public Node Next;
    }

    private Node top;  
    private int count;  

 
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
