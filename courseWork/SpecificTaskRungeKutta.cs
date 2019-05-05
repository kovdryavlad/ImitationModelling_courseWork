using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseWork
{
    class SpecificTaskRungeKutta : RungeKuttaSolving
    {
        public SpecificTaskRungeKutta(double a, double b, int N) :
            base(a, b, N)
        {
            base.setInitialValues(new[] { 1d, 0 });
        }

        public double[] P0 => m_y;

        public double[] P1 => m_z;

        public double[] P2 => m_P2;

        double[] m_P2;

        public override void Solve()
        {
            base.Solve();

            CalcP2();
        }

        private void CalcP2()
        {
            m_P2 = new double[m_N];

            for (int i = 0; i < m_N; i++)
                m_P2[i] = 1 - m_y[i] - m_z[i];
        }
    }
}
