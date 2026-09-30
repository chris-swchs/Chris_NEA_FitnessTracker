using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA
{

    //Enum variables

    //Contains all of the possible sets of muscle groups
    public enum MuscleGroup
    {
        Chest,
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

    //Contains all of the possible sets of exercise types
    public enum ExerciseType
    {
        Strength,
        Flexibility,
        Cardio,
        Balance,
        Pylometric
    }

    //Contains all of the possible sets of workout types
    public enum WorkoutType
    {
        Strength,
        Cardio
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
