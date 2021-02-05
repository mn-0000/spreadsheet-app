using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BST_Number_List__HW1_
{
    public class Node
    {
        public int data;
        public Node left;
        public Node right;

        public Node(int value)
        {
            data = value;
        }
    }

    public class BinarySearchTree
    {
        Node root;
        public void Insert(int value)
        {
            ///    if (curr == null)
            ///    {
            ///        curr = new Node();
            ///        curr.data = value;
            ///    } else if (value > curr.data)
            ///    {
            ///        curr.left = Insert(curr.left, value);
            ///    } else
            ///    {
            ///        curr.right = Insert(curr.right, value);
            ///    }
            ///    return curr;
            ///}
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

        protected Node CreateNewNode(int value)
        {
            return new Node(value);
        }


    }
}