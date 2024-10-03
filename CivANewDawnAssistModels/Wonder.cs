using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivANewDawnAssistModels
{
    public class Wonder
    {
        public string Name { get; }
        public string Ability { get; }
        public WonderType WonderType { get; }
        public WonderEra WonderEra { get; }
        public int Cost { get; }
        public Resource Resource1 { get; }
        public Resource Resource2 { get; }
        public Wonder(string name, string ability, WonderType wonderType, WonderEra wonderEra, int cost, Resource resource1, Resource resource2)
        {
            Name = name;
            Ability = ability;
            WonderType = wonderType;
            WonderEra = wonderEra;
            Cost = cost;
            Resource1 = resource1;
            Resource2 = resource2;
        }

        public override string ToString()
        {
            return $"{Name}\n{WonderEra}\n{Ability}\nCost: {Cost} Resources: {Resource1} {Resource2}";
        }
    }
}
