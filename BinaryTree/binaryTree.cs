using System;
using System.Collections;

namespace Algorithm.DataStructures
{
    // Define tree nodes
    public class TreeNode<TValue>
    {
        public TValue value;
        public TreeNode<TValue> left, right;

        public TreeNode(TValue v)
        {
            value = v;
            left = null;
            right = null;
        }
    }

	// The Binary tree itself
	public class BinarySearchTree<TItem>
	{
	    private TreeNode<TItem> root;     // Points to the root of the tree
		private int count = 0;

        public BinarySearchTree()
		{
		    root = null;
			count = 0;
		}

        private void killTree(ref TreeNode<TItem> p)
        {
           if (p != null) {
             killTree (ref p.left);
             killTree (ref p.right);
             p = null;
		   }
        }

		/// <summary>
		/// Clear the binary tree.
		/// </summary>
		public void clear() {
		   killTree (ref root);
		   count = 0;
		} 
			
		/// <summary>
		/// Returns the number of nodes in the tree
		/// </summary>
		/// <returns>Number of nodes in the tree</returns>
		public int Count() {
			return count;
		}      

		/// <summary>
		/// Add a symbol to the tree if it's a new one. Returns reference to the new
		/// node if a new node inserted, else returns null to indicate node already present.
		/// </summary>
		/// <param name="v">Value of node</param>
		/// <returns> Returns reference to the new node is the node was inserted.
		/// If a duplicate node (same name was located then returns null</returns>
        public TreeNode<TItem> insert(TItem v)
        {
            TreeNode<TItem> node = new TreeNode<TItem>(v);
			try {
                if (root == null)
                {
                    root = node;
                }
                else
                {
                    // If we find a node with the same name then it's 
                    // a duplicate and we can't continue
                    int comparison = String.Compare(node.name, tree.name);
                    if (comparison == 0)
                        throw new Exception();

                    if (comparison < 0)
                    {
                        add(node, ref tree.left);
                    }
                    else
                    {
                        add(node, ref tree.right);
                    }
                }
				count++;
				return node;
			} catch (Exception) {
				return null;
			}
		}

		// Searches for a node with name key, name. If found it returns a reference
		// to the node and to thenodes parent. Else returns null.
		private TreeNode findParent (string name, ref TreeNode parent) {
			TreeNode np = root;
			parent = null;
			int cmp;
			while (np != null) {
				cmp = String.Compare (name, np.name);
				if (cmp == 0)   // found !
					return np;

				if (cmp < 0) { 
					parent = np;
					np = np.left;
				}
				else {
					parent = np;
					np = np.right;
				}
			}
			return null;  // Return null to indicate failure to find name
		}

		/// <summary>
		/// Find the next ordinal node starting at node startNode.
		/// Due to the structure of a binary search tree, the
		/// successor node is simply the left most node on the right branch.
		/// </summary>
		/// <param name="startNode">Name key to use for searching</param>
		/// <param name="parent">Returns the parent node if search successful</param>
		/// <returns>Returns a reference to the node if successful, else null</returns>
		public TreeNode findSuccessor (TreeNode startNode, ref TreeNode parent) {
			parent = startNode;
			// Look for the left-most node on the right side
			startNode = startNode.right; 
			while (startNode.left != null) {
				parent = startNode;
				startNode = startNode.left;
			}
			return startNode;
		}

		/// <summary>
		/// Delete a given node. This is the more complex method in the binary search
		/// class. The method considers three senarios, 1) the deleted node has no
		/// children; 2) the deleted node as one child; 3) the deleted node has two
		/// children. Case one and two are relatively simple to handle, the only
		/// unusual considerations are when the node is the root node. Case 3) is
		/// much more complicated. It requires the location of the successor node.
		/// The node to be deleted is then replaced by the sucessor node and the
		/// successor node itself deleted. Throws an exception if the method fails
		/// to locate the node for deletion.
		/// </summary>
		/// <param name="key">Name key of node to delete</param>
		public void delete (string key) {
			TreeNode parent = null;
			// First find the node to delete and its parent
			TreeNode nodeToDelete = findParent (key, ref parent);
			if (nodeToDelete == null) 
				throw new Exception ("Unable to delete node: " + key.ToString());  // can't find node, then say so 
			
			// Three cases to consider, leaf, one child, two children

			// If it is a simple leaf then just null what the parent is pointing to
			if ((nodeToDelete.left == null) && (nodeToDelete.right == null)) {
				if (parent == null) {
					root = null;
					return;
				}

				// find out whether left or right is associated 
				// with the parent and null as appropriate
				if (parent.left == nodeToDelete)
					parent.left = null;
				else
					parent.right = null;
				count--;
				return;
			}

			// One of the children is null, in this case
			// delete the node and move child up
			if (nodeToDelete.left == null) {
				// Special case if we're at the root
				if (parent == null) {
					root = nodeToDelete.right;
					return;
				}

				// Identify the child and point the parent at the child
				if (parent.left == nodeToDelete)
                    parent.right = nodeToDelete.right;
				else 
					parent.left = nodeToDelete.right;
				nodeToDelete = null; // Clean up the deleted node
				count--;
				return;
			}

			// One of the children is null, in this case
			// delete the node and move child up
			if (nodeToDelete.right == null) {
				// Special case if we're at the root			
				if (parent == null) {
					root = nodeToDelete.left;
					return;
				}

				// Identify the child and point the parent at the child
				if (parent.left == nodeToDelete)
					parent.left = nodeToDelete.left;
				else 
					parent.right = nodeToDelete.left;
				nodeToDelete = null; // Clean up the deleted node
				count--;
				return;
			}

			// Both children have nodes, therefore find the successor, 
			// replace deleted node with successor and remove successor
			// The parent argument becomes the parent of the successor
			TreeNode successor = findSuccessor (nodeToDelete, ref parent);
			// Make a copy of the successor node
			TreeNode tmp = new TreeNode (successor.name, successor.value);
			// Find out which side the successor parent is pointing to the
			// successor and remove the successor
			if (parent.left == successor)
				parent.left = null;
			else
				parent.right = null;

			// Copy over the successor values to the deleted node position
			nodeToDelete.name = tmp.name;
			nodeToDelete.value = tmp.value;
			count--;
		}


		// Simple 'drawing' routines
		private string drawNode (TreeNode node) {
			if (node == null)
				return "empty";

			if ((node.left == null) && (node.right == null))
				return node.name;
			if ((node.left != null) && (node.right == null))
				return node.name + "(" + drawNode (node.left) + ", _)";
			
			if ((node.right != null) && (node.left == null))
				return node.name + "(_, " + drawNode (node.right) + ")";

			return node.name + "(" + drawNode (node.left) + ", " + drawNode (node.right) + ")";
		}


		/// <summary>
		/// Return the tree depicted as a simple string, useful for debugging, eg
		/// 50(40(30(20, 35), 45(44, 46)), 60)
		/// </summary>
		/// <returns>Returns the tree</returns>
 		public string drawTree() {
			return drawNode (root);
		}

	}
}
