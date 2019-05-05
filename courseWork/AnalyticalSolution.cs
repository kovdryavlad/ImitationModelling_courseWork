using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseWork
{
    class AnalyticalSolution
    {
        //RESULT
        public double[] P0;
        public double[] P1;
        public double[] P2;

        double λ = 0.01;
        double μ = 0.1;

        double m1;  //корни характреристического уравнения
        double m2;  //корни характреристического уравнения

        double c1;  //константы для дифура
        double c2;  //константы для дифура

        double A;   //частное решение

        public AnalyticalSolution()
        {
            m1 = -2 * (μ + λ);
            m2 = -1 * (μ + λ);

            A = (μ * μ) / Math.Pow((μ + λ), 2);

            c2 = (m1 + 2 * λ) * (1 - A) + 2 * λ * A;
            c2 /= -(m2 - m1);

            c1 = 1 - c2 - A;
        }

        double p0(double t) => c1 * Math.Exp(m1 * t) + c2 * Math.Exp(m2 * t) + A;

        double dp0_dt(double t) => m1 * c1 * Math.Exp(m1 * t) + m2 * c2 * Math.Exp(m2 * t);

        double p1(double t) => (dp0_dt(t) + 2 * λ * p0(t)) / μ;

        double p2(double t, double p0, double p1) => 1 - p0 - p1;

        public void CalcSolution(double[] tArr)
        {
            Init(tArr.Length);

            for (int i = 0; i < tArr.Length; i++)
            {
                double t = tArr[i];

                P0[i] = p0(t);
                P1[i] = p1(t);
                P2[i] = p2(t, P0[i], P1[i]);
            }
        }

        private void Init(int N)
        {
            P0 = new double[N];
            P1 = new double[N];
            P2 = new double[N];
        }
    }
}
