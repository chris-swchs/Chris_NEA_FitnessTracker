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
        Legs,
        Core
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

    public enum MovementPattern
    {
        VerticalPull,
        HorizontalPush,
        VerticalPush,
        HorizontalPull,
        HorizontalAbduction,
        HorizontalAdduction,
        ElbowExtension,
        ElbowFlexion,
        ShoulderAbduction,
        ShoulderFlexion,
        Lunge,
        HipExtension,
        HipHinge,
        HipAdduction,
        HipAbduction,
        Squat,
        AnklePlantarFlexion,
        CoreFlexion,
        CoreStabilisation,
        CoreRotation,
        WristExtension,
        WristFlexion,
        Running,
        Cycling,
        Rowing,
        Other
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
        public static MainMuscleGroup[] GetWorkoutTypeMainMuscleGroups(WorkoutType workoutType)
        {
            switch (workoutType)
            {
                case WorkoutType.FullBody:
                    return new MainMuscleGroup[] { MainMuscleGroup.Chest, MainMuscleGroup.Shoulders, MainMuscleGroup.Biceps, MainMuscleGroup.Triceps, MainMuscleGroup.Forearms, MainMuscleGroup.Back, MainMuscleGroup.Legs,  MainMuscleGroup.Core };

                case WorkoutType.UpperBody:
                    return new MainMuscleGroup[] { MainMuscleGroup.Chest, MainMuscleGroup.Shoulders, MainMuscleGroup.Biceps, MainMuscleGroup.Triceps, MainMuscleGroup.Forearms, MainMuscleGroup.Back, MainMuscleGroup.Core };

                case WorkoutType.LowerBody:
                    return new MainMuscleGroup[] { MainMuscleGroup.Legs };

                case WorkoutType.Push:
                    return new MainMuscleGroup[] { MainMuscleGroup.Chest, MainMuscleGroup.Shoulders, MainMuscleGroup.Triceps };

                case WorkoutType.Pull:
                    return new MainMuscleGroup[] { MainMuscleGroup.Back, MainMuscleGroup.Biceps, MainMuscleGroup.Forearms, MainMuscleGroup.Shoulders };

                case WorkoutType.Legs:
                    return new MainMuscleGroup[] { MainMuscleGroup.Legs };

                case WorkoutType.Core:
                    return new MainMuscleGroup[] { MainMuscleGroup.Core };

                case WorkoutType.Cardio:
                    return new MainMuscleGroup[] { MainMuscleGroup.Legs };

                case WorkoutType.Mobility:
                    return new MainMuscleGroup[] { MainMuscleGroup.Chest, MainMuscleGroup.Shoulders, MainMuscleGroup.Biceps, MainMuscleGroup.Triceps, MainMuscleGroup.Forearms, MainMuscleGroup.Back, MainMuscleGroup.Legs, MainMuscleGroup.Core };

                default:
                    return new MainMuscleGroup[] { };
            }
        }

        public static MuscleGroup[] GetMuscleGroupsFromMain(MainMuscleGroup mainMuscleGroup)
        {
            switch (mainMuscleGroup)
            {
                case MainMuscleGroup.Chest:
                    return new MuscleGroup[] { MuscleGroup.UpperChest, MuscleGroup.LowerChest };

                case MainMuscleGroup.Shoulders:
                    return new MuscleGroup[] { MuscleGroup.FrontShoulders, MuscleGroup.SideShoulders, MuscleGroup.RearShoulders };

                case MainMuscleGroup.Biceps:
                    return new MuscleGroup[] { MuscleGroup.Biceps };

                case MainMuscleGroup.Triceps:
                    return new MuscleGroup[] { MuscleGroup.Triceps };

                case MainMuscleGroup.Forearms:
                    return new MuscleGroup[] { MuscleGroup.Forearms };

                case MainMuscleGroup.Back:
                    return new MuscleGroup[] { MuscleGroup.UpperBack, MuscleGroup.LowerBack };

                case MainMuscleGroup.Legs:
                    return new MuscleGroup[] { MuscleGroup.Quads, MuscleGroup.Hamstrings, MuscleGroup.Glutes, MuscleGroup.Adductors, MuscleGroup.Abductors, MuscleGroup.Calves };

                case MainMuscleGroup.Core:
                    return new MuscleGroup[] { MuscleGroup.UpperCore, MuscleGroup.LowerCore, MuscleGroup.Obliques };

                default:
                    return new MuscleGroup[] { };
            }
        }
}