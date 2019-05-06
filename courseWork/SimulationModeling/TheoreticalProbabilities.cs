using System;

namespace TheoreticalProbabilitiesNS
{
    class TheoreticalProbabilities
    {
        public double m_q;
        public double m_A;
        public double m_z;
        public double m_r;
        public double m_k;
        public double m_t_waiting;
        public double m_t_system;

        public double[] P_c;

        public void Calc(double lambda, double nu, int m, int n)
        {
            double ro = lambda / nu;
            P_c = new double[n + m + 1];

            for (int i = 0; i < n + 1; i++)
                P_c[0] += Math.Pow(ro, i) / Factorial(i);

            P_c[0] += Math.Pow(ro, n) / Factorial(n) * (ro / n - Math.Pow(ro / n, m + 1)) / (1.0 - ro / n);
            P_c[0] = 1.0 / P_c[0];

            for (int i = 1; i <= n; i++)
                P_c[i] = P_c[0] * Math.Pow(ro, i) / Factorial(i);

            for (int i = 1; i <= m; i++)
                P_c[i + n] = P_c[0] * Math.Pow(ro, i + n) / (Factorial(n) * Math.Pow(n, i));

            m_q = 1 - P_c[n + m];
            m_A = lambda * m_q;
            m_z = m_A / nu;
            m_r = (double)P_c[n + 1] * (1.0 - (m + 1) * Math.Pow(ro / n, m) + m * Math.Pow(ro / n, m + 1)) / Math.Pow(1.0 - Math.Pow(ro / n, 1), 2);
            m_k = m_z + m_r;
            m_t_waiting = m_r / lambda;
            m_t_system = m_t_waiting + m_q / nu;
        }

        private double Factorial(int v)
        {
            double res = 1;
            for (int i = 2; i < v; i++)
                i *= v;

            return res;
        }
    }
}