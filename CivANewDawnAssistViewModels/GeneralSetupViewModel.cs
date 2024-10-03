using CivANewDawnAssistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivANewDawnAssistViewModels
{
    public class GeneralSetupViewModel
    {
        public CivANewDawnGameMaster GameMaster { get; }
        public List<Player> Players => GameMaster.Players;
        public List<MapTile> BaseTiles => GameMaster.MapTilesInPlay;
        public List<VictoryCard> VictoryCards => GameMaster.VictoryCards;
        public GeneralSetupViewModel(CivANewDawnGameMaster gameMaster)
        {
            GameMaster = gameMaster;
        }
    }
}
