using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheoreticalProbabilitiesNS;

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
                m_nodes[i] = new Node("node" + i, i);    //передается и номер узла для понимания перемещений в графе

            for (int i = 0; i < m_nodes.Length; i++)
            {
                List<NodeIntensityObject> children = new List<NodeIntensityObject>();


                if(i == m_nodes.Length-1)
                    children.Add(new NodeIntensityObject(null, m_λ));

                else
                    children.Add(new NodeIntensityObject(m_nodes[i + 1], m_λ));



                if (i - 1 >= 0)
                    children.Add(new NodeIntensityObject(m_nodes[i - 1], m_μ));

                m_nodes[i].SetChildren(children.ToArray());
            }


            SetStartNode(m_nodes[0]);
        }


        internal void outputParams(DataGridView dataGridView, DataGridView probabilitiesDataGrid, TextBox logTextBox)
        {
            CalsStatistics(dataGridView);

            OutPutProbabilities(probabilitiesDataGrid);

            FillLog(logTextBox);
        }

        private void FillLog(TextBox logTextBox)
        {
            logTextBox.Text = "";

            logTextBox.Text += "Кількість відмов: " + FailuresCounter+Environment.NewLine;
            logTextBox.Text += "Кількість опрацьованих вимог: " + ProcessedCounter;
        }

        private void OutPutProbabilities(DataGridView probabilitiesDataGrid)
        {
            probabilitiesDataGrid.Rows.Clear();

            int lastProbabilityIndex = Probabilities[0].Count-1;

            for (int i = 0; i < m_nodes.Length; i++)
                probabilitiesDataGrid.Rows.Add("p" + i, 
                    probabilitiesTheoretical[i].ToString("0.0000"), 
                    Probabilities[i][lastProbabilityIndex].ToString("0.0000"));
        }

        private void CalsStatistics(DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();

            double pFailure, q, A, r, w, k, t_waiting, t_processing, t_AverageInSystem;
            GetTheoreticalStatistics(out pFailure, out q, out A, out r, out w, out k, out t_waiting, out t_processing, out t_AverageInSystem);

            double pFailure_stat, q_stat, A_stat, r_stat, w_stat, k_stat, t_waiting_stat, t_processing_stat, t_AverageInSystem_stat;
            GetStatisticaltatistics(out pFailure_stat, out q_stat, out A_stat, out r_stat, out w_stat, out k_stat, out t_waiting_stat, out t_processing_stat, out t_AverageInSystem_stat);

            dataGridView.Rows.Add("Ймовірність відмови обслуговування", pFailure.ToString("0.0000"), pFailure_stat.ToString("0.0000"));
            dataGridView.Rows.Add("Відносна пропускна властивість", q.ToString("0.0000"), q_stat.ToString("0.0000"));
            dataGridView.Rows.Add("Абсолютна пропускна властивість", A.ToString("0.0000"), A_stat.ToString("0.0000"));
            dataGridView.Rows.Add("Середня кількість вимог, що знаходиться в черзі", r.ToString("0.0000"), r_stat.ToString("0.0000"));
            dataGridView.Rows.Add("Середня кількість вимог, що обслуговуються системою", w.ToString("0.0000"), w_stat.ToString("0.0000"));
            dataGridView.Rows.Add("Середня кількість вимог, що знаходиться в системі ", k.ToString("0.0000"), k_stat.ToString("0.0000"));
            dataGridView.Rows.Add("Середній час очікування в черзі", t_waiting.ToString("0.0000"), t_waiting_stat.ToString("0.0000"));
            dataGridView.Rows.Add("Середній час обслуговування однієї вимоги", t_processing.ToString("0.0000"), t_processing_stat.ToString("0.0000"));
            dataGridView.Rows.Add("Середній час перебування вимоги в СМО", t_AverageInSystem.ToString("0.0000"), t_AverageInSystem_stat.ToString("0.0000"));
        }

        double[] probabilitiesTheoretical;

        private void GetTheoreticalStatistics(out double pFailure, out double q, out double A, out double r, out double w, out double k, out double t_waiting, out double t_processing, out double t_AverageInSystem)
        {
            TheoreticalProbabilities statistics = new TheoreticalProbabilities();

            statistics.Calc(m_λ, m_μ, m_m, 1);
            probabilitiesTheoretical = statistics.P_c;

            //вероятность отказа
            pFailure = statistics.P_c[1 + m_m];

            //относительная пропускная возможность
            q = statistics.m_q;

            //абсолютная пропусная возможность
            A = m_λ * q;

            //среднее количество в очереди
            r = statistics.m_r;

            //среднее количество требований, которые обслуживаются системой
            w = statistics.m_z; 

            //среднее количество требований в системе
            k = statistics.m_k;

            //среднее время ожидания в очереди
            t_waiting = statistics.m_t_waiting;

            //среднее время обслуживая одного требования
            t_processing = 1 / m_μ;

            //среднее время пребывания требования в СМО
            t_AverageInSystem = statistics.m_t_system;
        }

        private void GetStatisticaltatistics(out double pFailure_stat, out double q_stat, out double a_stat, out double r_stat, out double w_stat, out double k_stat, out double t_waiting_stat, out double t_processing_stat, out double t_AverageInSystem_stat)
        {
            int lastProbabilityIndex = Probabilities[0].Count-1;

            List<double> lastNodeProbabilities = Probabilities[m_nodes.Length - 1];


            pFailure_stat = lastNodeProbabilities[lastProbabilityIndex];
            a_stat  = ProcessedCounter / base.m_time;
            q_stat = a_stat/m_λ;

            r_stat =0;

            for (int i = 1; i <= m_m; i++)
                r_stat += i * Probabilities[i+1][lastProbabilityIndex];


            w_stat =1 - Probabilities[0][lastProbabilityIndex];

            k_stat =0;

            for (int i = 1; i <= m_m+1; i++)
                k_stat += i * Probabilities[i][lastProbabilityIndex];


            t_waiting_stat = m_processedQueries
                                .Select(el=>el.StartProcessingTime - el.IncomeTime)
                                .Average();
            
            t_processing_stat = m_processedQueries
                                .Select(el => el.EndProcessingTime - el.StartProcessingTime)
                                .Average();

            t_AverageInSystem_stat = m_processedQueries
                                .Select(el => el.EndProcessingTime - el.IncomeTime)
                                .Average(); ;
        }
    }
}
