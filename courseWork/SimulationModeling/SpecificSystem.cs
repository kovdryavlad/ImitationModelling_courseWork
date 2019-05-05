using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseWork.SimulationModeling
{
    class SpecificSystem: SystemSM
    {
        public override void Model(double tLimit)
        {
            Init();
            base.Model(tLimit);
        }

        private void Init()
        {
            double λ = 0.01;
            double μ = 0.1;

            Node n0 = new Node("node0");
            Node n1 = new Node("node1");
            Node n2 = new Node("node2");

            n0.SetChildren(new[] { new NodeIntensityObject(n1, 2 * λ) });

            n1.SetChildren(new[] { new NodeIntensityObject(n0,  μ),
                                   new NodeIntensityObject(n2,  λ)});

            n2.SetChildren(new[] { new NodeIntensityObject(n1, 2 * μ) });

            m_nodes = new[] { n0, n1, n2 };

            SetStartNode(n0);
        }
    }
}
