using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivANewDawnAssistModels
{
    public class Civilization
    {
        public string Name { get; }
        public string LeaderName { get; }
        public string AbilityName { get; }
        public string Ability { get; }

        public Civilization(string name, string leaderName, string abilityName, string ability)
        {
            Name = name;
            LeaderName = leaderName;
            AbilityName = abilityName;
            Ability = ability;
        }

        public override string ToString()
        {
            return $"{Name} ({LeaderName}):\n{AbilityName}: {Ability}";
        }
    }
}
