using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public enum Goal
    {
        Strength,
        MuscleGrowth,
        CardiovascularFitness,
        GeneralFitness,
        MuscularEndurance,
        Power,
        Mobility
    }

    public enum ExperienceLevel
    {
        Beginner,
        Intermediate,
        Advance
    }

    public enum Frequency
    {
        Low,
        Moderate,
        High
    }
    public enum Difficulty
    {
        Easy,
        Moderate,
        Hard
    }
}
