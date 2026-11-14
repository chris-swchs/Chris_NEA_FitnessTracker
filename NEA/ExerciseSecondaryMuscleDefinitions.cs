using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NEA
{
    public static class ExerciseSecondaryMuscleDefinitions
    {
        public static readonly Dictionary<string, Dictionary<MuscleGroup, double>>
            SecondaryMuscleGroups = new Dictionary<string, Dictionary<MuscleGroup, double>>
            {
                {
                "Bench Press",
                new Dictionary<MuscleGroup, double>
                {
                    { MuscleGroup.FrontShoulders, 0.8},
                    { MuscleGroup.Triceps, 0.2}
                }
            },
        };
    }
}