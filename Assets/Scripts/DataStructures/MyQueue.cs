using System;
using UnityEngine;

public class MyQueue<T> : IQueue<T>
{
    
    private class Node
    {
        public T Data;
        public Node Next;
    }

    private Node head; 
    private Node tail;  
    private int count;  


    public int Count => count;
    public bool IsEmpty => count == 0;

    // Agrega un elemento al final de la cola
    public void Enqueue(T item)
    {
        Node newNode = new Node { Data = item, Next = null };

        if (tail != null)
            tail.Next = newNode;
        tail = newNode;

        if (head == null)
            head = newNode;

        count++;
    }

    // Extrae y remueve el elemento del frente de la cola
    public T Dequeue()
    {
        if (IsEmpty)
            throw new InvalidOperationException("The Queue is Empty");

        T item = head.Data;
        head = head.Next;

        if (head == null)
            tail = null;

        count--;
        return item;
    }

    // Devuelve el elemento del frente sin removerlo
    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("The Queue is Empty");

        return head.Data;
    }
}

