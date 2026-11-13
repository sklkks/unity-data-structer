using System;
using System.Collections;
using System.Collections.Generic;

public class CircularDoublyLinkedListNode<T>
{
    public T Value { get; set; }

    public CircularDoublyLinkedListNode<T> next;
    public CircularDoublyLinkedListNode<T> prev;

    public CircularDoublyLinkedListNode<T> Next => next == null || next == List.First ? null : next;

    public CircularDoublyLinkedListNode<T> Previous =>
        prev == null || prev == List.Last ? null : prev;
    public CircularDoublyLinkedList<T> List { get; set; }

    public CircularDoublyLinkedListNode(T value)
    {
        Value = value;
    }
}

public class CircularDoublyLinkedList<T> : IEnumerable<T>
{
    private CircularDoublyLinkedListNode<T> head;

    public int Count { get; private set; }

    public CircularDoublyLinkedListNode<T> First => head;
    public CircularDoublyLinkedListNode<T> Last => head?.prev;

    public IEnumerator<T> GetEnumerator()
    {
        CircularDoublyLinkedListNode<T> current = First;
        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    public IEnumerable<T> Backward()
    {
        CircularDoublyLinkedListNode<T> current = Last;
        while (current != null)
        {
            yield return current.Value;
            current = current.Previous;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public CircularDoublyLinkedListNode<T> AddFirst(T value)
    {
        var node = AddLast(value);
        head = node;
        return node;
    }

    public CircularDoublyLinkedListNode<T> AddLast(T value)
    {
        var node = new CircularDoublyLinkedListNode<T>(value);
        node.List = this;
        if (head == null)
        {
            node.next = node;
            node.prev = node;
            head = node;
        }
        else
        {
            InsertBefore(head, node);
        }
        Count++;
        return node;
    }

    public CircularDoublyLinkedListNode<T> AddAfter(CircularDoublyLinkedListNode<T> node, T value)
    {
        ValidataNode(node);

        var newNode = new CircularDoublyLinkedListNode<T>(value);
        newNode.List = this;
        InsertBefore(node.next, newNode);

        Count++;
        return newNode;
    }

    private void InsertBefore(
        CircularDoublyLinkedListNode<T> node,
        CircularDoublyLinkedListNode<T> newNode
    )
    {
        newNode.prev = node.prev;
        newNode.next = node;
        node.prev.next = newNode;
        node.prev = newNode;
    }

    public CircularDoublyLinkedListNode<T> AddBefore(CircularDoublyLinkedListNode<T> node, T value)
    {
        ValidataNode(node);

        var newNode = new CircularDoublyLinkedListNode<T>(value);
        newNode.List = this;
        InsertBefore(node, newNode);

        if (node == head)
        {
            head = newNode;
        }

        Count++;
        return newNode;
    }

    public void RemoveFirst()
    {
        if (head == null)
        {
            throw new InvalidOperationException("리스트가 비어있습니다.");
        }
        Remove(head);
    }

    public void RemoveLast()
    {
        if (head == null)
        {
            throw new InvalidOperationException("리스트가 비어있습니다.");
        }
        Remove(head.prev);
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

    public void Remove(CircularDoublyLinkedListNode<T> node)
    {
        ValidataNode(node);

        if (Count == 1)
        {
            head = null;
        }
        else
        {
            node.prev.next = node.next;
            node.next.prev = node.prev;

            if (node == head)
            {
                head = node.next;
            }
        }

        node.next = null;
        node.prev = null;
        node.List = null;

        Count--;
    }

    public CircularDoublyLinkedListNode<T> Find(T value)
    {
        var comparer = EqualityComparer<T>.Default;
        CircularDoublyLinkedListNode<T> current = head;
        while (current != null)
        {
            if (comparer.Equals(current.Value, value))
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
        for (int i = 0; i < Count; ++i)
        {
            var next = current.next;
            current.next = null;
            current.prev = null;
            current.List = null;
            current = next;
        }
        head = null;
        Count = 0;
    }

    private void ValidataNode(CircularDoublyLinkedListNode<T> node)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }
        if (node.List != this)
        {
            throw new InvalidOperationException("이 리스트에 속한 노드가 아닙니다.");
        }
    }
}