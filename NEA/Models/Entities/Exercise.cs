using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Models.Entities
{
    public class Exercise
    {
        public int ExerciseID { get; set; }
        public string ExerciseName { get; set; }
        public string ExerciseDescription { get; set; }
        ExerciseType ExerciseType { get; set; }
        MuscleGroup TargetMuscle {  get; set; }
        public bool Completed { get; private set; }
        public int RestDuration { get; set; }

        public Dictionary<MuscleGroup, double> SecondaryMuscleGroups { get; private set; }
        public Exercise(string exerciseName, string exerciseDescription, ExerciseType exerciseType, MuscleGroup targetMuscle, int restDuration)
        {
            ExerciseName = exerciseName;
            ExerciseDescription = exerciseDescription;
            ExerciseType = exerciseType;
            TargetMuscle = targetMuscle;
            RestDuration = restDuration;

            Dictionary<MuscleGroup, double> contributions;

            if (ExerciseSecondaryMuscleDefinitions.SecondaryMuscleGroups.TryGetValue(
                exerciseName, out contributions))
            {
                SecondaryMuscleGroups =
                    new Dictionary<MuscleGroup, double>(contributions);
            }
            else
            {
                SecondaryMuscleGroups = new Dictionary<MuscleGroup, double>();
            }
        }


        public void MarkCompleted()
        {
            Completed = true;
        }

        public bool isCompound()
        {
            return SecondaryMuscleGroups.Count >= 2;
        }

        public virtual float GetEstimatedDuration()
        {
            return 0;
        }
    }
}
