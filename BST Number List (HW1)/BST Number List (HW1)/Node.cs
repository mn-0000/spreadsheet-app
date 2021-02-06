
namespace HW1
{
    public class Node
    {
        public int data;
        public Node left;
        public Node right;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="data"></param>
        public Node(int data)
        {
            this.data = data;
            left = null;
            right = null;
        }
    }

}
