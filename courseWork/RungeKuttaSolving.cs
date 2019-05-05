namespace courseWork
{
    class RungeKuttaSolving
    {
        //source: https://www.nsc.liu.se/~boein/f77to90/rk.html

        double f(double x, double y, double z) => -0.02 * y + 0.1 * z;
        double g(double x, double y, double z) => -0.18 * y - 0.31 * z + 0.2;

        double m_h;
        protected int m_N;
        protected double[] m_x; 
        protected double[] m_y;
        protected double[] m_z;

        public double[] X => m_x;
        public double[] Y => m_y;
        public double[] Z => m_z;

        void setStep(double a, double b, int N) => m_h = (b - a) / N;

        void Init(int N)
        {
            m_N = N;

            m_x = new double[N];
            m_y = new double[N];
            m_z = new double[N];
        }

        public RungeKuttaSolving(double a, double b, int N)
        {
            Init(N);
            setStep(a, b, N);
            
            m_x[0] = a;     //remember the initial value
        }

        public void setInitialValues(double[] initValues)
        {
            m_y[0] = initValues[0];
            m_z[0] = initValues[1];
        }

        public virtual void Solve()
        {
            for (int n = 0; n < m_N-1; n++)
            {
                double k1 = m_h * f(m_x[n], m_y[n], m_z[n]);
                double l1 = m_h * g(m_x[n], m_y[n], m_z[n]);

                double k2 = m_h * f(m_x[n] + m_h / 2, m_y[n] + k1 / 2, m_z[n] + l1 / 2);
                double l2 = m_h * g(m_x[n] + m_h / 2, m_y[n] + k1 / 2, m_z[n] + l1 / 2);
                
                double k3 = m_h * f(m_x[n] + m_h / 2, m_y[n] + k2 / 2, m_z[n] + l2 / 2);
                double l3 = m_h * g(m_x[n] + m_h / 2, m_y[n] + k2 / 2, m_z[n] + l2 / 2);
                
                double k4 = m_h * f(m_x[n] + m_h, m_y[n] + k3, m_z[n] + l3);
                double l4 = m_h * g(m_x[n] + m_h, m_y[n] + k3, m_z[n] + l3);
                
                double k = 1d / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
                double l = 1d / 6 * (l1 + 2 * l2 + 2 * l3 + l4);

                m_x[n + 1] = m_x[n] + m_h;
                m_y[n + 1] = m_y[n] + k;
                m_z[n + 1] = m_z[n] + l;
            }
        }
    }


}
