using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivANewDawnAssistModels
{
    public class Turn
    {
        public TurnType TurnType { get; }
        public int Idx { get; }
        public Turn(TurnType turnType, int idx)
        {
            TurnType = turnType;
            Idx = idx;
        }
    }
}
