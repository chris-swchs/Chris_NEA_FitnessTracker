using System.Collections.Generic;

namespace NEA
{
    public static class CompatibilityDefinitions
    {
        public static CompatibilityMatrix<WorkoutType, Goal>
            WorkoutType_Goal
        { get; private set; }

        public static CompatibilityMatrix<WorkoutType, MainMuscleGroup> WorkoutType_MuscleGroup { get; private set; }

        public static CompatibilityMatrix<WorkoutType, Frequency>
            WorkoutType_CardioPreference
        { get; private set; }

        static CompatibilityDefinitions()
        {
            WorkoutType_Goal = new CompatibilityMatrix<WorkoutType, Goal>();

            WorkoutType_MuscleGroup = new CompatibilityMatrix<WorkoutType, MainMuscleGroup>();

            WorkoutType_CardioPreference = new CompatibilityMatrix<WorkoutType, Frequency>();

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
                    { Goal.CardiovascularFitness, 0.65 },
                    { Goal.GeneralFitness, 1.00 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.UpperBody,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.95 },
                    { Goal.CardiovascularFitness, 0.30 },
                    { Goal.GeneralFitness, 0.75 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.LowerBody,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.95 },
                    { Goal.CardiovascularFitness, 0.50 },
                    { Goal.GeneralFitness, 0.80 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.Push,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.95 },
                    { Goal.CardiovascularFitness, 0.20 },
                    { Goal.GeneralFitness, 0.65 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.Pull,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.95 },
                    { Goal.CardiovascularFitness, 0.20 },
                    { Goal.GeneralFitness, 0.65 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.Legs,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 1.00 },
                    { Goal.CardiovascularFitness, 0.35 },
                    { Goal.GeneralFitness, 0.70 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.Core,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.55 },
                    { Goal.CardiovascularFitness, 0.55 },
                    { Goal.GeneralFitness, 0.80 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.Cardio,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.20 },
                    { Goal.CardiovascularFitness, 1.00 },
                    { Goal.GeneralFitness, 0.85 }
                });

            WorkoutType_Goal.AddRange(
                WorkoutType.Mobility,
                new Dictionary<Goal, double>
                {
                    { Goal.Strength, 0.30 },
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
                    { MainMuscleGroup.Triceps, 0.75 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.UpperBody,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.95 },
                    { MainMuscleGroup.Triceps, 0.90 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.LowerBody,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.05 },
                    { MainMuscleGroup.Triceps, 0.05 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.Push,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 1.00 },
                    { MainMuscleGroup.Triceps, 1.00 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.Pull,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.10 },
                    { MainMuscleGroup.Triceps, 0.10 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.Legs,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.05 },
                    { MainMuscleGroup.Triceps, 0.05 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.Core,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.15 },
                    { MainMuscleGroup.Triceps, 0.15 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.Cardio,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.10 },
                    { MainMuscleGroup.Triceps, 0.10 }
                });

            WorkoutType_MuscleGroup.AddRange(
                WorkoutType.Mobility,
                new Dictionary<MainMuscleGroup, double>
                {
                    { MainMuscleGroup.Chest, 0.20 },
                    { MainMuscleGroup.Triceps, 0.20 }
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