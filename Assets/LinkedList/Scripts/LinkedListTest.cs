using System;
using System.Collections.Generic;
using UnityEngine;

public class LinkedListTest : MonoBehaviour
{
    private int failCount;

    private void Start()
    {
        Run(TestCircularDoublyLinkedList);
    }

    void Run(Action test)
    {
        try
        {
            test();
        }
        catch (Exception e)
        {
            failCount++;
            Debug.LogError($"[실패] {test.Method.Name} 중단 - 예상하지 못한 예외: {e}");
        }
    }

        void TestCircularDoublyLinkedList()
    {
        Debug.Log("=== CircularDoublyLinkedList 테스트 시작 ===");

        var list = new CircularDoublyLinkedList<string>();

        Debug.Log("\n1. 추가 (AddLast / AddFirst):");
        var b = list.AddLast("B");
        list.AddLast("D");
        list.AddFirst("A");
        Expect($"{string.Join(" <-> ", list)} (개수: {list.Count})", "A <-> B <-> D (개수: 3)", "정방향");

        Debug.Log("\n2. 안쪽은 원형, 바깥은 null:");
        Expect(list.Last.next == list.First && list.First.prev == list.Last, true, "내부 링크 원형");
        Expect(list.Last.Next == null && list.First.Previous == null, true, "이음새의 Next/Previous는 null");

        Debug.Log("\n3. 노드 기준 삽입 (AddAfter / AddBefore):");
        list.AddAfter(b, "C");
        list.AddBefore(list.First, "시작");
        list.AddAfter(list.Last, "끝");
        Expect(string.Join(" <-> ", list), "시작 <-> A <-> B <-> C <-> D <-> 끝", "정방향");
        Expect(string.Join(" <-> ", list.Backward()), "끝 <-> D <-> C <-> B <-> A <-> 시작", "역방향");

        Debug.Log("\n4. 사용자 순회 (Next / Previous가 null에서 멈춤):");
        var forward = new List<string>();
        for (var node = list.First; node != null; node = node.Next)
        {
            forward.Add(node.Value);
        }
        var backward = new List<string>();
        for (var node = list.Last; node != null; node = node.Previous)
        {
            backward.Add(node.Value);
        }
        Expect($"{string.Join(" -> ", forward)} / {string.Join(" -> ", backward)}", "시작 -> A -> B -> C -> D -> 끝 / 끝 -> D -> C -> B -> A -> 시작", "Next / Previous");

        Debug.Log("\n5. 제거 (Remove(node) / Remove(T) / 양 끝):");
        list.Remove(b);
        Expect(list.Remove("D"), true, "Remove(\"D\")");
        list.RemoveFirst();
        list.RemoveLast();
        Expect($"{string.Join(" <-> ", list)} (개수: {list.Count})", "A <-> C (개수: 2)", "남은 내용");
        Expect(list.Last.next == list.First && list.Last.Next == null, true, "제거 후에도 안쪽 원형, 바깥 null");

        Debug.Log("\n6. 노드 하나 / 빈 리스트:");
        list.RemoveFirst();
        Expect(list.First.next == list.First && list.First.Next == null, true, "하나 남은 노드: next는 자기 자신, Next는 null");
        list.RemoveLast();
        Expect($"[{string.Join(" <-> ", list)}] (개수: {list.Count}), First: {list.First == null}, Last: {list.Last == null}", "[] (개수: 0), First: True, Last: True", "빈 리스트");
        ExpectThrows<InvalidOperationException>(() => list.RemoveFirst(), "빈 리스트 예외");

        Debug.Log("\n7. 다른 리스트·제거된 노드 검증:");
        var other = new CircularDoublyLinkedList<string>();
        var foreign = other.AddLast("X");
        list.AddLast("Y");
        ExpectThrows<InvalidOperationException>(() => list.Remove(foreign), "다른 리스트 노드");
        ExpectThrows<InvalidOperationException>(() => list.AddAfter(b, "Z"), "제거된 노드");

        Debug.Log("\n=== CircularDoublyLinkedList 테스트 완료 ===");
    }
    void Expect<T>(T actual, T expected, string label)
    {
        if (EqualityComparer<T>.Default.Equals(actual, expected))
        {
            Debug.Log($"[통과] {label}: {actual}");
        }
        else
        {
            failCount++;
            Debug.LogError($"[실패] {label}: {actual} (기대값: {expected})");
        }
    }

    void ExpectThrows<TException>(Action action, string label)
        where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException e)
        {
            Debug.Log($"[통과] {label}: {e.Message}");
            return;
        }

        failCount++;
        Debug.LogError($"[실패] {label}: {typeof(TException).Name}이 발생하지 않음");
    }
}
