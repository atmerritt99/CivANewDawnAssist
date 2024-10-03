using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivANewDawnAssistModels
{
    public class VictoryCard
    {
        private string _condition1;
        private string _condition2;

        public VictoryCard(string condition1, string condition2)
        {
            _condition1 = condition1;
            _condition2 = condition2;
        }

        public override string ToString()
        {
            return $"{_condition1}\n{_condition2}";
        }
    }
}
