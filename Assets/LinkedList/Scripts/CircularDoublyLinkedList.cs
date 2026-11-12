using System;
using System.Collections;
using System.Collections.Generic;

public class CircularDoublyLinkedListNode<T>
{
    public T Value { get; set; }
    public CircularDoublyLinkedList<T> List { get; set; }

    // 리스트 내부 전용 링크 = 마지막 노드의 next가 처음 노드인 원형 연결
    public CircularDoublyLinkedListNode<T> next;
    public CircularDoublyLinkedListNode<T> prev;

    // 사용자가 보는 링크 = 이음새(마지막 → 처음)에서 null을 돌려줘 원형을 숨긴다
    public CircularDoublyLinkedListNode<T> Next => next == null || next == List.First ? null : next;
    public CircularDoublyLinkedListNode<T> Previous => prev == null || this == List.First ? null : prev;

    public CircularDoublyLinkedListNode(T value)
    {
        Value = value;
    }
}
public class CircularDoublyLinkedList<T> : IEnumerable<T>
{
    private CircularDoublyLinkedListNode<T> head;
    private CircularDoublyLinkedListNode<T> tail;
    public int Count { get; private set;}
    public CircularDoublyLinkedListNode<T> First => head;
    public CircularDoublyLinkedListNode<T> Last => tail;

    public IEnumerator<T> GetEnumerator()
    {
        CircularDoublyLinkedListNode<T> current = head;
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
        CircularDoublyLinkedListNode<T> current = tail;
        while(current != null)
        {
            yield return current.Value;
            current = current.previous;
        }
    }

    public CircularDoublyLinkedListNode<T> AddFirst(T value)
    {
        var node = new CircularDoublyLinkedListNode<T>(value);
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
    public CircularDoublyLinkedListNode<T> AddLast(T value)
    {
        var node = new CircularDoublyLinkedListNode<T>(value);
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
    public CircularDoublyLinkedListNode<T> AddAfter(CircularDoublyLinkedListNode<T>node, T value)
    {
        ValidataNode(node);

        if(node == tail)
        {
            return AddLast(value);
        }

        var newNode = new CircularDoublyLinkedListNode<T>(value);
        newNode.List = this;

        newNode.Next = node.Next;
        newNode.previous = node;
        node.Next.previous = newNode;
        node.Next = newNode;

        Count++;
        return newNode;
    }

       public CircularDoublyLinkedListNode<T> AddBefore(CircularDoublyLinkedListNode<T>node, T value)
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

        var newNode = new CircularDoublyLinkedListNode<T>(value);
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
    public void Remove(CircularDoublyLinkedListNode<T> node)
    {
        ValidataNode(node);
        
        CircularDoublyLinkedListNode<T> previous = null;
        CircularDoublyLinkedListNode<T> current = head;

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

    public CircularDoublyLinkedListNode<T> Find(T value)
    {
        var comparer = EqualityComparer<T>.Default;

        CircularDoublyLinkedListNode<T> current = head;
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

    private void ValidataNode(CircularDoublyLinkedListNode<T> node)
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
