using System;
using System.Collections;

namespace Algorithm.DataStructures
{
    // Define tree nodes
    public class TreeNode<TKey, TValue> where TKey : IComparable
    {
        public TKey key;
        public TValue value;
        public TreeNode<TKey, TValue> left, right;

        public TreeNode(TKey k, TValue v)
        {
            key = k;
            value = v;
            left = null;
            right = null;
        }
    }

    // The Binary tree itself
    public class BinarySearchTree<TKey, TValue> where TKey : IComparable
    {
        private TreeNode<TKey, TValue> root;     // Points to the root of the tree
        private int count = 0;

        public BinarySearchTree()
        {
            root = null;
            count = 0;
        }

        public void Clear()
        {
            killTree(ref root);
            count = 0;
        }

        public int Count()
        {
            return count;
        }

        public string DrawTree()
        {
            return drawNode(root);
        }

        public TreeNode<TKey, TValue> InsertRecursively(TKey k, TValue v)
        {
            TreeNode<TKey, TValue> node = new TreeNode<TKey, TValue>(k, v);
            try
            {
                addRecursively(node, ref root);
                count++;
                return node;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TreeNode<TKey, TValue> Insert(TKey k, TValue v)
        {
            TreeNode<TKey, TValue> node = new TreeNode<TKey, TValue>(k, v);
            TreeNode<TKey, TValue> parent = null;
            TreeNode<TKey, TValue> currentNode = root;
            while (currentNode != null)
            {
                parent = currentNode;
                int comp = node.key.CompareTo(currentNode.key);
                if (comp == 0) return null;

                if (comp < 0)
                {
                    currentNode = currentNode.left;
                }
                else
                {
                    currentNode = currentNode.right;
                }
            }

            //node.parent = parent;
            if (parent == null)
            {
                root = node;
            }
            else
            {
                if (node.key.CompareTo(parent.key) < 0)
                {
                    parent.left = node;
                }
                else
                {
                    parent.right = node;
                }
            }

            count++;
            return node;
        }

        public void Delete(TreeNode<TKey, TValue> node)
        {
            bool isOneLinkFree = node.left == null || node.right == null;
            TreeNode<TKey, TValue> y = isOneLinkFree ? node : FindSuccessor(node);
            bool hasLeftLink = y.left != null;
            TreeNode<TKey, TValue> x = hasLeftLink ? y.left : y.right;
            /*if (x != null)
                x.parent = y.parent;
            */
            TreeNode<TKey, TValue> yParent = getParentNode(y);
            if (yParent == null)
            {
                root = x;
            }
            else
            {
                if (yParent.left == y)
                {
                    yParent.left = x;
                }
                else
                {
                    yParent.right = x;
                }
            }
            if (y != node)
            {
                node.key = y.key;
                node.value = y.value;
            }

        }

        public TreeNode<TKey, TValue> Search(TKey key)
        {
            TreeNode<TKey, TValue> node = root;
            int cmp;
            while (node != null)
            {
                cmp = key.CompareTo(node.key);
                if (cmp == 0)   // found !
                    return node;

                if (cmp < 0)
                {
                    node = node.left;
                }
                else
                {
                    node = node.right;
                }
            }
            return null;
        }

        public TreeNode<TKey, TValue> Minimum()
        {
            return minimum(root);
        }

        public TreeNode<TKey, TValue> Maximum()
        {
            return maximum(root);
        }

        public TreeNode<TKey, TValue> FindSuccessor(TreeNode<TKey, TValue> node)
        {
            if (node.right != null)
            {
                return minimum(node.right);
            }
            TreeNode<TKey, TValue> successor = getParentNode(node);
            while ((successor != null) && (successor.right == node))
            {
                node = successor;
                successor = getParentNode(node);
            }
            return successor;
        }

        public TreeNode<TKey, TValue> FindPredeccessor(TreeNode<TKey, TValue> node)
        {
            if (node.left != null)
            {
                return maximum(node.left);
            }
            TreeNode<TKey, TValue> predeccessor = getParentNode(node);
            while ((predeccessor != null) && (predeccessor.left == node))
            {
                node = predeccessor;
                predeccessor = getParentNode(node);
            }
            return predeccessor;
        }

        private string drawNode(TreeNode<TKey, TValue> node)
        {
            if (node == null)
                return "_";

            bool isLeaf = node.left == null && node.right == null;

            return node.key.ToString() + (!isLeaf ? "(" + drawNode(node.left) + ", " + drawNode(node.right) + ")" : string.Empty);
        }

        private void killTree(ref TreeNode<TKey, TValue> p)
        {
            if (p != null)
            {
                killTree(ref p.left);
                killTree(ref p.right);
                p = null;
            }
        }

        private void addRecursively(TreeNode<TKey, TValue> node, ref TreeNode<TKey, TValue> tree)
        {
            if (tree == null)
                tree = node;
            else
            {
                // If we find a node with the same key then it's 
                // a duplicate and we can't continue
                int comparison = node.key.CompareTo(tree.key);
                if (comparison == 0)
                    throw new Exception();

                if (comparison < 0)
                {
                    addRecursively(node, ref tree.left);
                }
                else
                {
                    addRecursively(node, ref tree.right);
                }
            }
        }

        private TreeNode<TKey, TValue> minimum(TreeNode<TKey, TValue> node)
        {
            TreeNode<TKey, TValue> min = node;
            if (min != null)
            {
                while (min.left != null)
                {
                    min = min.left;
                }
            }
            return min;
        }

        private TreeNode<TKey, TValue> maximum(TreeNode<TKey, TValue> node)
        {
            TreeNode<TKey, TValue> max = node;
            if (max != null)
            {
                while (max.right != null)
                {
                    max = max.right;
                }
            }
            return max;
        }

        private TreeNode<TKey, TValue> getParentNode(TreeNode<TKey, TValue> node)
        {
            TreeNode<TKey, TValue> x = root;
            TreeNode<TKey, TValue> xParent = null;
            int cmp;
            while (x != node)
            {
                cmp = node.key.CompareTo(x.key);

                if (cmp < 0)
                {
                    xParent = x;
                    x = x.left;
                }
                else
                {
                    xParent = x;
                    x = x.right;
                }
            }
            return xParent;
        }

        public string Save()
        {
            return save(root);
        }

        private string save(TreeNode<TKey, TValue> node)
        {
            if (node == null)
                return string.Empty;

            return node.key.ToString() + " " + save(node.left) + " " + save(node.right);
        }


    }


}
