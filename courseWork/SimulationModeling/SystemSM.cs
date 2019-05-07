using System;
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

        int m_failuresCounter;
        public int FailuresCounter => m_failuresCounter; //счетчик отказов

        int m_processedCounter;
        public int ProcessedCounter=>m_processedCounter;

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

        protected Query m_currentQuery;
        protected List<Query> m_processedQueries = new List<Query>();
        protected List<Query> m_Queue = new List<Query>();

        //первое т всегда равно 0
        public List<double> timeList = new List<double>();

        protected Dictionary<double, double> failuredDict = new Dictionary<double, double>();
        protected Dictionary<double, double> processedDict = new Dictionary<double, double>();

        public virtual void Model(double tLimit)
        {
            System.Diagnostics.Debug.WriteLine(m_currentNode);

            while (m_time < tLimit)
                TransferToNextNode();

            
            System.Diagnostics.Debug.WriteLine("Failures: " + m_failuresCounter);
            System.Diagnostics.Debug.WriteLine("Processed: " + m_processedCounter);
        }

        protected double m_time;

        private void TransferToNextNode()
        {
            NodeIntensityObject[] children = m_currentNode.Children;

            foreach (NodeIntensityObject child in children)
                child.generateInterval(r.NextDouble());

            var orderedChildren = children.OrderBy(ch => ch.Interval).ToArray();

            NodeIntensityObject nextObject = null;
            if (orderedChildren[0].Node == null)    //отказ
            {
                nextObject = orderedChildren[1];
                m_failuresCounter++;

                failuredDict.Add(m_time, m_failuresCounter);
            }
            else
                nextObject = orderedChildren[0];    //переход

            double interval = nextObject.Interval;
            m_currentNode.AddTime(interval);

            //System.Diagnostics.Debug.WriteLine("time: " + m_currentNode.Time);

            m_time += interval;
            timeList.Add(m_time);

            //переходим к следующему элементу
            SetNextNode(nextObject.Node, interval);

            //подсчет вероятностей
            CalcStatistics(m_time);
        }

        private void CalcStatistics(double t)
        {
            foreach (Node node in m_nodes)
                node.AddProbability(t);
        }

        void SetNextNode(Node nextNode, double interval)
        {
            //куда произошло перемещение
            if (nextNode.Number > m_currentNode.Number)
                MoveRight(nextNode, interval);
            else
                MoveLeft(nextNode, interval);


            //счетчик обработанных
            if (nextNode.Number < m_currentNode.Number)
            {
                m_processedCounter++;

                processedDict.Add(m_time, m_processedCounter);
            }

            nextNode.Transfer();
            m_currentNode = nextNode;
            //System.Diagnostics.Debug.WriteLine(m_currentNode);
        }

        private void MoveLeft(Node nextNode, double interval)
        {
            m_currentQuery.EndProcessingTime = m_time + interval;
            m_processedQueries.Add(m_currentQuery);

            if (m_currentNode.Number == 1)
                m_currentQuery = null;

            else
            {
                //вытягиваем первый из очереди
                m_currentQuery = m_Queue[0];
                m_Queue.RemoveAt(0);

                m_currentQuery.StartProcessingTime = m_time + interval;
            }
            
        }

        private void MoveRight(Node nextNode, double interval)
        {
            if (m_currentNode.Number == 0)
            {
                m_currentQuery = new Query(m_time + interval);
                m_currentQuery.StartProcessingTime = m_currentQuery.IncomeTime;
            }
            else
                m_Queue.Add(new Query(m_time + interval));
        }

        public List<List<double>> Probabilities => m_nodes.Select(n=>n.Probabilities).ToList();
    }
}
