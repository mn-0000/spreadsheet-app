using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW1;

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
                // Locate the parent node
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

        public static void PrintSortedBST(Node root)
        {
            if (root != null)
            {
                PrintSortedBST(root.left);
                Console.Write("{0} ", root.data);
                PrintSortedBST(root.right);
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
    }
}