using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseWork.SimulationModeling
{
    class Query
    {
        public double IncomeTime;
        public double StartProcessingTime;
        public double EndProcessingTime;

        public Query(double incomeTime) => IncomeTime = incomeTime;
    }
}
