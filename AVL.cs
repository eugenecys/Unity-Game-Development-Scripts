using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AVL<T> : ICollection<T> where T : IComparable<T>
{
    private Node<T> _root;

    public AVL()
    {

    }

    public AVL(ICollection<T> collection)
    {
        AddAll(collection);
    }

    public static AVL<T> CopyOf(ICollection<T> collection)
    {
        return new AVL<T>(collection);
    }

    public T this[int i]
    {
        get
        {
            return _getIndexFromLeft(_root, i).value;
        }
        private set
        {

        }
    }

    public int Count
    {
        get
        {
            if (_root == null)
            {
                return 0;
            }
            else
            {
                return _root.weight;
            }
        }
        private set
        {

        }
    }

    public void Clear()
    {
        _root = null;
    }

    public bool Contains(T item)
    {
        return Find(item) != -1;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (Count - arrayIndex > 0)
        {
            array = new T[Count - arrayIndex];
            for (int i = arrayIndex; i < Count; i++)
            {
                array[i] = this[i];
            }
        }
    }

    public bool IsReadOnly
    {
        get { throw new NotImplementedException(); }
    }

    public bool Remove(T item)
    {
        int idx = Find(item);
        if (idx >= 0)
        {
            Pop(idx);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public T Pop(int idx)
    {
        Node<T> node = _getIndexFromLeft(_root, idx);
        _remove(node);
        return node.value;
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public void Add(T arg)
    {
        Node<T> newNode = new Node<T>(arg);
        if (_root == null)
        {
            _root = newNode;
            newNode.parent = null;
            newNode.height = 0;
            newNode.balance = 0;
        }
        else
        {
            _insertNode(_root, newNode);
        }
    }

    public void AddAll(ICollection<T> collection)
    {
        foreach (T obj in collection)
        {
            Add(obj);
        }
    }

    private void _insertNode(Node<T> current, Node<T> newNode)
    {
        if (current.CompareTo(newNode) > 0)
        {
            //Go Left
            if (current.left == null)
            {
                current.left = newNode;
                newNode.parent = current;
                _updateParents(current);
                _balanceTree(current);
            }
            else
            {
                _insertNode(current.left, newNode);
            }
        }
        else if (current.CompareTo(newNode) < 0)
        {
            // Go Right
            if (current.right == null)
            {
                current.right = newNode;
                newNode.parent = current;
                _updateParents(current);
                _balanceTree(current);
            }
            else
            {
                _insertNode(current.right, newNode);
            }
        }
    }

    public void Push(T arg)
    {
        Add(arg);
    }

    public int Find(T arg)
    {
        int result = _getIndex(_root, arg, 0);
        return result;
    }

    public T Pop()
    {
        Node<T> node = _getIndexFromLeft(_root, 0);
        _remove(node);
        return node.value;
    }

    private void _remove(Node<T> node) 
    {
        Node<T> current = node;
        Node<T> parent = node.parent;
        Node<T> right = node.right;
        Node<T> left = node.left;


        if (left == null)
        {
            if (parent == null)
            {
                // root
                _root = right;
                if (right != null)
                {
                    right.parent = null;
                    _balanceTree(right);
                }
            }
            else 
            {
                if (parent.left != null && parent.left.Equals(node))
                {
                    parent.left = right;
                }
                else if (parent.right != null && parent.right.Equals(node))
                {
                    parent.right = right;
                }

                if (right != null)
                {
                    right.parent = parent;
                }
                node = null;
                _updateParents(parent);
                _balanceTree(parent);
            }
        }
        else if (right == null)
        {
            if (parent == null)
            {
                // root
                _root = left;
                if (left != null)
                {
                    left.parent = null;
                    _balanceTree(left);
                }
            }
            else
            {
                if (parent.left != null && parent.left.Equals(node))
                {
                    parent.left = left;
                }
                else if (parent.right != null && parent.right.Equals(node))
                {
                    parent.right = left;
                }

                if (left != null)
                {
                    left.parent = parent;
                }
                node = null;
                _updateParents(parent);
                _balanceTree(parent);
            }
        }
        else
        {
            // Both sides are not null
            Node<T> successor = _getNext(node);
            node.value = successor.value;
            _remove(successor);
        }
    }
    
    
    private Node<T> _getNext(Node<T> node)
    {
        if (node.right == null)
        {
            return null;
        }
        else
        {
            node = node.right;
            while (node.left != null)
            {
                node = node.left;
            }
            return node;
        }
    }

    private Node<T> _getPrevious(Node<T> node)
    {
        if (node.left == null)
        {
            return null;
        }
        else
        {
            node = node.left;
            while (node.right != null)
            {
                node = node.right;
            }
            return node;
        }
    }

    private int _getIndex(Node<T> current, T val, int index)
    {
        if (current.value.CompareTo(val) > 0)
        {
            if (current.left == null)
            {
                return -1;
            }
            else
            {
                return _getIndex(current.left, val, index);
            }
        }
        else if (current.value.CompareTo(val) < 0)
        {
            if (current.right == null)
            {
                return -1;
            }
            else
            {
                int leftWeight = 0;
                if (current.left != null)
                {
                    leftWeight = current.left.weight;
                }
                return _getIndex(current.right, val, index + leftWeight + 1);
            }
        }
        else
        {
            return index + ((current.left == null) ? 0 : current.left.weight);
        }
    }
    
    private Node<T> _getIndexFromLeft(Node<T> node, int idx)
    {
        if (idx < 0)
        {
            return null;
        }

        if (node.left == null)
        {
            if (idx == 0)
            {
                return node;
            }
            else
            {
                return _getIndexFromLeft(node.right, idx - 1);
            }
        }
        else
        {
            if (node.left.weight > idx)
            {
                return _getIndexFromLeft(node.left, idx);
            }
            else
            {
                int balance = idx - node.left.weight;
                if (balance == 0)
                {
                    return node;
                } 
                else 
                {
                    return _getIndexFromLeft(node.right, balance - 1);
                }
            }
        }
    }

    private void _balanceTree(Node<T> node)
    {
        if (node.balance > 1)
        {
            // Left is too tall
            _rotateTreeRight(node);
        }
        else if (node.balance < -1)
        {
            // Right is too tall
            _rotateTreeLeft(node);
        }

        if (node.parent != null)
        {
            _balanceTree(node.parent);
        }
    }

    private void _rotateTreeRight(Node<T> node)
    {
        //Check if left node is right heavy first
        //Left must not be empty
        
        Node<T> checkNode = node.left;
        if (checkNode.balance < 0)
        {
            _rotateLeft(checkNode);
        }
        _rotateRight(node);
    }

    private void _rotateTreeLeft(Node<T> node)
    {
        //check if right node is left heavy first
        //Right must not be empty

        Node<T> checkNode = node.right;

        if (checkNode.balance > 0)
        {
            _rotateRight(checkNode);
        }
        _rotateLeft(node);
    }

    private void _rotateLeft(Node<T> node)
    {
        if (node.right != null)
        {
            Node<T> newNode = node.right;
            Node<T> parent = node.parent;

            if (parent != null)
            {

                //Parent-node relations
                if (parent.left != null && parent.left == node)
                {
                    parent.left = newNode;
                }
                else if (parent.right != null && parent.right == node)
                {
                    parent.right = newNode;
                }
                newNode.parent = parent;
            }
            else
            {
                newNode.parent = null;
                _root = newNode;
            }

            //shift-node relations
            if (newNode.left != null)
            {
                newNode.left.parent = node;
                node.right = newNode.left;
            }
            else
            {
                node.right = null;
            }

            //node-old node relations
            node.parent = newNode;
            newNode.left = node;

            //update weights and heights
            _updateParents(node);

        } 
    }

    private void _rotateRight(Node<T> node)
    {
        if (node.left != null)
        {
            Node<T> newNode = node.left;
            Node<T> parent = node.parent;

            if (parent != null)
            {

                //Parent-node relations
                if (parent.left != null && parent.left == node)
                {
                    parent.left = newNode;
                }
                else if (parent.right != null && parent.right == node)
                {
                    parent.right = newNode;
                }
                newNode.parent = parent;
            }
            else
            {
                newNode.parent = null;
                _root = newNode;
            }
            //shift-node relations
            if (newNode.right != null)
            {
                newNode.right.parent = node;
                node.left = newNode.right;
            }
            else
            {
                node.left = null;
            }

            //node-old node relations
            node.parent = newNode;
            newNode.right = node;

            //update weights and heights
            _updateParents(node);

        }
    }

    private void _updateParents(Node<T> node)
    {
        int leftHeight = -1;
        int rightHeight = -1;
        int leftWeight = 0;
        int rightWeight = 0;
        if (node.left != null)
        {
            leftHeight = node.left.height;
            leftWeight = node.left.weight;
        }
        if (node.right != null)
        {
            rightHeight = node.right.height;
            rightWeight = node.right.weight;
        }

        node.weight = rightWeight + leftWeight + 1;
        node.height = Mathf.Max(leftHeight, rightHeight) + 1;
        node.balance = leftHeight - rightHeight;

        if (node.parent != null)
        {
            _updateParents(node.parent);
        }
    }

    private class Node<T> : IComparable<Node<T>> where T : IComparable<T> 
    {
        public Node<T> parent;
        public Node<T> left;
        public Node<T> right;
        public int height;
        public int balance;
        public T value;
        public int weight;

        public Node()
        {

        }

        public Node(T arg)
        {
            value = arg;
            height = 0;
            balance = 0;
            weight = 1;
        }

        public int CompareTo(Node<T> other)
        {
            return value.CompareTo(other.value);
        }

        public override bool Equals(object obj)
        {
            Node<T> node = obj as Node<T>;
            if (object.ReferenceEquals(node, null))
            {
                return false;
            }
            return this.CompareTo(node) == 0;
        }

        public bool Equals(Node<T> other)
        {
            return value.CompareTo(other.value) == 0;
        }

    }
}
