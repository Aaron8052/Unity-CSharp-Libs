// @FengYan 2024 Copyright Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CsLibs.Collections
{
    /// <summary>
    /// 二叉树
    /// </summary>
    /// <typeparam name="T">实现了IComparable的任意类型</typeparam>
    public class BinaryTree<T>: IEnumerable<T> where T : IComparable
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
        
#region Enumerator
        // 升序迭代器
        public IEnumerator<T> GetEnumerator()
        {
            //return new BinaryTreeEnum(this);
            return ToList().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

#endregion

#region Public Functions
        public void InsertNode(T item)
        {
            var newNode = new TreeNode {value = item};
            Insert(ref root, ref newNode);
        }
        public void InsertNode(params T[] items)
        {
            for (var i = 0; i < items.Length; i++)
            {
                InsertNode(items[i]);
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
        
        public T[] ToArray()
        {
            return ToList().ToArray();
        }
        public List<T> ToList()
        {
            var list = new List<T>();
            InOrder(root);
            void InOrder(TreeNode nodePtr)
            {
                if (nodePtr)
                {
                    InOrder(nodePtr.left);
                    list.Add(nodePtr.value);
                    InOrder(nodePtr.right);
                }
            }

            return list;
        }
        public Stack<T> ToStack()
        {
            var stack = new Stack<T>();
            InOrder(root);
            void InOrder(TreeNode nodePtr)
            {
                if (nodePtr)
                {
                    InOrder(nodePtr.left);
                    stack.Push(nodePtr.value);
                    InOrder(nodePtr.right);
                }
            }

            return stack;
        }
        /// <summary>
        /// 升序输出二叉树
        /// </summary>
        public void DisplayInOrder() => DisplayInOrder(root);
        public void DisplayPreOrder() => DisplayPreOrder(root);
        public void DisplayPostOrder() => DisplayPostOrder(root);
#endregion

#region Private Functions
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


#endregion
        
        
        // 迭代器
        /*public class BinaryTreeEnum : IEnumerator<T>
        {
            private T[] items;
            private int index = -1;
            
            public BinaryTreeEnum(BinaryTree<T> tree)
            {
                items = tree.ToArray();
            }
            
            
            public bool MoveNext()
            {
                index++;
                return (index < items.Length);
            }

            public void Reset()
            {
                index = -1;
            }

            public T Current
            {
                get
                {
                    try
                    {
                        return items[index];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                items = null;
            }
        } */
    }

    
}
