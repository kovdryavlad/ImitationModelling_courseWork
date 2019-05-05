using System.Collections.Generic;

namespace courseWork.SimulationModeling
{
    class Node
    {
        double m_time;
        int m_transitions;
        string m_name;

        public Node()
        {
            //начальная вероятнсть
            Probabilities.Add(0);
        }

        public Node(string name)
            :this()
        {
            m_name = name;
        }

        NodeIntensityObject[] m_childrenNodes;

        public NodeIntensityObject[] Children => m_childrenNodes;

        public void SetChildren(NodeIntensityObject[] childrenNodes) => m_childrenNodes = childrenNodes;

        public double Time => m_time;
        public int Transition => m_transitions;

        public double AverageTimeInNode => m_time / m_transitions;

        public void AddTime(double addedTime) => m_time += addedTime;

        //переход в него
        public void Transfer() => m_transitions++;

        public override string ToString() => m_name;

        //для работы с вероятностями - считаються при переходах
        public List<double> Probabilities = new List<double>();
        public void AddProbability(double allTime) => Probabilities.Add(Time / allTime);

        public void ClearProbabilites() {
            //Probabilities.Clear(); -- Спросить Тараса или Димы!

            //для начальных значений
            Probabilities = new List<double>(1);

        }

        public void SetStartProbability(double p)=> Probabilities[0] = p;
        /*
        public void SetStartProbability(double p){
            //посмотреть на свежую голову
            //Probabilities.Count == 0 ?  Probabilities.Add(p) : Probabilities[0] = p;
            //Probabilities.Count == 0 ? 1 : 2;

            if (Probabilities.Count == 0)
                Probabilities.Add(p);
            else
                Probabilities[0] = p;

        } */
    }
}
