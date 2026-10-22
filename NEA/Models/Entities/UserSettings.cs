using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Models.Entities
{
    public class UserSettings
    {
        public Goal PrimaryGoal { get; set; }
        public Goal SecondaryGoal { get; set; }
        public int Age { get; set; }
        public ExperienceLevel ExperienceLevel { get; set; }
        public int PreferredSessionDuration { get; set; }
        public int PreferredTrainingDays { get; set; }
        public float MotivationDisciplineInterpolation { get; set; }
        public Frequency PreferredTrainingIntensity { get; set; }
        public float CompoundIsolationInterpolation { get; set; }
        public Difficulty PreferredWorkoutDifficulty { get; set; }
        public Frequency CardioPreference { get; set; }
        public MainMuscleGroup MainMuscleGroupPriority { get; set; }
    }
}
