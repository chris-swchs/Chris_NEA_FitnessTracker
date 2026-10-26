using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using NEA.Models.LinkedLists;

namespace NEA
{

     class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DatabaseManager DB = new DatabaseManager();
            DB.InitializeDatabase();

            LinkedList list = new LinkedList();

            list.Append("5");
            list.Append("10");
            list.Prepend("20");

            Console.WriteLine("hello");

            list.PrintList();

            //Adding all the exercises
            DB.AddExerciseRecord("flat Bench Press",
                ExerciseType.Strength,
                MuscleGroup.LowerChest,
                "Push the bar up and down while laying flat on a flat-adjusted bench");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
}
