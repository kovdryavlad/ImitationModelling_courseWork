using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace courseWork.SimulationModeling
{
    class SpecificSystem: SystemSM
    {
        double m_λ;
        double m_μ;
        int m_m;

        public void SetProcessingParams(double lyambda, double mu, int m)
        {
            m_λ = lyambda;
            m_μ = mu;
            m_m = m;
        }

        public override void Model(double tLimit)
        {
            Init();
            base.Model(tLimit);
        }

        private void Init()
        {
            base.m_nodes = new Node[m_m + 2];

            for (int i = 0; i < m_nodes.Length; i++)
                m_nodes[i] = new Node("node" + i);

            for (int i = 0; i < m_nodes.Length; i++)
            {
                List<NodeIntensityObject> children = new List<NodeIntensityObject>();

                if (i + 1 <= m_nodes.Length - 1)
                    children.Add(new NodeIntensityObject(m_nodes[i + 1], m_λ));

                if (i - 1 >= 0)
                    children.Add(new NodeIntensityObject(m_nodes[i - 1], m_μ));

                m_nodes[i].SetChildren(children.ToArray());
            }


            SetStartNode(m_nodes[0]);
        }


        internal void outputParams(DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            
            // приведенная интенсивность
            double ro = m_λ / m_μ;
            
            //вероятность отказа
            double pFailure = (Math.Pow(ro, m_m + 1) * (1 - ro)) / (1 - Math.Pow(ro, m_m + 2));

            //относительная пропускная возможность
            double q = 1 - pFailure;

            //абсолютная пропусная возможность
            double A = m_λ * q;

            //среднее количество требований в системе
            double r = (ro * ro * (1 - Math.Pow(ro, m_m)) * (m_m + 1 - m_m * ro)) / ((1 - Math.Pow(ro, m_m + 2)) * (1 - ro));

            //среднее количество требований, которые обслуживаются системой
            double w = (ro + Math.Pow(ro, m_m + 2)) / (1 - Math.Pow(ro, m_m + 2));

            //среднее количество требований в системе
            double k = w + r;

            //среднее время ожидания в очереди
            double t_waiting = r / m_λ;

            //среднее время обслуживая одного требования
            double t_processing = 1 / m_μ;

            //среднее время пребывания требования в СМО
            double t_AverageInSystem = r / m_λ + q / m_μ;

            dataGridView.Rows.Add("Ймовірність відмови обслуговування"                 , pFailure          .ToString("0.0000") );
            dataGridView.Rows.Add("Відносна пропускна властивість"                     , q                 .ToString("0.0000") );
            dataGridView.Rows.Add("Абсолютна пропускна властивість"                    , A                 .ToString("0.0000") );
            dataGridView.Rows.Add("Середня кількість вимог, що знаходиться в черзі"    , r                 .ToString("0.0000") );
            dataGridView.Rows.Add("Середня кількість вимог, що обслуговуються системою", w                 .ToString("0.0000") );
            dataGridView.Rows.Add("Середня кількість вимог, що знаходиться в системі " , k                 .ToString("0.0000") );
            dataGridView.Rows.Add("Середній час очікування в черзі"                    , t_waiting         .ToString("0.0000") );
            dataGridView.Rows.Add("Середній час обслуговування однієї вимоги"          , t_processing      .ToString("0.0000") );
            dataGridView.Rows.Add("Середній час перебування вимоги в СМО"              , t_AverageInSystem .ToString("0.0000") );
        }
    }
}
