using System;
using System.Collections.Generic;
using UnityEngine;

public class LinkedListTest : MonoBehaviour
{
    private int failCount;

    private void Start()
    {
        Run(TestDoublyLinkedList);
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

    void TestDoublyLinkedList()
  {
      Debug.Log("=== DoublyLinkedList 테스트 시작 ===");

      var list = new DoublyLinkedList<string>();

      Debug.Log("\n1. 추가 (AddLast / AddFirst):");
      var b = list.AddLast("B");
      list.AddLast("D");
      list.AddFirst("A");
      Expect($"{string.Join(" <-> ", list)} (개수: {list.Count})", "A <-> B <-> D (개수: 3)", "정방향");

      Debug.Log("\n2. 노드 기준 삽입 (AddAfter / AddBefore):");
      list.AddAfter(b, "C");
      list.AddBefore(list.First, "시작");
      Expect(string.Join(" <-> ", list), "시작 <-> A <-> B <-> C <-> D", "정방향");
      Expect(string.Join(" <-> ", list.Backward()), "D <-> C <-> B <-> A <-> 시작", "역방향");

      Debug.Log("\n3. 노드로 O(1) 제거 (Remove(node)):");
      list.Remove(b);
      Expect(string.Join(" <-> ", list), "시작 <-> A <-> C <-> D", "B 노드 제거");

      Debug.Log("\n4. 값으로 제거 / 양 끝 제거:");
      Expect(list.Remove("D"), true, "Remove(\"D\")");
      list.RemoveFirst();
      list.RemoveLast();
      Expect($"{string.Join(" <-> ", list)} (개수: {list.Count})", "A (개수: 1)", "남은 내용");

      Debug.Log("\n5. 다른 리스트의 노드 검증:");
      var other = new DoublyLinkedList<string>();
      var foreign = other.AddLast("X");
      ExpectThrows<InvalidOperationException>(() => list.Remove(foreign), "예외 발생");

      Debug.Log("\n6. 이미 제거된 노드 재사용 검증:");
      ExpectThrows<InvalidOperationException>(() => list.AddAfter(b, "Z"), "예외 발생");

      Debug.Log("\n=== DoublyLinkedList 테스트 완료 ===");
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
