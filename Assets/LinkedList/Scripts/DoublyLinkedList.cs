 using System;
using System.Collections;
using System.Collections.Generic;

public class DoublyLinkedListNode<T>
{
    public T Value {get; set;}
    public DoublyLinkedListNode<T> Next {get; set;}
    public DoublyLinkedListNode<T> previous {get; set;}
    public DoublyLinkedList<T> List {get; set;}
    public DoublyLinkedListNode(T value)
    {
        Value = value;
    }
}
public class DoublyLinkedList<T> : IEnumerable<T>
{
    private DoublyLinkedListNode<T> head;
    private DoublyLinkedListNode<T> tail;
    public int Count { get; private set;}
    public DoublyLinkedListNode<T> First => head;
    public DoublyLinkedListNode<T> Last => tail;

    public IEnumerator<T> GetEnumerator()
    {
        DoublyLinkedListNode<T> current = head;
        while(current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public IEnumerable<T> Backward()
    {
        DoublyLinkedListNode<T> current = tail;
        while(current != null)
        {
            yield return current.Value;
            current = current.previous;
        }
    }

    public DoublyLinkedListNode<T> AddFirst(T value)
    {
        var node = new DoublyLinkedListNode<T>(value);
        node.List = this;

        if(head == null)
        {
            head = node;
            tail = node;
        }
        else
        {
            node.Next = head;
            head.previous = node;
            head = node;
        }

        Count++;
        return node;
    }
    public DoublyLinkedListNode<T> AddLast(T value)
    {
        var node = new DoublyLinkedListNode<T>(value);
        node.List = this;

        if(tail == null)
        {
            head = node;
            tail = node;
        }
        else
        {
            tail.Next = node;
            node.previous = tail;
            tail = node;
        }

        Count++;
        return node;
    }
    public DoublyLinkedListNode<T> AddAfter(DoublyLinkedListNode<T>node, T value)
    {
        ValidataNode(node);

        if(node == tail)
        {
            return AddLast(value);
        }

        var newNode = new DoublyLinkedListNode<T>(value);
        newNode.List = this;

        newNode.Next = node.Next;
        newNode.previous = node;
        node.Next.previous = newNode;
        node.Next = newNode;

        Count++;
        return newNode;
    }

       public DoublyLinkedListNode<T> AddBefore(DoublyLinkedListNode<T>node, T value)
    {
        ValidataNode(node);

        if(node == head)
        {
            return AddFirst(value);
        }

        var previous = head;
        while (previous.Next != node)
        {
            previous = previous.Next;
        }

        var newNode = new DoublyLinkedListNode<T>(value);
        newNode.List = this;

        newNode.Next = node;
        newNode.previous = node.previous;        
        node.previous.Next = newNode;
        node.previous = newNode;

        Count++;
        return newNode;
    }
    public void RemoveFirst()
    {
        if(head == null)
        {
            throw new InvalidOperationException("리스트가 비어있습니다.");
        }
        Remove(head);
    }

    public void RemoveLast()
    {
        if(tail == null)
        {
            throw new InvalidOperationException("리스트가 비어있습니다.");
        }
        Remove(tail);
    }
    public bool Remove(T value)
    {
        var node = Find(value);
        if (node == null)
        {
            return false;
        }
        Remove(node);
        return true;
    }
    public void Remove(DoublyLinkedListNode<T> node)
    {
        ValidataNode(node);
        
        DoublyLinkedListNode<T> previous = null;
        DoublyLinkedListNode<T> current = head;

        if(node == head)
        {
            head = node.Next;

        }
        else
        {
            node.previous.Next = node.Next;
        }

        if(node == tail)
        {
            tail = node.previous;
        }
        else
        {
            node.Next.previous = node.previous;
        }

        node.Next = null;
        node.previous = null;
        node.List = null;

        Count--;
    }

    public DoublyLinkedListNode<T> Find(T value)
    {
        var comparer = EqualityComparer<T>.Default;

        DoublyLinkedListNode<T> current = head;
        while(current != null)
        {
            if(comparer.Equals(current.Value, value))
            {
                return current;
            }

            current = current.Next;
        }
        return null;
    }
    public bool Contains(T value)
    {
        return Find(value) != null;
    }
    public void Clear()
    {

        var current = head;
        while (current != null)
        {
            var next = current.Next;
            current.Next = null;
            current.previous = null;
            current.List = null;
            current = current.Next;
        }

        head = tail = null;
        Count = 0;
    }

    private void ValidataNode(DoublyLinkedListNode<T> node)
    {
        if(node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }
        if(node.List != this)
        {
            throw new InvalidOperationException("이 리스트에 속한 노드가 아닙니다.");
        }
    }
}