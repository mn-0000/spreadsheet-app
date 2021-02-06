using System;

namespace HW1
{
    public class BinarySearchTree
    {
        static Node root;

        /// <summary>
        /// Inserts a given value into the BST.
        /// </summary>
        /// <param name="value"></param>
        public void Insert(int value)
        {
            if (root == null)
            {
                root = CreateNewNode(value); // Create a new root
            }
            else
            {
                Node parent = null;
                Node current = root;
                while (current != null)
                {
                    if (value < current.data)
                    {
                        parent = current;
                        current = current.left;
                    }
                    else
                    {
                        parent = current;
                        current = current.right;
                    }
                }

                // Create new node and attach it to the parent node
                if (value < parent.data)
                {
                    parent.left = CreateNewNode(value);
                }
                else
                {
                    parent.right = CreateNewNode(value);
                }
            }
        }

        /// <summary>
        /// Creates a new node for the BST given a certain value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> New node </returns>
        private Node CreateNewNode(int value)
        {
            return new Node(value);
        }

        /// <summary>
        /// Prints all elements of the BST sorted from lowest to highest value.
        /// </summary>
        /// <param name="node"></param>
        public static void PrintSortedBST(Node node)
        {
            if (node != null)
            {
                PrintSortedBST(node.left);
                Console.Write("{0} ", node.data);
                PrintSortedBST(node.right);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Overload method for the PrintSortedBST method so that it can be called from Main().
        /// </summary>
        public static void PrintSortedBST()
        {
            PrintSortedBST(root);
        }

        /// <summary>
        /// Determines the number of elements in the BST.
        /// </summary>
        /// <param name="node"></param>
        /// <returns> number of elements in BST </returns>
        public int Count(Node node)
        {
            if (node != null)
            {
                return 1 + Count(node.left) + Count(node.right);
            }
            return 0;
        }

        /// <summary>
        /// Overload method for the Count method so that it can be called from Main().
        /// </summary>
        public int Count()
        {
            return Count(root);
        }

        /// <summary>
        /// Determine the number of levels of the BST.
        /// </summary>
        /// <param name="node"></param>
        /// <returns> number of levels </returns>
        public int DetermineLevel(Node node)
        {
            if (node != null)
            {
                return 1 + Math.Max(DetermineLevel(node.left), DetermineLevel(node.right));
            }
            return 0;
        }

        /// <summary>
        /// Overload method for the DetermineLevel method so that it can be called from Main().
        /// </summary>
        public int DetermineLevel()
        {
            return DetermineLevel(root);
        }
    }
}