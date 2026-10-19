using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Services
{
    public class WorkoutTypeSelection
    {
        public void RunAlgorithm()
        {
            foreach (WorkoutType workoutType in Enum.GetValues(typeof(WorkoutType))
            {
                int WorkoutTypeIndex = (int)workoutType;
                int[] Scores = new int[Enum.GetValues(typeof(WorkoutType)).Length];
            }
        }
    }
}
