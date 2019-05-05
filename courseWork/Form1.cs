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
        }

        double m_a;
        double m_b;
        int m_n;

        private void button1_Click(object sender, EventArgs e)
        {
            bool status = Init();

            if (status)
            {
                //численное решение
                SpecificTaskRungeKutta rk = new SpecificTaskRungeKutta(m_a, m_b, m_n);
                rk.Solve();

                chart1.Series[3].Points.DataBindXY(rk.X, rk.P0);
                chart1.Series[4].Points.DataBindXY(rk.X, rk.P1);
                chart1.Series[5].Points.DataBindXY(rk.X, rk.P2);

                //аланалитическое решение
                AnalyticalSolution analyticalSolution = new AnalyticalSolution();
                analyticalSolution.CalcSolution(rk.X);

                chart1.Series[0].Points.DataBindXY(rk.X, analyticalSolution.P0);
                chart1.Series[1].Points.DataBindXY(rk.X, analyticalSolution.P1);
                chart1.Series[2].Points.DataBindXY(rk.X, analyticalSolution.P2);

                //имитационное моделирование
                SpecificSystem specificSystem = new SpecificSystem();
                specificSystem.Model(m_b);
                List<List<double>> imitationProbabilities = specificSystem.Probabilities;

                chart1.Series[6].Points.DataBindXY(specificSystem.timeList, imitationProbabilities[0]);
                chart1.Series[7].Points.DataBindXY(specificSystem.timeList, imitationProbabilities[1]);
                chart1.Series[8].Points.DataBindXY(specificSystem.timeList, imitationProbabilities[2]);
            }
        }

        private bool Init()
        {
            bool status = true;
            try
            {
                m_a = Convert.ToDouble(AtextBox.Text.Replace(".", ","));
                m_b = Convert.ToDouble(BtextBox.Text.Replace(".", ","));
                m_n = Convert.ToInt32(NtextBox.Text.Replace(".", ","));
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
