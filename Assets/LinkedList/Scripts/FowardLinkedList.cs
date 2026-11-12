using System;
using System.Collections;
using System.Collections.Generic;

public class ForwardLinkedListNode<T>
{
    public T Value {get; set;}
    public ForwardLinkedListNode<T> Next {get; set;}
    public ForwardLinkedList<T> List {get; set;}

    public ForwardLinkedListNode(T value)
    {
        Value = value;
    }
}
public class ForwardLinkedList<T> : IEnumerable<T>
{
    private ForwardLinkedListNode<T> head;
    private ForwardLinkedListNode<T> tail;
    public int Count { get; private set;}
    public ForwardLinkedListNode<T> First => head;
    
    public ForwardLinkedListNode<T> Last => tail;

    public IEnumerator<T> GetEnumerator()
    {
        ForwardLinkedListNode<T> current = head;
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

    public ForwardLinkedListNode<T> AddFirst(T value)
    {
        var node = new ForwardLinkedListNode<T>(value);
        node.List = this;

        node.Next = head;
        head = node;

        if(tail == null)
        {
            tail = node;
        }
        Count++;
        return node;
    }
    public ForwardLinkedListNode<T> AddLast(T value)
    {
        var node = new ForwardLinkedListNode<T>(value);
        node.List = this;

        if(tail == null)
        {
            head = node;
            tail = node;
        }
        else
        {
            tail.Next = node;
            tail = node;
        }

        Count++;
        return node;
    }
    public ForwardLinkedListNode<T> AddAfter(ForwardLinkedListNode<T>node, T value)
    {
        ValidataNode(node);

        if(node == tail)
        {
            return AddLast(value);
        }

        var newNode = new ForwardLinkedListNode<T>(value);
        newNode.List = this;

        newNode.Next = node.Next;
        node.Next = newNode;

        Count++;
        return newNode;
    }

       public ForwardLinkedListNode<T> AddBefore(ForwardLinkedListNode<T>node, T value)
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

        var newNode = new ForwardLinkedListNode<T>(value);
        newNode.List = this;

        newNode.Next = node;
        previous.Next = newNode;

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
    public void Remove(ForwardLinkedListNode<T> node)
    {
        ValidataNode(node);
        
        ForwardLinkedListNode<T> previous = null;
        ForwardLinkedListNode<T> current = head;

        while (current != node)
        {
            previous = current;
            current = current.Next;
        }

        if(previous == null)
        {
            head = node.Next;
        }
        else
        {
            previous.Next = node.Next;
        }

        if(node == tail)
        {
            tail = previous;            
        }

        node.Next = null;
        node.List = null;

        Count--;
    }

    public ForwardLinkedListNode<T> Find(T value)
    {
        var comparer = EqualityComparer<T>.Default;

        ForwardLinkedListNode<T> current = head;
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
            current.List = null;
            current = current.Next;
        }

        head = tail = null;
        Count = 0;
    }

    private void ValidataNode(ForwardLinkedListNode<T> node)
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