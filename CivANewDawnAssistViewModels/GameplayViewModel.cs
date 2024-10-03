using CivANewDawnAssistModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivANewDawnAssistViewModels
{
    public class GameplayViewModel
    {
        private CivANewDawnGameMaster _gameMaster;
        public string CultureWonder => _gameMaster.GetCulWonder();
        public string EconomyWonder => _gameMaster.GetEconWonder();
        public string MilitaryWonder => _gameMaster.GetMilWonder();
        public string ScienceWonder => _gameMaster.GetSciWonder();
        public string CurrentTurn => _gameMaster.GetCurrentTurn();
        public string NextTurn => _gameMaster.GetNextTurn();
        public bool AreMapTilesEmpty => _gameMaster.AreMapTilesEmpty();
        public bool IsAstronomyPossible => _gameMaster.IsAstronomyPossible();
        public List<EventCard> NextEventCards => _gameMaster.NextEventCards;
        public EventCardEra CurrentEra => _gameMaster.CurrentEra;
        public bool EndOfEra => _gameMaster.EndOfEra;
        public GameplayViewModel(CivANewDawnGameMaster gameMaster)
        {
            _gameMaster = gameMaster;
        }

        public void PurchaseCultureWonder()
        {
            _gameMaster.PurchaseCultureWonder();
        }

        public void PurchaseEconomyWonder()
        {
            _gameMaster.PurchaseEconomyWonder();
        }

        public void PurchaseMilitaryWonder()
        {
            _gameMaster.PurchaseMilitaryWonder();
        }

        public void PurchaseScienceWonder()
        {
            _gameMaster.PurchaseScienceWonder();
        }

        public MapTile GetNextMapTile()
        {
            return _gameMaster.GetNextMapTile();
        }

        public void ReturnMapTile(int mapTileNum)
        {
            _gameMaster.ReturnMapTile(mapTileNum);
        }

        //private void GetPrevEventCards()
        //{
        //    if(_eventNumber > 1)
        //        _eventNumber--;

        //    if (_eventNumber >= _eventCards.Count)
        //        _eventNumber = _eventCards.Count;

        //    NextEventCards = new List<EventCard>();
        //    for(int i = _eventNumber - 1; i < _players.Count + _eventNumber - 1; i++)
        //    {
        //        if (i < 0)
        //            break;

        //        if (i >= _eventCards.Count)
        //            break;

        //        NextEventCards.Add(_eventCards[i]);
        //    }
        //}

        //public string GetPrevTurn()
        //{
        //    if (_turnNumber > 1)
        //        _turnNumber--;

        //    var currentTurn = TurnOrder[_turnNumber - 1];

        //    if (currentTurn.TurnType == TurnType.PLAYER)
        //    {
        //        GetPrevEventCards();
        //        return _players[currentTurn.Idx].Name;
        //    }
        //    else
        //    {
        //        return _eventCards[currentTurn.Idx].Name + "\n" + _eventCards[currentTurn.Idx].Event;
        //    }
        //}
    }
}
