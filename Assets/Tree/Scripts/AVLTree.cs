using System;
using Unity.VisualScripting;
using UnityEngine;

public class AVLTreeTest<TKey, TValue> : AVLTree<TKey, TValue> where TKey : IComparable<TKey>
{
    public TreeNode<TKey, TValue> GetRoot()
    {
        return root;
    }
}

public class AVLTree<TKey, TValue> : BinarySearchTree<TKey, TValue> where TKey : IComparable<TKey>
{
    
    protected override TreeNode<TKey, TValue> Add(
        TreeNode<TKey, TValue> node,
        TKey key,
        TValue value
    )
    {
        node = base.Add(node, key, value);
        return Balance(node);
    }
    protected override TreeNode<TKey, TValue> AddOrUpdate(
        TreeNode<TKey, TValue> node,
        TKey key,
        TValue value
    )
    {
        node = base.AddOrUpdate(node, key, value);
        return Balance(node);
    }
    protected override TreeNode<TKey, TValue> Remove(
        TreeNode<TKey, TValue> node,
        TKey key        
    )
    {
        node = base.Remove(node, key);
        if(node == null) return node;
        return Balance(node);
    }
    protected int BalanceFactor(TreeNode<TKey, TValue> node)
    {
        return node == null ? 0 : Height(node.Left) - Height(node.Right);
    }

    protected TreeNode<TKey, TValue> Balance(TreeNode<TKey, TValue> node)
    {
        int balanceFactor = BalanceFactor(node);
        // TODD: LL, LR, RR, RL
        if (balanceFactor > 1)
        {
            if (BalanceFactor(node.Left) < 0)
            {
                node.Left = RotateLeft(node.Left);
            }
            node = RotateRight(node);
        }
        else if (balanceFactor < -1)
        {
            if (BalanceFactor(node.Right) > 0)
            {
                node.Right = RotateRight(node.Right);
            }
            node = RotateLeft(node);
        }
        return node;
    }
    protected TreeNode<TKey, TValue> RotateLeft(TreeNode<TKey, TValue> node)
    {
        var rightChild = node.Right;
        var leftSubTreeOfRightChild = rightChild.Left;

        rightChild.Left = node;
        node.Right = leftSubTreeOfRightChild;
        
        UpdateHeight(node);
        UpdateHeight(rightChild);
        return rightChild;
    }
    protected TreeNode<TKey, TValue> RotateRight(TreeNode<TKey, TValue> node)
    {
        var leftChild = node.Left;
        var rightSubTreeOfRightChild = leftChild.Left;

        leftChild.Left = node;
        node.Left = rightSubTreeOfRightChild;
        
        UpdateHeight(node);
        UpdateHeight(leftChild);
        return leftChild;
    }

        // 모든 노드의 BF ∈ {-1, 0, +1}인지 검증 (AVL 불변식).
    public bool IsBalanced()
    {
        return IsBalanced(root);
    }

    private bool IsBalanced(TreeNode<TKey, TValue> node)
    {
        if (node == null)
        {
            return true;
        }
        if (Mathf.Abs(BalanceFactor(node)) > 1)
        {
            return false;
        }
        return IsBalanced(node.Left) && IsBalanced(node.Right);
    }

}