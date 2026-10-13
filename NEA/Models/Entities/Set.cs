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
        private bool Completed { get; set; } = false;
        public Set(float weight, int reps)
        {
            Weight = weight;
            Reps = reps;
        }

        public void MarkCompleted()
        {
            Completed = true;
        }
    }
}
