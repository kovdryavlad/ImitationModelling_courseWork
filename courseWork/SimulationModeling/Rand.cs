using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseWork.SimulationModeling
{
    class Rand : Random
    {
        int i = 0;
        public double[] values = new[] { 0.4, 0.25, 0.7, 0.6, 0.8, 0.1 };

        public override double NextDouble() => values[i++];
    }
}
