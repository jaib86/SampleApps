using System;
using System.Collections.Generic;
using System.Text;

namespace Ritambhara
{
    public static class LinkListExample
    {
        /// <summary>
        /// Finding middle element in a linked list
        /// </summary>
        /// <returns></returns>
        public static Node GetMiddleElement(Node listHead)
        {
            Node leftPointer = listHead;
            Node rightPointer = listHead;

            while (rightPointer?.Next != null)
            {
                leftPointer = leftPointer.Next;
                rightPointer = rightPointer.Next.Next;
            }

            return leftPointer;
        }

        /// <summary>
        /// Generate a linked list and return head of the linked list.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Node CreateLinkList(int count)
        {
            Node head = null;

            if (count > 0)
            {
                var random = new Random((int)DateTime.Now.Ticks);

                head = new Node { Data = random.Next(100) };

                Node node = head;

                for (int i = 1; i < count; i++)
                {
                    node.Next = new Node { Data = random.Next(100) };

                    node = node.Next;
                }
            }

            return head;
        }
    }

    public class Node
    {
        public int Data { get; set; }

        public Node Next { get; set; }
    }
}
