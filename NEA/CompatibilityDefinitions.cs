
using System.Collections.Generic;

namespace NEA
{
    public static class CompatibilityDefinitions
    {
        public static CompatibilityMatrix<WorkoutType, Goal>
            WorkoutType_Goal
        { get; private set; }

        public static CompatibilityMatrix<WorkoutType, MainMuscleGroup>
            WorkoutType_MuscleGroup
        { get; private set; }

        public static CompatibilityMatrix<WorkoutType, Frequency>
            WorkoutType_CardioPreference
        { get; private set; }

        static CompatibilityDefinitions()
        {
            WorkoutType_Goal =
                new CompatibilityMatrix<WorkoutType, Goal>();

            WorkoutType_MuscleGroup =
                new CompatibilityMatrix<WorkoutType, MainMuscleGroup>();

            WorkoutType_CardioPreference =
                new CompatibilityMatrix<WorkoutType, Frequency>();

            InitialiseWorkoutType_Goals();
            InitialiseWorkoutType_MuscleGroup();
            InitialiseWorkoutType_CardioPreference();
        }

        private static void InitialiseWorkoutType_Goals()
        {
            WorkoutType_Goal.AddRange(
                WorkoutType.FullBody,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.85 },
                    { Goal.MuscleGrowth, 0.90 },
                    { Goal.CardiovascularFitness, 0.65 },
                    { Goal.GeneralFitness, 1.00 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.UpperBody,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.95 },
                    { Goal.MuscleGrowth, 1.00 },
                    { Goal.CardiovascularFitness, 0.30 },
                    { Goal.GeneralFitness, 0.75 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.LowerBody,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.95 },
                    { Goal.MuscleGrowth, 1.00 },
                    { Goal.CardiovascularFitness, 0.50 },
                    { Goal.GeneralFitness, 0.80 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.Push,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.95 },
                    { Goal.MuscleGrowth, 1.00 },
                    { Goal.CardiovascularFitness, 0.20 },
                    { Goal.GeneralFitness, 0.65 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.Pull,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.95 },
                    { Goal.MuscleGrowth, 1.00 },
                    { Goal.CardiovascularFitness, 0.20 },
                    { Goal.GeneralFitness, 0.65 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.Legs,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 1.00 },
                    { Goal.MuscleGrowth, 1.00 },
                    { Goal.CardiovascularFitness, 0.35 },
                    { Goal.GeneralFitness, 0.70 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.Core,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.55 },
                    { Goal.MuscleGrowth, 0.55 },
                    { Goal.CardiovascularFitness, 0.55 },
                    { Goal.GeneralFitness, 0.80 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.Cardio,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.20 },
                    { Goal.MuscleGrowth, 0.15 },
                    { Goal.CardiovascularFitness, 1.00 },
                    { Goal.GeneralFitness, 0.85 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.Mobility,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.30 },
                    { Goal.MuscleGrowth, 0.20 },
                    { Goal.CardiovascularFitness, 0.35 },
                    { Goal.GeneralFitness, 0.80 }
                });
        }

        private static void InitialiseWorkoutType_MuscleGroup()
        {
            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.FullBody,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.80 },
                    { MainMuscleGroup.Shoulders, 0.80 },
                    { MainMuscleGroup.Biceps, 0.75 },
                    { MainMuscleGroup.Triceps, 0.75 },
                    { MainMuscleGroup.Forearms, 0.65 },
                    { MainMuscleGroup.Back, 0.85 },
                    { MainMuscleGroup.Legs, 0.90 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.UpperBody,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.95 },
                    { MainMuscleGroup.Shoulders, 0.95 },
                    { MainMuscleGroup.Biceps, 0.95 },
                    { MainMuscleGroup.Triceps, 0.90 },
                    { MainMuscleGroup.Forearms, 0.85 },
                    { MainMuscleGroup.Back, 0.95 },
                    { MainMuscleGroup.Legs, 0.05 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.LowerBody,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.05 },
                    { MainMuscleGroup.Shoulders, 0.05 },
                    { MainMuscleGroup.Biceps, 0.05 },
                    { MainMuscleGroup.Triceps, 0.05 },
                    { MainMuscleGroup.Forearms, 0.10 },
                    { MainMuscleGroup.Back, 0.20 },
                    { MainMuscleGroup.Legs, 1.00 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.Push,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 1.00 },
                    { MainMuscleGroup.Shoulders, 1.00 },
                    { MainMuscleGroup.Biceps, 0.10 },
                    { MainMuscleGroup.Triceps, 1.00 },
                    { MainMuscleGroup.Forearms, 0.20 },
                    { MainMuscleGroup.Back, 0.10 },
                    { MainMuscleGroup.Legs, 0.05 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.Pull,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.10 },
                    { MainMuscleGroup.Shoulders, 0.40 },
                    { MainMuscleGroup.Biceps, 1.00 },
                    { MainMuscleGroup.Triceps, 0.10 },
                    { MainMuscleGroup.Forearms, 0.95 },
                    { MainMuscleGroup.Back, 1.00 },
                    { MainMuscleGroup.Legs, 0.10 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.Legs,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.05 },
                    { MainMuscleGroup.Shoulders, 0.05 },
                    { MainMuscleGroup.Biceps, 0.05 },
                    { MainMuscleGroup.Triceps, 0.05 },
                    { MainMuscleGroup.Forearms, 0.10 },
                    { MainMuscleGroup.Back, 0.20 },
                    { MainMuscleGroup.Legs, 1.00 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.Core,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.15 },
                    { MainMuscleGroup.Shoulders, 0.20 },
                    { MainMuscleGroup.Biceps, 0.10 },
                    { MainMuscleGroup.Triceps, 0.15 },
                    { MainMuscleGroup.Forearms, 0.15 },
                    { MainMuscleGroup.Back, 0.45 },
                    { MainMuscleGroup.Legs, 0.30 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.Cardio,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.10 },
                    { MainMuscleGroup.Shoulders, 0.20 },
                    { MainMuscleGroup.Biceps, 0.10 },
                    { MainMuscleGroup.Triceps, 0.10 },
                    { MainMuscleGroup.Forearms, 0.10 },
                    { MainMuscleGroup.Back, 0.20 },
                    { MainMuscleGroup.Legs, 0.60 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.Mobility,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.20 },
                    { MainMuscleGroup.Shoulders, 0.40 },
                    { MainMuscleGroup.Biceps, 0.20 },
                    { MainMuscleGroup.Triceps, 0.20 },
                    { MainMuscleGroup.Forearms, 0.20 },
                    { MainMuscleGroup.Back, 0.40 },
                    { MainMuscleGroup.Legs, 0.40 }
                });
        }

        private static void InitialiseWorkoutType_CardioPreference()
        {
            WorkoutType_CardioPreference.AddRange(
                WorkoutType.FullBody,
                new Dictionary<Frequency, double>
                {
                    { Frequency.Low, 0.65 },
                    { Frequency.Moderate, 0.95 },
                    { Frequency.High, 0.70 }
                });

            WorkoutType_CardioPreference.AddRange(
                WorkoutType.UpperBody,
                new Dictionary<Frequency, double>
                {
                    { Frequency.Low, 0.95 },
                    { Frequency.Moderate, 0.65 },
                    { Frequency.High, 0.25 }
                });

            WorkoutType_CardioPreference.AddRange(
                WorkoutType.LowerBody,
                new Dictionary<Frequency, double>
                {
                    { Frequency.Low, 0.85 },
                    { Frequency.Moderate, 0.70 },
                    { Frequency.High, 0.35 }
                });

            WorkoutType_CardioPreference.AddRange(
                WorkoutType.Push,
                new Dictionary<Frequency, double>
                {
                    { Frequency.Low, 1.00 },
                    { Frequency.Moderate, 0.60 },
                    { Frequency.High, 0.20 }
                });

            WorkoutType_CardioPreference.AddRange(
                WorkoutType.Pull,
                new Dictionary<Frequency, double>
                {
                    { Frequency.Low, 1.00 },
                    { Frequency.Moderate, 0.60 },
                    { Frequency.High, 0.20 }
                });

            WorkoutType_CardioPreference.AddRange(
                WorkoutType.Legs,
                new Dictionary<Frequency, double>
                {
                    { Frequency.Low, 0.90 },
                    { Frequency.Moderate, 0.65 },
                    { Frequency.High, 0.30 }
                });

            WorkoutType_CardioPreference.AddRange(
                WorkoutType.Core,
                new Dictionary<Frequency, double>
                {
                    { Frequency.Low, 0.65 },
                    { Frequency.Moderate, 0.85 },
                    { Frequency.High, 0.70 }
                });

            WorkoutType_CardioPreference.AddRange(
                WorkoutType.Cardio,
                new Dictionary<Frequency, double>
                {
                    { Frequency.Low, 0.10 },
                    { Frequency.Moderate, 0.65 },
                    { Frequency.High, 1.00 }
                });

            WorkoutType_CardioPreference.AddRange(
                WorkoutType.Mobility,
                new Dictionary<Frequency, double>
                {
                    { Frequency.Low, 0.75 },
                    { Frequency.Moderate, 0.80 },
                    { Frequency.High, 0.60 }
                });
        }
    }
}
