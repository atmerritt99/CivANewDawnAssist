using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivANewDawnAssistModels
{
    public class Player
    {
        public string Name { get; }
        public PlayerColor PlayerColor { get; }
        public Civilization ChoosenCiv { get; set; }
        public Civilization Civ1 { get; set; }
        public Civilization Civ2 { get; set; }
        public MapTile StartingTile { get; set; }

        public Player(string name, PlayerColor playerColor)
        {
            Name = name;
            PlayerColor = playerColor;
        }

        public override string ToString()
        {
            return $"{Name} - {ChoosenCiv}";
        }
    }
}
