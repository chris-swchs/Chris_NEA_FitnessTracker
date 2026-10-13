using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Models.LinkedLists
{
    public class LinkedList
    {
        private Node head; // The starting entry point of the list

        // Add a node to the very end of the list
        public void Append(string data)
        {
            Node newNode = new Node(data);

            // If the list is empty, make this the head
            if (head == null)
            {
                head = newNode;
                return;
            }

            // Otherwise, traverse to the end of the list
            Node current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }

            // Link the last node to our new node
            current.Next = newNode;
        }

        // Add a node to the very front of the list
        public void Prepend(string data)
        {
            Node newNode = new Node(data);
            newNode.Next = head; // Point new node to the old head
            head = newNode;      // Move head to point to the new node
        }

        // Returns the data at a specific index 
        public string GetAt(int index)
        {
            // Case 1: Index cannot be negative
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative.");
            }

            Node current = head;
            int counter = 0;

            // Step through the chain until we reach the index or the end of the list
            while (current != null)
            {
                if (counter == index)
                {
                    return current.Data; // Found it!
                }

                current = current.Next; // Move to the next node
                counter++;
            }

            // Case 2: Index is larger than the number of nodes in the list
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range of the list.");
        }

        // Print the full list layout
        public void PrintList()
        {
            Node current = head;
            while (current != null)
            {
                Console.Write(current.Data + " -> ");
                current = current.Next;
            }
            Console.WriteLine("null");
        }
    }
}
