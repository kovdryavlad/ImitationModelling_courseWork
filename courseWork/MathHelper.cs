using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseWork
{
    class MathHelper
    {
        public static double Factorial(int v)
        {
            double res = 1;
            for (int i = 2; i <= v; i++)
                res *= i;

            return res;
        }
    }
}
