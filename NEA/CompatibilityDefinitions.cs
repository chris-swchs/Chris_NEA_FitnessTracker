using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    public static class CompatibilityDefinitions
    {
        public static CompatibilityMatrix<WorkoutType, Goal> WorkoutType_Goal { get; private set; }
        public static CompatibilityMatrix<WorkoutType, MainMuscleGroup> WorkoutType_MuscleGroup { get; private set; }
        public static CompatibilityMatrix<WorkoutType, Frequency> WorkoutType_CardioPreference { get; private set; }

        static CompatibilityDefinitions()
        {
            WorkoutType_Goal = new CompatibilityMatrix<WorkoutType, Goal>();
            WorkoutType_MuscleGroup = new CompatibilityMatrix<WorkoutType, MainMuscleGroup>();
            WorkoutType_CardioPreference = new CompatibilityMatrix<WorkoutType, Frequency>();

            InitialiseWorkoutType_Goals();
            InitialiseWorkoutType_MuscleGroup();
            InitialiseWorkoutType_CardioPreferene();
        }
        private static void InitialiseWorkoutType_Goals()
        {
            WorkoutType_Goal.AddRange(WorkoutType.Strength, new Dictionary<Goal, double>
            {
                {Goal.Strength, 0.95},
                {Goal.CardiovascularFitness, 0.4},
                {Goal.GeneralFitness, 0.6}
            });
            WorkoutType_Goal.AddRange(WorkoutType.Cardio, new Dictionary<Goal, double>
            {
                {Goal.Strength, 0.2},
                {Goal.CardiovascularFitness, 0.95},
                {Goal.GeneralFitness, 0.7},
            });
        }
        private static void InitialiseWorkoutType_MuscleGroup()
        {
            WorkoutType_MuscleGroup.AddRange(WorkoutType.Strength, new Dictionary<MainMuscleGroup, double>
            {
                {MainMuscleGroup.Chest, 0.8 },
                {MainMuscleGroup.Triceps, 0.8 }
            });
            WorkoutType_MuscleGroup.AddRange(WorkoutType.Cardio, new Dictionary<MainMuscleGroup, double>
            {
                {MainMuscleGroup.Chest, 0.1 },
                {MainMuscleGroup.Triceps, 0.1 }
            });
        }

        private static void InitialiseWorkoutType_CardioPreferene()
        {
            WorkoutType_CardioPreference.AddRange(WorkoutType.Strength, new Dictionary<Frequency, double>
            {
                {Frequency.Low, 0.9 },
                {Frequency.Moderate, 0.5 },
                {Frequency.High, 0.1 },
            });
            WorkoutType_CardioPreference.AddRange(WorkoutType.Cardio, new Dictionary<Frequency, double>
            {
                {Frequency.Low, 0.1 },
                {Frequency.Moderate, 0.5 },
                {Frequency.High, 0.9 },
            });
        }
    }
}
