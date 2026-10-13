using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NEA.Models.LinkedLists
{
    public class Node
    {
        public string Data { get; set; }
        public Node Next { get; set; } // Nullable, as the last node points to null

        public Node(string data)
        {
            Data = data;
            Next = null; // Default pointer is null
        }
    }
}
