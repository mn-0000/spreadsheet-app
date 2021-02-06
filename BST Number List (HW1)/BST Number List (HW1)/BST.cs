using System;


namespace HW1
{
    public class BinarySearchTree
    {
        static Node root;

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

                // Create the new node and attach it to the parent node
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

        private Node CreateNewNode(int value)
        {
            return new Node(value);
        }

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

        public static void PrintSortedBST()
        {
            PrintSortedBST(root);
        }

        public int Count(Node node)
        {
            if (node != null)
            {
                return 1 + Count(node.left) + Count(node.right);
            }
            return 0;
        }

        public int Count()
        {
            return Count(root);
        }

        public int Height(Node node)
        {
            if (node != null)
            {
                return 1 + Math.Max(Height(node.left), Height(node.right));
            }
            return 0;
        }

        public int Height()
        {
            return Height(root);
        }
    }
}