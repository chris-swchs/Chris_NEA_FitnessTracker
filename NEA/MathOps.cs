using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    public static class MathOps
    {
        // Pure function: accepts any number of integers and returns their sum
        public static double GetSum(params double[] numbers) => numbers.Sum();
    }
}
