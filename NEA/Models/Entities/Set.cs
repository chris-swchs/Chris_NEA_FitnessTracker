using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Models.Entities
{
    public class Set
    {
        public float Weight { get; set; }
        public int Reps { get; set; }
        public float RestDuration { get; set; }
        public bool Completed { get; private set; }
        public Set(float weight, int reps, float restDuration)
        {
            Weight = weight;
            Reps = reps;
            RestDuration = restDuration;
        }

        public void MarkCompleted()
        {
            Completed = true;
        }
    }
}
