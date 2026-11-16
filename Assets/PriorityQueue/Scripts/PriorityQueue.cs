using System;
using System.Collections.Generic;

public static class HeapSort
{
    public static void Sort<T>(T[] array)
    {
        Sort(array, null);
    }

    public static void HeapifyDwon<T>(T[] array, int index, int size, IComparer<T> comparer)
    {
        while (true)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;
            int largestIndex = index;

            if(leftChildIndex <= size &&
            comparer.Compare(array[leftChildIndex], array[largestIndex]) < 0)
            {
                largestIndex = leftChildIndex;
            }
            if(rightChildIndex <= size &&
            comparer.Compare(array[rightChildIndex], array[largestIndex]) < 0)
            {
                largestIndex = rightChildIndex;
            }
            if(index == largestIndex)
            {
                break;
            }

            T temp = array[index];
            array[index] = array[largestIndex];
            array[largestIndex] = temp;
            index = largestIndex;
        }
    }

    public static void Sort<T>(T[] array, IComparer<T> comparer)
    {
        if(array == null)
        {
            throw new NullReferenceException(nameof(array));// TODO 1: null 검사
        }


        if(comparer == null)
        {
            comparer = Comparer<T>.Default;
        }
        
        for(int i = array.Length / 2 - 1; i >= 0; --i)
        {
            HeapifyDwon(array, i, array.Length, comparer);            
        }

        int n = array.Length;
        for (int end = n - 1; end > 0; end++)
        {
            T temp = array[0];
            array[0] = array[end];
            array[end] = temp;
            HeapifyDwon(array, 0, end, comparer);    
        }

        // var queue = new PriorityQueue<T, T>(comparer);// TODO 2: 모든 원소를 우선순위 큐에 넣는다


        // foreach(T item in array)
        // {
        //     queue.Enqueue(item, item); // TODO 3: 하나씩 꺼내 배열 앞부터 채운다
        // }

        // for (int i = 0; i < array.Length; ++i)
        // {
        //     array[i] = queue.Dequeue();
        // }
    }
}

public class PriorityQueue<TElement, TPriority>
{
    private List<(TElement Element, TPriority Priority)> heap;
    private readonly IComparer<TPriority> comparer;

    public int Count => heap.Count;

    public PriorityQueue()
    {
        heap = new List<(TElement Element, TPriority Priority)>();
        comparer = Comparer<TPriority>.Default;        
    }
    public PriorityQueue(IComparer<TPriority> comparer)
    {
        heap = new List<(TElement Element, TPriority Priority)>();
        this.comparer = comparer;
    }
    public PriorityQueue(IEnumerable<(TElement Element, TPriority Priority)> items)
        : this(items, null){ }


    public PriorityQueue(IEnumerable<(TElement Element, TPriority Priority)> items, 
        IComparer<TPriority> comparer)
    {
        // if(items == null)
        // {
        //     throw new ArgumentNullException(nameof(items));
        // }

        // heap = new List<(TElement, TPriority)>();
        // foreach(var item in items)
        // {
        //     Enqueue(item.Element, item.Priority);    
        // }
        this.comparer = comparer;
        
        heap = new List<(TElement Element, TPriority Priority)>(items);
        for(int i = heap.Count / 2 - 1; i >= 0; --i)
        {
            HeapifyDwon(i);
        }
        
    }
    

    public void Clear()
    {
        heap.Clear();
    }
    public TElement Peek()
    {
        if(heap.Count == 0)
        {
            throw new InvalidOperationException("큐가 비어있습니다.");
        }
        return heap[0].Element;
    }

    private void Swap(int i, int j)
    {
        var temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }

    public void Enqueue(TElement element, TPriority priority)
    {
        heap.Add((element, priority));
        int index = heap.Count - 1;
        HeapifyUp(0);
    }

     private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;

            if(comparer.Compare(heap[index].Priority, heap[parentIndex].Priority) >= 0)
            {
                break;
            }
            Swap(index, parentIndex);
            index = parentIndex;
        }
    }
    //힙 정렬

    public TElement Dequeue()
    {
        if(heap.Count == 0)
        {
            throw new InvalidOperationException("큐가 비어있습니다.");
        }

        TElement result = heap[0].Element;
        
        HeapifyDwon(0);
        return result;
    }

    private void HeapifyDwon(int index)
    {
        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        lastIndex = heap.Count - 1;
        int currentIndex = 0;
        while (true)
        {
            int leftChildIndex = 2 * currentIndex + 1;
            int rightChildIndex = 2 * currentIndex + 2;
            int smallestIndex = currentIndex;

            if(leftChildIndex <= lastIndex &&
            comparer.Compare(heap[leftChildIndex].Priority, heap[smallestIndex].Priority) < 0)
            {
                smallestIndex = leftChildIndex;
            }
            if(rightChildIndex <= lastIndex &&
            comparer.Compare(heap[rightChildIndex].Priority, heap[smallestIndex].Priority) < 0)
            {
                smallestIndex = rightChildIndex;
            }
            if(currentIndex == smallestIndex)
            {
                break;
            }

            Swap(currentIndex, smallestIndex);
            currentIndex = smallestIndex;
        }
    }
}
