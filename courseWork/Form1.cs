using courseWork.SimulationModeling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace courseWork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].CursorY.Interval = 0.005D;


            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0.0}";
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0.00}";

            chart1.ChartAreas[0].AxisY.Maximum = 1;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
        }

        double m_lyabmbda;
        double m_mu;
        int m_maxTime;
        int m_queueLength;

        private void button1_Click(object sender, EventArgs e)
        {
            bool status = Init();

            if (status)
            {
                //имитационное моделирование
                SpecificSystem specificSystem = new SpecificSystem();
                specificSystem.SetProcessingParams(m_lyabmbda, m_mu, m_queueLength);                
                specificSystem.Model(m_maxTime);
                List<List<double>> imitationProbabilities = specificSystem.Probabilities;

                chart1.Series.Clear();

                for (int i = 0; i < imitationProbabilities.Count; i++)
                {
                    Series s = new Series();
                    s.ChartType = SeriesChartType.Line;
                    s.Name = "p" + i;
                    s.Points.DataBindXY(specificSystem.timeList, imitationProbabilities[i]);
                    chart1.Series.Add(s);
                }

                specificSystem.outputParams(dataGridView1, ProbabilitieDataGridView, LogTextBox);

            }
        }

        private bool Init()
        {
            bool status = true;
            try
            {
                m_lyabmbda = Convert.ToDouble(lyambdaTextBox.Text.Replace(".", ","));
                m_mu = Convert.ToDouble(mutextBox.Text.Replace(".", ","));
                m_maxTime = Convert.ToInt32(maxTimeTextBox.Text.Replace(".", ","));
                m_queueLength = Convert.ToInt32(queueLengthtextBox.Text.Replace(".", ","));
            }
            catch 
            {
                MessageBox.Show("Помилка в параметрах");
                status = false;
            }

            return status;
        }
    }
}
