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

    public enum MainMuscleGroup
    {
        Chest,
        Shoulders,
        Biceps,
        Triceps,
        Forearms,
        Back,
        Legs
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
        FullBody,
        UpperBody,
        LowerBody,
        Push,
        Pull,
        Legs,
        Core,
        Cardio,
        Mobility
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

    //  Contains functions for gathering dater related to a defined enum within the program
    public static class EnumDefinitions
    {
        // TO DO - Make it return linked  list of muscle groups
        public static int GetWorkoutTypeMuscleGroups(WorkoutType workoutType)
        {
            switch (workoutType)
            {
                case WorkoutType.Pull:
                    return 999;
                default:
                    return 3;
            }
        }
    }
}