using System;

namespace NEA.Services
{
    public class WorkoutTypeSelection
    {
        public void RunAlgorithm()
        {
            // for loop temporary - is meant to loop through each available workout slot
            for (int i = 0; i < 10; i++)
            {
                double[] Scores = new double[Enum.GetValues(typeof(WorkoutType)).Length];
                int WorkoutTypeIndex = 0;
                foreach (WorkoutType workoutType in Enum.GetValues(typeof(WorkoutType)))
                {
                     WorkoutTypeIndex = (int)workoutType;

                    double score = CalculateScore();
                    Scores[WorkoutTypeIndex] = score;
                    WorkoutTypeIndex++;


                }
            }
        }

        private double CalculateScore()
        {
            return 5.0;
        }
    }
}
