using CivANewDawnAssistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivANewDawnAssistViewModels
{
    public class CivSelectionViewModel
    {
        public CivANewDawnGameMaster GameMaster { get; }
        public List<Player> Players => GameMaster.Players;
        public CivSelectionViewModel(List<Player> players)
        {
            GameMaster = new CivANewDawnGameMaster(players);
        }

        public void PlayerChoosesSelection1(int playerIdx)
        {
            GameMaster.Players[playerIdx].ChoosenCiv = GameMaster.Players[playerIdx].Civ1;
        }

        public void PlayerChoosesSelection2(int playerIdx)
        {
            GameMaster.Players[playerIdx].ChoosenCiv = GameMaster.Players[playerIdx].Civ2;
        }
    }
}
