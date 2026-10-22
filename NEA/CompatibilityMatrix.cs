using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    // Compatibility Matrix - defines the compatibility of 2 different attriute types
    public class CompatibilityMatrix<T1, T2>
    {
        private readonly Dictionary<T1, Dictionary<T2, double>> values = new Dictionary<T1, Dictionary<T2, double>>();

        public void Add(T1 key1, T2 key2, double compatibility)
        {
            if (compatibility < 0 || compatibility > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(compatibility), "Compatibility value must be between 0 and 1");
            }
            if (!values.ContainsKey(key1))
            {
                values[key1] = new Dictionary<T2, double>();
            }
            values[key1].Add(key2, compatibility);
        }

        public void AddRange(T1 key1, Dictionary<T2, double> compatibilities) 
        {
            foreach (var entry in compatibilities)
            {
                Add(key1, entry.Key, entry.Value);
            }
        }

        public double Get(T1 key1, T2 key2)
        {
            return values[key1][key2];
        }
    }
}
