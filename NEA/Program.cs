using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NEA
{

    //Enum variables

    //Contains all of the possible sets of muscle groups
    public enum MuscleGroup
    {
        UpperChest,
        LowerChest,
        FrontShoulders,
        SideShoulders,
        RearShoulders,
        Triceps,
        Biceps,
        Forearms,
        UpperBack,
        LowerBack,
        UpperCore,
        LowerCore,
        Obliques,
        Quads,
        Hamstrings,
        Glutes,
        Adductors,
        Abductors,
        Calves
    }

    //Contains list of the possible sets of exercise types
    public enum ExerciseType
    {
        Strength,
        Flexibility,
        Cardio,
        Balance,
        Pylometric
    }

    //Contains list of the possible sets of workout types
    public enum WorkoutType
    {
        Strength,
        Cardio
    }

    //Contains list of all the possible equipments which are available to the program
    public enum Equipment
    {
        Bench,
        Dumbell,
        Barbell
    }


    public class Node
    {
        public int Data { get; set; }
        public Node Next { get; set; } // Nullable, as the last node points to null

        public Node(int data)
        {
            Data = data;
            Next = null; // Default pointer is null
        }
    }

    public class LinkedList
    {
        private Node head; // The starting entry point of the list

        // Add a node to the very end of the list
        public void Append(int data)
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
        public void Prepend(int data)
        {
            Node newNode = new Node(data);
            newNode.Next = head; // Point new node to the old head
            head = newNode;      // Move head to point to the new node
        }

        // Returns the data at a specific index 
        public int GetAt(int index)
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



    public class Set
    {
        public double Weight;
        public int Reps;
        private bool Completed = false;
        public Set(double weight, int reps)
        {
            Weight = weight;
            Reps = reps;
        }
        
        public void MarkCompleted()
        {
            Completed = true;
        }
    }

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DatabaseManager db = new DatabaseManager();
            db.InitializeDatabase();

            LinkedList list = new LinkedList();

            list.Append(10);
            list.Append(20);
            list.Prepend(5);

            Console.WriteLine("hello");

            list.PrintList();

            //Adding all the exercises
            db.AddExerciseRecord("flat Bench Press",
                ExerciseType.Strength,
                MuscleGroup.LowerChest,
                "Push the bar up and down while laying flat on a flat-adjusted bench");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
}
