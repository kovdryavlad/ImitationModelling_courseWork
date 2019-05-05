using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseWork.SimulationModeling
{
    class NodeIntensityObject
    {
        Node m_node;
        double m_intensity;

        public Node Node => m_node;
        public double Intensity => m_intensity;

        double m_interval;
        public double Interval => m_interval;

        public NodeIntensityObject(Node node, double intensity)
        {
            m_node = node;
            m_intensity = intensity;
        }

        public double generateInterval(double gama) => m_interval = Math.Log(1d / (1 - gama)) / m_intensity;

        public override string ToString() => String.Format("{0}|intensity: {1:0.00}| interval: {2:0.00}", m_node, m_intensity, m_interval);
    }
}
