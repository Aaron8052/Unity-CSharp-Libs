using System;
using UnityEngine;

namespace CsLibs.Collection
{
    /// <summary>
    /// 二叉树
    /// </summary>
    /// <typeparam name="T">实现了IComparable的任意类型</typeparam>
    public class BinaryTree<T> where T : IComparable
    {
        public class TreeNode
        {
            public T value;
            public TreeNode left;
            public TreeNode right;

            public static bool operator <(TreeNode a, TreeNode b) => a.value.CompareTo(b.value) < 0;
            public static bool operator <(T a, TreeNode b) => a.CompareTo(b.value) < 0;
            public static bool operator >(TreeNode a, TreeNode b) => a.value.CompareTo(b.value) > 0;
            public static bool operator >(T a, TreeNode b) => a.CompareTo(b.value) > 0;
            public static bool operator ==(T a, TreeNode b) => b != null && a != null && a.CompareTo(b.value) == 0;
            public static bool operator ==(TreeNode a, T b) => b == a;
            public static bool operator !=(T a, TreeNode b) => a < b || a > b;
            public static bool operator !=(TreeNode a, T b) => b != a;
            public static implicit operator bool(TreeNode a) => a != null;
        }
        
        private TreeNode root;

        public BinaryTree(params T[] items)
        {
            InsertNode(items);
        }
        ~BinaryTree()
        {
            DestroySubTree(root);
        }
        
        public void InsertNode(params T[] items)
        {
            for (var i = 0; i < items.Length; i++)
            {
                var newNode = new TreeNode {value = items[i]};
                Insert(ref root, ref newNode);
            }
        }
        
        public bool SearchNode(T item)
        {
            var nodePtr = root;
            while (nodePtr)
            {
                if (nodePtr == item)
                    return true;
                else if (item < nodePtr)
                    nodePtr = nodePtr.left;
                else
                    nodePtr = nodePtr.right;
            }

            return false;
        }
        
        public void Remove(T item) => DeleteNode(item, ref root);
        
        /// <summary>
        /// 升序输出二叉树
        /// </summary>
        public void DisplayInOrder() => DisplayInOrder(root);
        public void DisplayPreOrder() => DisplayPreOrder(root);
        public void DisplayPostOrder() => DisplayPostOrder(root);
        
        private void Insert(ref TreeNode nodePtr, ref TreeNode newNode)
        {
            if (nodePtr == null)
                nodePtr = newNode;
            else if(newNode < nodePtr)
                Insert(ref nodePtr.left, ref newNode);
            else
                Insert(ref nodePtr.right, ref newNode);
        }
        
        private void DestroySubTree(TreeNode nodePtr)
        {
            if (nodePtr)
            {
                if(nodePtr.left)
                    DestroySubTree(nodePtr.left);
                if(nodePtr.right)
                    DestroySubTree(nodePtr.right);
            }
            
        }

        private void DeleteNode(T item, ref TreeNode nodePtr)
        {
            if (item < nodePtr)
                DeleteNode(item, ref nodePtr.left);
            else if (item > nodePtr)
                DeleteNode(item, ref nodePtr.right);
            else
                MakeDeletion(ref nodePtr);
        }

        private void MakeDeletion(ref TreeNode nodePtr)
        {

            if (nodePtr == null)
                Debug.Log("Cannot delete empty node.");
            else if (!nodePtr.right)
            {
                // 如果右边是空的，直接将nodePtr替换为左边的节点
                nodePtr = nodePtr.left;
            }
            else if (!nodePtr.left)
            {
                // 如果左边是空的，直接将nodePtr替换为右边的节点
                nodePtr = nodePtr.right;
            }
            else
            {
                // 目标：需要把所有左边的节点移动到右边的节点的左边的末尾，然后用原先右边的节点代替nodePtr
                var tempNodePtr = nodePtr.right;
                // 遍历至nodePtr右边Node的左边的最后一个节点
                while (tempNodePtr.left)
                    tempNodePtr = tempNodePtr.left;
                // 将nodePtr左边的所有节点移动到Node的右边的左边的最后一个节点
                tempNodePtr.left = nodePtr.left;
                // 将nodePtr替换为原nodePtr右边的节点
                nodePtr = nodePtr.right;
            }
        }


        private void DisplayInOrder(TreeNode nodePtr)
        {
            if (nodePtr)
            {
                DisplayInOrder(nodePtr.left);
                OutputNode(nodePtr);
                DisplayInOrder(nodePtr.right);
            }
        }
        

        private void DisplayPreOrder(TreeNode nodePtr)
        {
            if (nodePtr)
            {
                OutputNode(nodePtr);
                DisplayPreOrder(nodePtr.left);
                DisplayPreOrder(nodePtr.right);
            }
        }
        

        private void DisplayPostOrder(TreeNode nodePtr)
        {
            if (nodePtr)
            {
                DisplayPostOrder(nodePtr.left);
                DisplayPostOrder(nodePtr.right);
                OutputNode(nodePtr);
            }
        }
        
        private void OutputNode(TreeNode nodePtr)
        {
            Debug.Log($"{nodePtr.value.ToString()}");
        }
    }
}
