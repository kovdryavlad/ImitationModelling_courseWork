﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseWork.SimulationModeling
{
    class SystemSM
    {
        public SystemSM()
        {
            //время всегда начинаетяс с 0
            timeList.Add(0);
        }

        protected Node[] m_nodes;

        public Node[] Nodes
        {
            set {
                m_nodes = value;

                foreach (var node in m_nodes)
                    node.ClearProbabilites();
            }
        }

        protected Node m_currentNode;

        Random r = new Random();

        double[] getAverageTimeInStates() => m_nodes.Select(node => node.AverageTimeInNode).ToArray();

        double Time => m_nodes.Sum(n => n.Time);

        public void SetStartNode(Node node)
        {
            node.Transfer();
            node.SetStartProbability(1);
            m_currentNode = node;
        }

        //первое т всегда равно 0
        public List<double> timeList = new List<double>();

        public virtual void Model(double tLimit)
        {
            System.Diagnostics.Debug.WriteLine(m_currentNode);

            double t = 0;
            while (t < tLimit)
                TransferToNextNode(ref t);
        }

        private void TransferToNextNode(ref double t)
        {
            NodeIntensityObject[] children = m_currentNode.Children;

            foreach (NodeIntensityObject child in children)
                child.generateInterval(r.NextDouble());

            var orderedChildren = children.OrderBy(ch => ch.Interval).ToArray();

            double minInterval = orderedChildren[0].Interval;
            m_currentNode.AddTime(minInterval);

            System.Diagnostics.Debug.WriteLine("time: " + m_currentNode.Time);

            t += minInterval;
            timeList.Add(t);

            //переходим к следующему элементу
            SetNextNode(orderedChildren[0].Node);

            //подсчет вероятностей
            CalcStatistics(t);
        }

        private void CalcStatistics(double t)
        {
            foreach (Node node in m_nodes)
                node.AddProbability(t);
        }

        void SetNextNode(Node nextNode)
        {
            nextNode.Transfer();
            m_currentNode = nextNode;
            System.Diagnostics.Debug.WriteLine(m_currentNode);
        }

        public List<List<double>> Probabilities => m_nodes.Select(n=>n.Probabilities).ToList();
    }
}
