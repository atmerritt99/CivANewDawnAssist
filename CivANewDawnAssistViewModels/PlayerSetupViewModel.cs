using CivANewDawnAssistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivANewDawnAssistViewModels
{
    public class PlayerSetupViewModel
    {
        public PlayerSetupViewModel()
        {

        }

        public List<Player> CreatePlayers(List<string> playerNames, List<int> playerColors)
        {
            var players = new List<Player>();

            for(int i = 0; i < playerNames.Count; i++)
            {
                players.Add(new Player(playerNames[i], (PlayerColor)playerColors[i]));
            }

            return players;
        }
    }
}
