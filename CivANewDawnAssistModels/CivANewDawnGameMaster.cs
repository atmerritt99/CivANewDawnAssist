using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CivANewDawnAssistModels
{
    public class CivANewDawnGameMaster
    {
        private Random _rng;
        private List<EventCard> _allEventCards;
        private List<Wonder> _allWonders;
        private List<MapTile> _allMapTiles;
        private List<Civilization> _allCivilizations;
        private List<VictoryCard> _allVictoryCards;
        private int _culNumber;
        private int _econNumber;
        private int _milNumber;
        private int _sciNumber;
        private int _antiquityNumber;
        private int _culTradeTokens;
        private int _econTradeTokens;
        private int _milTradeTokens;
        private int _sciTradeTokens;
        private int _turnNumber;

        public List<Player> Players { get; }
        public List<Turn> TurnOrder { get; private set; }
        public List<EventCard> EventDeck { get; private set; }
        public List<EventCard> NextEventCards { get; private set; }
        public List<MapTile> MapTilesInPlay { get; private set; }
        public List<Wonder> CulWonders { get; private set; }
        public List<Wonder> EconWonders { get; private set; }
        public List<Wonder> MilWonders { get; private set; }
        public List<Wonder> SciWonders { get; private set; }
        public List<Wonder> AntiquityWonders { get; private set; }
        public List<VictoryCard> VictoryCards { get; private set; }
        public List<MapTile> MapTilesDeck { get; private set; }
        public char StartingSide { get; private set; }
        public EventCardEra CurrentEra { get; private set; }
        public bool EndOfEra { get; private set; }
        public CivANewDawnGameMaster(IEnumerable<Player> players)
        {
            _rng = new Random();
            EndOfEra = false;
            CurrentEra = EventCardEra.ANCIENT;

            Players = players.ToList();

            SetupCivSelection();
            SetupMapTiles();
            SetupWonders();
            SetupEventDeck();
            SetupVictoryCards();
        }

        private Civilization GetRandomCivilization(List<Civilization> civs)
        {
            int idx = _rng.Next(civs.Count);
            var civ = civs[idx];
            civs.RemoveAt(idx);
            return civ;
        }

        private Player GetCurrentPlayer()
        {
            var currentTurn = TurnOrder[_turnNumber];
            if (currentTurn.TurnType == TurnType.EVENT)
                return null;
            else
                return Players[currentTurn.Idx];
        }

        #region Setup

        private void SetupTurnOrder()
        {
            TurnOrder = new List<Turn>();
            _turnNumber = 0;

            for (int eventIdx = 0; eventIdx < EventDeck.Count; eventIdx++)
            {
                for(int playerIdx = 0; playerIdx < Players.Count; playerIdx++)
                {
                    TurnOrder.Add(new Turn(TurnType.PLAYER, playerIdx));
                }

                TurnOrder.Add(new Turn(TurnType.EVENT, eventIdx));
            }
        }

        private void SetupMapTiles()
        {
            _allMapTiles = _allMapTiles ?? GetMapTiles().ToList();

            var startingTiles = _allMapTiles.Where(x => x.IsStartingTile).ToList();
            MapTilesDeck = _allMapTiles.Where(x => !x.IsStartingTile).ToList();
            MapTilesInPlay = new List<MapTile>();

            foreach (var player in Players)
            {
                int idx = _rng.Next(startingTiles.Count);
                player.StartingTile = startingTiles[idx];
                startingTiles.RemoveAt(idx);
            }

            ShuffleMapTiles();

            int numOfBaseTiles = Players.Count < 4 ? 2 : 4;

            for (int i = 0; i < numOfBaseTiles; i++)
            {
                var mapTile = MapTilesDeck.First();
                MapTilesDeck.Remove(mapTile);
                MapTilesInPlay.Add(mapTile);
            }

            int coinFlip = _rng.Next(2);
            StartingSide = coinFlip == 0 ? 'A' : 'B';
        }

        private void SetupVictoryCards()
        {
            _allVictoryCards = _allVictoryCards ?? GetAllVictoryCards().ToList();
            VictoryCards = new List<VictoryCard>();

            VictoryCard[] vicCardArray = new VictoryCard[_allVictoryCards.Count];
            _allVictoryCards.CopyTo(vicCardArray);
            var vicCards = vicCardArray.ToList();

            for (int i = 0; i < 4; i++)
            {
                int idx = _rng.Next(vicCards.Count);
                var victoryCard = vicCards[idx];
                vicCards.RemoveAt(idx);
                VictoryCards.Add(victoryCard);
            }
        }

        private void SetupCivSelection()
        {
            _allCivilizations = _allCivilizations ?? GetCivilizations().ToList();

            Civilization[] civsArray = new Civilization[_allCivilizations.Count];
            _allCivilizations.CopyTo(civsArray);
            var civs = civsArray.ToList();

            foreach (var player in Players)
            {
                player.Civ1 = player.Civ1 ?? GetRandomCivilization(civs);
                player.Civ2 = player.Civ2 ?? GetRandomCivilization(civs);
                player.ChoosenCiv = player.Civ1;
            }
        }

        private void SetupEventDeck()
        {
            _allEventCards = _allEventCards ?? GetAllEventCards();
            EventDeck = new List<EventCard>();
            NextEventCards = new List<EventCard>();

            for (int i = (int)EventCardEra.ANCIENT; i <= (int)EventCardEra.FUTURE; i++)
            {
                if(((EventCardEra)i) == EventCardEra.CLASSICAL)
                    continue;

                var eventCards = _allEventCards.Where(x => x.Era == (EventCardEra)i).ToList();
                EventDeck.AddRange(CreateEraList(eventCards, (EventCardEra)i));
            }

            GetNextEventCards(0);
            SetupTurnOrder();
        }

        private void SetupWonders()
        {
            _culNumber = 0;
            _econNumber = 0;
            _milNumber = 0;
            _sciNumber = 0;

            _culTradeTokens = 0;
            _econTradeTokens = 0;
            _milTradeTokens = 0;
            _sciTradeTokens = 0;

            CulWonders = new List<Wonder>();
            EconWonders = new List<Wonder>();
            MilWonders = new List<Wonder>();
            SciWonders = new List<Wonder>();
            AntiquityWonders = new List<Wonder>();

            _allWonders = _allWonders ?? GetAllWonders().ToList();

            for (int wonderTypeNum = (int)WonderType.CULTURE; wonderTypeNum <= (int)WonderType.SCIENCE; wonderTypeNum++)
            {
                for (int wonderEraNum = (int)WonderEra.ANCIENT; wonderEraNum <= (int)WonderEra.MODERN; wonderEraNum++)
                {
                    var era = (WonderEra)wonderEraNum;
                    var type = (WonderType)wonderTypeNum;
                    var wonders = _allWonders.Where(x => x.WonderEra == era && x.WonderType == type).ToList();

                    ShuffleWonders(wonders);

                    if (type == WonderType.CULTURE)
                        CulWonders.AddRange(wonders);

                    if (type == WonderType.ECONOMY)
                        EconWonders.AddRange(wonders);

                    if (type == WonderType.MILITARY)
                        MilWonders.AddRange(wonders);

                    if (type == WonderType.SCIENCE)
                        SciWonders.AddRange(wonders);
                }
            }
        }

        #endregion

        #region Turn Management

        public string GetCurrentTurn()
        {
            var currentTurn = TurnOrder[_turnNumber];

            if (currentTurn.TurnType == TurnType.PLAYER)
            {
                return Players[currentTurn.Idx].ToString();
            }
            else
            {
                return EventDeck[currentTurn.Idx].ToString();
            }
        }

        public string GetNextTurn()
        {
            if (_turnNumber < TurnOrder.Count - 1)
                _turnNumber++;

            var nextTurn = TurnOrder[_turnNumber];
            EndOfEra = false;
            if (nextTurn.TurnType == TurnType.EVENT)
                ResolveEvent(nextTurn.Idx);

            return GetCurrentTurn();
        }

        private void GetNextEventCards(int nextEventCardIdx)
        {
            NextEventCards.Clear();
            for(int i = nextEventCardIdx; i < EventDeck.Count; i++)
            {
                NextEventCards.Add(EventDeck[i]);
                if (NextEventCards.Count >= nextEventCardIdx + 5)
                    break;
            }
        }

        private void ResolveEvent(int eventIdx)
        {
            var eventCard = EventDeck[eventIdx];

            CurrentEra = eventCard.Era;

            if(eventCard.Type == EventCardType.ERA_SCORE)
            {
                EndOfEra = true;
            }

            if (eventCard.Type == EventCardType.SELECT_A_PLAYER)
            {
                var x = _rng.Next(Players.Count);
                var randomPlayer = Players[x];
                eventCard.Event = eventCard.Event.Replace("Roll a die to select a Player", $"{randomPlayer.Name} ({randomPlayer.ChoosenCiv.Name})");
            }

            if (eventCard.Type == EventCardType.BARBARIANS_MOVE_1)
            {
                eventCard.Event = MoveBarbarians(1);
            }

            if (eventCard.Type == EventCardType.BARBARIANS_MOVE_2)
            {
                eventCard.Event = MoveBarbarians(2);
            }

            if (eventCard.Type == EventCardType.BARBARIANS_MOVE_3)
            {
                eventCard.Event = MoveBarbarians(3);
            }

            GetNextEventCards(eventIdx + 1);
        }

        private string MoveBarbarians(int movement)
        {
            StringBuilder result = new StringBuilder();
            result.Append($"Barbarians Move: ");
            for (int i = 0; i < movement; i++)
            {
                var x = _rng.Next(6) + 1;
                result.Append($"{x} ");
            }
            return result.ToString();
        }

        #endregion

        #region Wonder Management

        private string GetAntiquityWonder()
        {
            var x = _antiquityNumber;
            _antiquityNumber++;
            return AntiquityWonders[x].ToString();
        }

        public string GetCulWonder()
        {
            return _culNumber < CulWonders.Count ? CulWonders[_culNumber].ToString() + "\nTokens: " + _culTradeTokens.ToString() : string.Empty;
        }

        public string GetEconWonder()
        {
            return _econNumber < EconWonders.Count ? EconWonders[_econNumber].ToString() + "\nTokens: " + _econTradeTokens.ToString() : string.Empty;
        }

        public string GetMilWonder()
        {
            return _milNumber < MilWonders.Count ? MilWonders[_milNumber].ToString() + "\nTokens: " + _milTradeTokens.ToString() : string.Empty;
        }

        public string GetSciWonder()
        {
            return _sciNumber < SciWonders.Count ? SciWonders[_sciNumber].ToString() + "\nTokens: " + _sciTradeTokens.ToString() : string.Empty;
        }

        public void PurchaseCultureWonder()
        {
            if (_culNumber < CulWonders.Count)
            {
                AntiquityWonders.Add(CulWonders[_culNumber]);
                _culNumber++;
            }
            _culTradeTokens = 0;
        }

        public void PurchaseEconomyWonder()
        {
            if (_econNumber < EconWonders.Count)
            {
                AntiquityWonders.Add(EconWonders[_econNumber]);
                _econNumber++;
            }
            _econTradeTokens = 0;
        }

        public void PurchaseMilitaryWonder()
        {
            if (_milNumber < MilWonders.Count)
            {
                AntiquityWonders.Add(MilWonders[_milNumber]);
                _milNumber++;
            }
            _milTradeTokens = 0;
        }

        public void PurchaseScienceWonder()
        {
            if (_sciNumber < SciWonders.Count)
            {
                AntiquityWonders.Add(SciWonders[_culNumber]);
                _sciNumber++;
            }
            _sciTradeTokens = 0;
        }

        #endregion

        #region Map Tile Management

        public bool IsAstronomyPossible()
        {
            var player = GetCurrentPlayer();
            //TODO: Remove Magic Strings
            return player != null && player.ChoosenCiv.Name == "Poland" && !AreMapTilesEmpty();
        }

        public bool AreMapTilesEmpty()
        {
            return MapTilesDeck.Count <= 0;
        }

        public MapTile GetNextMapTile()
        {
            if (MapTilesDeck.Count <= 0)
                return null;

            var nextMapTile = MapTilesDeck.First();
            MapTilesDeck.Remove(nextMapTile);
            MapTilesInPlay.Add(nextMapTile);
            return nextMapTile;
        }

        public void ReturnMapTile(int mapTileNum)
        {
            var mapTile = MapTilesInPlay.Where(x => x.Num == mapTileNum).First();
            MapTilesDeck.Add(mapTile);
            MapTilesInPlay.Remove(mapTile);
        }

        #endregion

        #region Shuffle

        private IEnumerable<EventCard> CreateEraList(List<EventCard> eventCards, EventCardEra era)
        {
            var eventsInOrder = new List<EventCard>();

            if(era == EventCardEra.ANCIENT)
            {
                var nothingCard = eventCards.Where(x => x.Type == EventCardType.NOTHING).First();
                eventsInOrder.Add(nothingCard);
            }

            if((int)era < (int)EventCardEra.INDUSTRIAL)
            {
                var move1Barb = eventCards.Where(x => x.Type == EventCardType.BARBARIANS_MOVE_1).First();
                eventCards.Remove(move1Barb);
                eventsInOrder.Add(move1Barb);
            }
            else if((int)era < (int)EventCardEra.FUTURE)
            {
                var move2Barb = eventCards.Where(x => x.Type == EventCardType.BARBARIANS_MOVE_2).First();
                eventCards.Remove(move2Barb);
                eventsInOrder.Add(move2Barb);
            }
            else
            {
                var move3Barb = eventCards.Where(x => x.Type == EventCardType.BARBARIANS_MOVE_3).First();
                eventCards.Remove(move3Barb);
                eventsInOrder.Add(move3Barb);
            }

            var district = eventCards.Where(x => x.Type == EventCardType.DISTRICTS).First();
            eventsInOrder.Add(district);

            var gov = eventCards.Where(x => x.Type == EventCardType.GOVERNMENTS).First();
            eventsInOrder.Add(gov);

            if ((int)era < (int)EventCardEra.INDUSTRIAL)
            {
                var move1Barb = eventCards.Where(x => x.Type == EventCardType.BARBARIANS_MOVE_1).First();
                eventsInOrder.Add(move1Barb);
            }
            else if ((int)era < (int)EventCardEra.FUTURE)
            {
                var move2Barb = eventCards.Where(x => x.Type == EventCardType.BARBARIANS_MOVE_2).First();
                eventsInOrder.Add(move2Barb);
            }
            else
            {
                var move3Barb = eventCards.Where(x => x.Type == EventCardType.BARBARIANS_MOVE_3).First();
                eventsInOrder.Add(move3Barb);
            }

            eventsInOrder.Add(district);

            var eraScore = eventCards.Where(x => x.Type == EventCardType.ERA_SCORE).First();
            eventsInOrder.Add(eraScore);

            return eventsInOrder;
        }


        private void ShuffleErasEventCards(List<EventCard> eventCards, bool isFirstEra, int playerCount)
        {
            int finalIdx = eventCards.Count - 1;
            var finalCard = eventCards.Where(x => x.Type == EventCardType.ERA_SCORE).First();
            eventCards.Remove(finalCard);
            eventCards.Add(finalCard);

            for (int i = 0; i < 1000; i++)
            {
                int idx1 = _rng.Next(finalIdx);
                int idx2 = _rng.Next(finalIdx);
                var temp = eventCards[idx1];
                eventCards[idx1] = eventCards[idx2];
                eventCards[idx2] = temp;
            }

            //In first Era, Districts will always happen after every player takes a turn
            if (isFirstEra)
            {
                int idx1 = -1;
                for (int i = 0; i < playerCount; i++)
                {
                    if (eventCards[i].Type == EventCardType.DISTRICTS)
                    {
                        idx1 = i;
                        int idx2 = _rng.Next(finalIdx - playerCount) + playerCount;

                        //Prevents switching a Districts activate card for another Districts activate card
                        if (eventCards[idx2].Type == EventCardType.DISTRICTS)
                        {
                            //Assumes there is a max of 2 Districts Activate cards in the first era
                            idx2 = idx2 < finalIdx - 2 ? idx2 + 1 : idx2 - 1;
                        }

                        var temp = eventCards[idx1];
                        eventCards[idx1] = eventCards[idx2];
                        eventCards[idx2] = temp;
                    }
                }
            }

            for (int i = eventCards.Count - 2; i >= eventCards.Count - playerCount - 1; i--)
            {
                if (eventCards[i].Type == EventCardType.WONDERS_REDUCED)
                {
                    int r = _rng.Next(eventCards.Count - playerCount);
                    while (eventCards[r].Type == EventCardType.WONDERS_REDUCED)
                    {
                        r = _rng.Next(eventCards.Count - playerCount);
                    }

                    var temp = eventCards[r];
                    eventCards[r] = eventCards[i];
                    eventCards[i] = temp;
                }
            }

            ////Wonders are always reduced before wonders are built
            //int wondersReducedIdx = eventCards.FindLastIndex(x => x.Type == EventCardType.WONDERS_REDUCED);
            //int wondersBuiltIdx = eventCards.FindIndex(x => x.Type == EventCardType.WONDERS_REMOVED); // Assumed only 1 wonders built per era
            //if (wondersBuiltIdx < wondersReducedIdx)
            //{
            //    var temp = eventCards[wondersBuiltIdx];
            //    int x = wondersBuiltIdx;
            //    eventCards[wondersBuiltIdx] = eventCards[wondersReducedIdx];
            //    wondersBuiltIdx = wondersReducedIdx;
            //    eventCards[wondersReducedIdx] = temp;
            //    wondersReducedIdx = x;
            //}

            //// Wonders should not be built immediately after they are reduced
            //for (int i = wondersBuiltIdx - 1; i >= wondersBuiltIdx - playerCount; i--)
            //{
            //    if (i < 0)
            //        break;

            //    if (eventCards[i].Type == EventCardType.WONDERS_REDUCED)
            //    {
            //        int diff = wondersBuiltIdx - i;
            //        int offset = playerCount - diff;

            //        int newIdx = wondersBuiltIdx + offset;

            //        if (newIdx < finalIdx)
            //        {
            //            var temp = eventCards[newIdx];
            //            eventCards[newIdx] = eventCards[wondersBuiltIdx];
            //            eventCards[wondersBuiltIdx] = temp;
            //            break;
            //        }
            //        else
            //        {
            //            newIdx = i - offset;

            //            while (eventCards[newIdx].Type == EventCardType.WONDERS_REDUCED)
            //                newIdx--;

            //            var temp = eventCards[newIdx];
            //            eventCards[newIdx] = eventCards[i];
            //            eventCards[i] = temp;
            //        }
            //    }
            //}
        }

        private void ShuffleWonders(List<Wonder> wonders)
        {
            for (int i = 0; i < 100; i++)
            {
                int idx1 = _rng.Next(wonders.Count);
                int idx2 = _rng.Next(wonders.Count);

                var temp = wonders[idx1];
                wonders[idx1] = wonders[idx2];
                wonders[idx2] = temp;
            }
        }

        private void ShuffleMapTiles()
        {
            for (int i = 0; i < 1000; i++)
            {
                int idx1 = _rng.Next(MapTilesDeck.Count);
                int idx2 = _rng.Next(MapTilesDeck.Count);

                var temp = MapTilesDeck[idx1];
                MapTilesDeck[idx1] = MapTilesDeck[idx2];
                MapTilesDeck[idx2] = temp;
            }
        }

        #endregion

        //TODO: Add Data Access and Manager Classes
        #region Data Region
        private IEnumerable<Civilization> GetCivilizations()
        {
            return new List<Civilization>()
            {
                new Civilization("America", "Teddy Roosevelt", "Roosevelt Corollary", "When you gain or spend a natural wonder token, place it on any card in your focus row. You can spend a natural wonder token on a focus card as a trade token on that card or as a resource"),
                new Civilization("Aztec", "Montezuma", "Legend of the Five Suns", "After you reset your military focus card, if you won at least 1 attack this turn, you may swap any 2 cards in your focus row"),
                new Civilization("China", "Qin Shi Huang", "Great Wall", "When defending, your reinforced control tokens increase your combat value by 2 instead of 1"),
                new Civilization("Egypt", "Cleopatra", "Monument Builders", "The cost of all world wonders is reduced by 1 for you"),
                new Civilization("England", "Victoria", "Pax Britannica", "When you build a city, if it is the only city on its tile (excluding city-states), you may place 1 of your unused, unreinforced tokens in a space adjacent to that city"),
                new Civilization("France", "Catherine de Medici", "Grand Tour", "When resolving your culture focus card, place additional control tokens based on your latest-era world wonder:\nAncient: 1 token\nMedieval: 2 tokens\nModern: 3tokens"),
                new Civilization("Georgia", "Tamar", "Strength in Unity", "When resolving a focus card, if you have a diplomacy card from a city-state of that focus card's type, resolve it as though it is 1 slot further to the right"),
                new Civilization("Greece", "Pericles", "Surrounded By Glory", "After gaining a diplomacy card from a city-state, you can add that diplomacy card to your focus card matching the city-states type. When activating a focus card with diplomacy cards on it, you can treat the diplomacy cards as spent trade tokens."),
                new Civilization("Inca", "Pachacuti", "Terrace Farms", "After you place a control token on a mountaian space, you may place a control token on a space adjacent to that space (which can trigger this effect again)"),
                new Civilization("Indonesia", "Gitarja", "Jongs", "Your caravans and armies can move into water. When you move caravan, treat water spaces on the edge of the map as though they are adjacent to each other"),
                new Civilization("Japan", "Hojo Tokimune", "Divine Wind", "During your turn, desert and mountain spaces that are adjacent to water or at the edge of the map are treated as having a terrain difficulty of 3"),
                new Civilization("Netherlands", "Wilhelmina", "Polders", "Water spaces adjacent to your districts are treated as friendly spaces of all terrain types when resolving your districts abilities"),
                new Civilization("Nubia", "Amanitore", "Kandake of Meroe", "After you reset your growth focus card, resolve the effect of any 1 of your districts"),
                new Civilization("Ottoman", "Suleiman", "Grand Vizier", "At the start of your turn, you may choose another player. Give that player the \"Ibrahim\" card. When attacking or defending against the player with the \"Ibrahim\" card, increase your combat value by 2"),
                new Civilization("Poland", "Jadwiga", "Lithuanian Union", "At the start of your first turn, choose another player. Take 1 diplomacy card of your choice from that player. You can have any number of diplomacy cards from the choosen player"),
                new Civilization("Russia", "Peter the Great", "Mother Russia", "When placing control tokens, treat spaces 2 spaces away from cities as being adjacent to a city and spaces 3 spaces away from friendly cities as being only 2 spaces away"),
                new Civilization("Scythia", "Tomyris", "People of the Steppe", "When attacking or defending a grassland or hill space, increase your combat value by 3"),
                new Civilization("Sumeria", "Gilgamesh", "Epic Quest", "When you defeat a barbarian, gain1 resource of your choice from the supply (in addition to a trade token)"),
                new Civilization("Zulu", "Shaka", "Isibongo", "After you win a combat as the attacker, place 1 trade token from the supply on your military focus card, plus 1 additional trade token if you attacked a rival city or city-state")
            };
        }

        private IEnumerable<VictoryCard> GetAllVictoryCards()
        {
            return new List<VictoryCard>()
            {
                new VictoryCard("Civilized", "Moneygrubber"),
                new VictoryCard("Defensive", "Devastating"),
                new VictoryCard("Diplomatic", "Hoarder"),
                new VictoryCard("Evangelical", "Prolific"),
                new VictoryCard("Explorer", "Aesthetic"),
                new VictoryCard("Industrious", "Progressive"),
                new VictoryCard("Populous", "Preservationist"),
                new VictoryCard("Provincial", "Diversified"),
                new VictoryCard("Technophile", "Scholarly"),
                new VictoryCard("Warmonger", "Paranoid")
            };
        }

        private List<EventCard> GetAllEventCards()
        {
            return new List<EventCard>()
            {
                new EventCard("Nothing Happens", "", 2, EventCardEra.ANCIENT, EventCardType.NOTHING),
                new EventCard("Every Barbarian moves by 1 space", "Every Barbarian moves by 1 space. Roll a die to get each barbarian's direction", 2, EventCardEra.ANCIENT, EventCardType.BARBARIANS_MOVE_1),
                new EventCard("Every Barbarian moves by 1 space", "Every Barbarian moves by 1 space. Roll a die to get each barbarian's direction", 2, EventCardEra.ANCIENT, EventCardType.BARBARIANS_MOVE_1),
                new EventCard("Every Barbarian moves by 1 space", "Every Barbarian moves by 1 space. Roll a die to get each barbarian's direction", 2, EventCardEra.MEDIEVAL, EventCardType.BARBARIANS_MOVE_1),
                new EventCard("Every Barbarian moves by 1 space", "Every Barbarian moves by 1 space. Roll a die to get each barbarian's direction", 2, EventCardEra.MEDIEVAL, EventCardType.BARBARIANS_MOVE_1),
                new EventCard("Every Barbarian moves by 2 spaces", "Every Barbarian moves by 2 spaces. Roll a die to get each barbarian's direction", 2, EventCardEra.INDUSTRIAL, EventCardType.BARBARIANS_MOVE_2),
                new EventCard("Every Barbarian moves by 2 spaces", "Every Barbarian moves by 2 spaces. Roll a die to get each barbarian's direction", 2, EventCardEra.INDUSTRIAL, EventCardType.BARBARIANS_MOVE_2),
                new EventCard("Every Barbarian moves by 3 spaces", "Every Barbarian moves by 2 spaces. Roll a die to get each barbarian's direction", 2, EventCardEra.ATOMIC, EventCardType.BARBARIANS_MOVE_2),
                new EventCard("Every Barbarian moves by 3 spaces", "Every Barbarian moves by 2 spaces. Roll a die to get each barbarian's direction", 2, EventCardEra.ATOMIC, EventCardType.BARBARIANS_MOVE_2),
                new EventCard("Every Barbarian moves by 3 spaces", "Every Barbarian moves by 3 spaces. Roll a die to get each barbarian's direction", 2, EventCardEra.FUTURE, EventCardType.BARBARIANS_MOVE_3),
                new EventCard("Every Barbarian moves by 3 spaces", "Every Barbarian moves by 3 spaces. Roll a die to get each barbarian's direction", 2, EventCardEra.FUTURE, EventCardType.BARBARIANS_MOVE_3),
                new EventCard("Players may change their government or religious belief / Wonders Get Cheaper", "", 2, EventCardEra.ANCIENT, EventCardType.GOVERNMENTS),
                new EventCard("Players may change their government or religious belief / Wonders Get Cheaper", "", 2, EventCardEra.MEDIEVAL, EventCardType.GOVERNMENTS),
                new EventCard("Districts / Ancient Event", "Every district activates.\n\nTHUNDER STORMS:\nRoll a die to select a player. Then pillage any control / army / navy / or barbarian token next to or on a plain or hill", 2, EventCardEra.ANCIENT, EventCardType.DISTRICTS),
                new EventCard("Districts / Ancient Event", "Every district activates.\n\nCARVINGS:\nEvery Player can place 1 control token adjacent to a city", 2, EventCardEra.ANCIENT, EventCardType.DISTRICTS),
                new EventCard("Districts / Ancient Event", "Every district activates\n\nFLOODS:\nEvery player may spend a trade token from their focus row to replace one of their control tokens with a district", 2, EventCardEra.ANCIENT, EventCardType.DISTRICTS),
                new EventCard("Districts / Ancient Event", "Every district activates\n\nDEFENSIVE TACTICS:\nEvery Player can add 2 trade tokens to their growth card", 2, EventCardEra.ANCIENT, EventCardType.DISTRICTS),
                new EventCard("Districts / Classical Event", "Every district activates", 2, EventCardEra.CLASSICAL, EventCardType.DISTRICTS),
                new EventCard("Districts / Classical Event", "Every district activates", 2, EventCardEra.CLASSICAL, EventCardType.DISTRICTS),
                new EventCard("Districts / Classical Event", "Every district activates", 2, EventCardEra.CLASSICAL, EventCardType.DISTRICTS),
                new EventCard("Districts / Classical Event", "Every district activates", 2, EventCardEra.CLASSICAL, EventCardType.DISTRICTS),
                new EventCard("Districts / Medieval Event", "Every district activates\n\nFOREST FIRE:\nRoll a die to select a player. Then pillage any control / army / navy / or barbarian token next to or on a Forest", 2, EventCardEra.MEDIEVAL, EventCardType.DISTRICTS),
                new EventCard("Districts / Medieval Event", "Every district activates\n\nMERCENARIES:\nEvery player may spend 1 trade token to remove a barbarian", 2, EventCardEra.MEDIEVAL, EventCardType.DISTRICTS),
                new EventCard("Districts / Medieval Event", "Every district activates\n\nCRUSADES:\nEvery player may spend 1 trade token to shift their military card 2 spaces to the right", 2, EventCardEra.MEDIEVAL, EventCardType.DISTRICTS),
                new EventCard("Districts / Medieval Event", "Every district activates\n\nDROUGHT:\nFor each city, a player can either pillage one of their control tokens or spend a trade token", 2, EventCardEra.MEDIEVAL, EventCardType.DISTRICTS),
                new EventCard("Districts / Industrial Event", "Every district activates\n\nENLIGHTENMENT:\nEvery player may spend 2 trade tokens from their focus row to recruit a new Great Person", 2, EventCardEra.INDUSTRIAL, EventCardType.DISTRICTS),
                new EventCard("Districts / Industrial Event", "Every district activates\n\nGREAT DEPRESSION:\nEvery player loses 1 trade token per level of their economy card. If they have no more trade tokens, remove a control token instead", 2, EventCardEra.INDUSTRIAL, EventCardType.DISTRICTS),
                new EventCard("Districts / Industrial Event", "Every district activates\n\nSANDSTORM:\nRoll a die to select a player. Then pillage any control / army / navy / or barbarian token next to or on a Desert", 2, EventCardEra.INDUSTRIAL, EventCardType.DISTRICTS),
                new EventCard("Districts / Industrial Event", "Every district activates\n\nDIPLOMATIC INCIDENT:\n - Every player gives back 1 diplomacy card", 2, EventCardEra.INDUSTRIAL, EventCardType.DISTRICTS),
                new EventCard("Districts / Atomic Event", "Every district activates\n\nWORLD WAR:\nEvery player may spend a trade token to shift their military card 3 spaces to the right", 2, EventCardEra.ATOMIC, EventCardType.DISTRICTS),
                new EventCard("Districts / Atomic Event", "Every district activates\n\nVOLCANIC ERUPTION:\nRoll a die to select a player. Then pillage any control / army / navy / or barbarian token next to or on a Mountain", 2, EventCardEra.ATOMIC, EventCardType.DISTRICTS),
                new EventCard("Districts / Atomic Event", "Every district activates\n\nMETEOR:\nRoll a die to select a player. They remove one of their control tokens and get a resource of their choice", 2, EventCardEra.ATOMIC, EventCardType.DISTRICTS),
                new EventCard("Districts / Atomic Event", "Every district activates\n\nANTIQUITY:\nShuffle the world wonders removed from the game and draw 1. Put it next to the wonders available to build", 2, EventCardEra.ATOMIC, EventCardType.DISTRICTS),
                new EventCard("Districts / Future Event", "Every district activates\n\nHURRICANE:\nRoll a die to select a player. Then pillage any control / army / navy / or barbarian token next to or on a Water space", 2, EventCardEra.FUTURE, EventCardType.DISTRICTS),
                new EventCard("Districts / Future Event", "Every district activates\n\nGLOBAL WARMING:\nEvery player pillages 1 control token adjacent to water", 2, EventCardEra.FUTURE, EventCardType.DISTRICTS),
                new EventCard("Districts / Future Event", "Every district activates\n\nFUTURE TECH:\nEvery player activates their science card as if it were in the 5th slot", 2, EventCardEra.FUTURE, EventCardType.DISTRICTS),
                new EventCard("Districts / Future Event", "Every district activates\n\nNUCLEAR ACCIDENT:\nThe players with the most districts must pillage one of their districts", 2, EventCardEra.FUTURE, EventCardType.DISTRICTS),
                new EventCard("Barbarians Spawn / Wonders Get Cheaper / End of the Ancient Era", "Now begins your greatest quest: from this early cradle of civilization on towards the stars", 2, EventCardEra.ANCIENT, EventCardType.ERA_SCORE),
                new EventCard("End of the Future Era", "New frontiers of discovery expand our understanding, from the tiny atom to the majesty of outer space", 2, EventCardEra.FUTURE, EventCardType.ERA_SCORE),
                new EventCard("Barbarians Spawn / Wonders Get Cheaper / End of the Classical Era", "The sky above begins to reveal its secrets, a collection of heaven that uplifts our hearts and guides us to foriegn shores", 2, EventCardEra.CLASSICAL, EventCardType.ERA_SCORE),
                new EventCard("Barbarians Spawn / Wonders Get Cheaper / End of the Industrial Era", "Now your challenge is to maintain the delicate balance between earth and man, between peace and war", 2, EventCardEra.INDUSTRIAL, EventCardType.ERA_SCORE),
                new EventCard("Barbarians Spawn / Wonders Get Cheaper / End of the Medieval Era", "Just as the young apprentice learns to carry a sword, so shall you grow to understand your place in this world", 2, EventCardEra.MEDIEVAL, EventCardType.ERA_SCORE),
                new EventCard("Barbarians Spawn / Wonders Get Cheaper / End of the Atomic Era", "With flight and new forms of communication, you can create a small and intimate world. But, at what cost?", 2, EventCardEra.ATOMIC, EventCardType.ERA_SCORE),
                new EventCard("Players may change their government or religious belief / Wonders get cheaper / World Games", "", 2, EventCardEra.ATOMIC, EventCardType.GOVERNMENTS),
                new EventCard("Players may change their government / Wonders get cheaper", "", 2, EventCardEra.CLASSICAL, EventCardType.GOVERNMENTS),
                new EventCard("Players must change either their government or religious belief / Wonders get cheaper / Worlds Fair", "", 2, EventCardEra.INDUSTRIAL, EventCardType.GOVERNMENTS),
                new EventCard("Players may change their government to a Near Future Government, or change their religious belief / Wonders get cheaper / Laser Station", "", 2, EventCardEra.FUTURE, EventCardType.GOVERNMENTS),
            };
        }

        private IEnumerable<MapTile> GetMapTiles()
        {
            return new List<MapTile>()
            {
                new MapTile(1, "A"),
                new MapTile(2, "B"),
                new MapTile(3, "C"),
                new MapTile(4, "D"),
                new MapTile(5 , "E"),
                new MapTile(6),
                new MapTile(7, true),
                new MapTile(8),
                new MapTile(9, true),
                new MapTile(10, "F"),
                new MapTile(11, true, "G"),
                new MapTile(12, "H"),
                new MapTile(13),
                new MapTile(14),
                new MapTile(15, "I"),
                new MapTile(16, true),
                new MapTile(17, true),
                new MapTile(18, "J"),
                new MapTile(19),
                new MapTile(20, "K"),
                new MapTile(21)
            };
        }

        private IEnumerable<Wonder> GetAllWonders()
        {
            return new List<Wonder>()
            {
                new Wonder("Alhambra", "When attacking or defending, increase your combat value by 2", WonderType.MILITARY, WonderEra.MEDIEVAL, 10, Resource.OIL, Resource.MARBLE),
                new Wonder("Amundsen-Scott Research Station", "When you build this wonder, build a city on any legal space on the edge of the map and place this wonder in that city. Then place up to 2 control tokens in spaces adjacent to that city", WonderType.SCIENCE, WonderEra.MODERN, 10, Resource.MERCURY, Resource.DIAMOND),
                new Wonder("Apadana", "When you build or capture this wonder, choose an edge space on any tile. Explore from that space. Then, if you placed a tile, place 1 control token on an empty space on that tile", WonderType.ECONOMY, WonderEra.ANCIENT, 8, Resource.DIAMOND, Resource.MARBLE),
                new Wonder("Big Ben", "When attacking or defending, increase your combat value by 2 for each of your caravans adjacent to the defending space", WonderType.ECONOMY, WonderEra.MODERN, 10, Resource.DIAMOND, Resource.MERCURY),
                new Wonder("Chichen Itza", "When placing control tokens, you can place them on empty forest spaces that are not adjacent to a friendly city", WonderType.CULTURE, WonderEra.MEDIEVAL, 10, Resource.MARBLE, Resource.MERCURY),
                new Wonder("Colosseum", "At the start of your turn, you may reinforce 1 of your control tokens that is adjacent to a friendly city", WonderType.CULTURE, WonderEra.ANCIENT, 9, Resource.MARBLE, Resource.OIL),
                new Wonder("Colossus", "When resolving your economy focus card, your caravans can move a total of 6 additional spaces, divided as you choose", WonderType.ECONOMY, WonderEra.ANCIENT, 7, Resource.DIAMOND, Resource.OIL),
                new Wonder("Cristo Redentor", "When you build or capture this wonder, choose a rival non-capitol city (without an army in its space) within 3 spaces of this wonder. Replace that city with 1 of your unused cities", WonderType.CULTURE, WonderEra.MODERN, 11, Resource.MARBLE, Resource.MERCURY),
                new Wonder("Eiffel Tower", "At the start of your turn, you may choose 2 rival control tokens on the map belonging to the same player. Tha player replaces 1 of those tokens with 1 of your unused unreinforced control tokens", WonderType.CULTURE, WonderEra.MODERN, 12, Resource.MARBLE, Resource.OIL),
                new Wonder("Estadio do Maracana", "After you resolve a focus card other than your economy card, you can resolve and reset your econoy card", WonderType.ECONOMY, WonderEra.MODERN, 10, Resource.DIAMOND, Resource.MARBLE),
                new Wonder("Forbidden City", "At the start of your turn, you may destroy 1 rival control token that is adjacent to a friendly space", WonderType.CULTURE, WonderEra.MEDIEVAL, 9, Resource.MARBLE, Resource.OIL),
                new Wonder("Great Library", "When your caravan moves to another player's city, you may gain a focus card of the same type and tech level as a card in that player's focus row, replacing your card of the same type", WonderType.SCIENCE, WonderEra.ANCIENT, 8, Resource.MERCURY, Resource.DIAMOND),
                new Wonder("Great Lighthouse", "When building cities, you can build in empty spaces on the edge of the map, as if they were within 2 spaces of a friendly space", WonderType.ECONOMY, WonderEra.ANCIENT, 8, Resource.DIAMOND, Resource.MERCURY),
                new Wonder("Great Zimbabwe", "You can place trade tokens on this card instead of on your focus cards, up to a limit of 4. At the start of your turn, you may move trade tokens from this card to cards in your focus row", WonderType.ECONOMY, WonderEra.MEDIEVAL, 9, Resource.DIAMOND, Resource.OIL),
                new Wonder("Hanging Gardens", "At the start of your turn, you may place 1 control token on a space of terrain difficulty 4 or less thata is adjacent to a friendly city", WonderType.CULTURE, WonderEra.ANCIENT, 8, Resource.MARBLE, Resource.DIAMOND),
                new Wonder("Huey Teocalli", "When defending, increase your combat value by 1 for each water space that is adjacent to the defending space", WonderType.MILITARY, WonderEra.MEDIEVAL, 9, Resource.OIL, Resource.MERCURY),
                new Wonder("Jebel Barkal", "When attacking or defending, you can spend resource tokens (not natural wonder tokens) to increase your combat value by 2 for each resurce spent", WonderType.MILITARY, WonderEra.ANCIENT, 7, Resource.OIL, Resource.MERCURY),
                new Wonder("Kilwa Kisiwani", "When you move a caravan to a city-state, place 1 additional trade token from the supply on any 1 of your focus cards", WonderType.ECONOMY, WonderEra.MEDIEVAL, 9, Resource.DIAMOND, Resource.MARBLE),
                new Wonder("Kremlin", "When attacking a rival space, increase your combat value by 4 if you have more reinforced control tokens on the map than the defending player", WonderType.SCIENCE, WonderEra.MODERN, 11, Resource.MERCURY, Resource.OIL),
                new Wonder("Machu Picchu", "When you resolve a card in the first or second slot of your focus row, resolve it as though it is 2 slots farther to the right", WonderType.ECONOMY, WonderEra.MEDIEVAL, 10, Resource.DIAMOND, Resource.MERCURY),
                new Wonder("Pentagon", "When attacking, increase your combat value by 2. Your armies can move any number of spaces. (They must still obey all other movement rules)", WonderType.MILITARY, WonderEra.MODERN, 12, Resource.OIL, Resource.DIAMOND),
                new Wonder("Petra", "When defending, increase your combat value by 2. Barbarians cannot move into spaces containing your cities or reinforced control tokens", WonderType.MILITARY, WonderEra.ANCIENT, 7, Resource.OIL, Resource.DIAMOND),
                new Wonder("Porcelian Tower", "When you build this wonder, replace up to 2 cards in your focus row with cards of the next highest tech level of the same type", WonderType.SCIENCE, WonderEra.MEDIEVAL, 9, Resource.MERCURY, Resource.DIAMOND),
                new Wonder("Polta Palace", "You can have 4 diplomacy cards from each other player. When you build this wonder, you may take a total of 3 diplomacy cards of your choice from other players", WonderType.SCIENCE, WonderEra.MEDIEVAL, 10, Resource.MERCURY, Resource.MARBLE),
                new Wonder("Pyramids", "When you build this wonder, choose up to 3 level-I cards in your focus row. Replace each with a level-II card of the same type", WonderType.SCIENCE, WonderEra.ANCIENT, 9, Resource.MERCURY, Resource.OIL),
                new Wonder("Oracle", "At the start of your turn, you may swap 2 adjacent cards in your focus row", WonderType.SCIENCE, WonderEra.ANCIENT, 8, Resource.MERCURY, Resource.MARBLE),
                new Wonder("Orszaghaz", "After you move a caravan into a city-state, you may conquer it", WonderType.ECONOMY, WonderEra.MODERN, 11, Resource.DIAMOND, Resource.OIL),
                new Wonder("Oxford University", "When you replace a focus card other than a science focus card, you do not have to replace it with a card of the same type", WonderType.SCIENCE, WonderEra.MODERN, 10, Resource.MERCURY, Resource.MARBLE),
                new Wonder("Ruhr Valley", "When defending, increase your combat value by 5", WonderType.MILITARY, WonderEra.MODERN, 11, Resource.OIL, Resource.MERCURY),
                new Wonder("Statue of Liberty", "Before you replace a rival city with 1 of your cities, replace all rival control tokens that are adjacent to that rival city with your unused, unreinforced control tokens", WonderType.MILITARY, WonderEra.MODERN, 12, Resource.OIL, Resource.MARBLE),
                new Wonder("Stonehenge", "After you place a control token on a hill space, you may place a control token on 1 or more hill spaces adjacent to that space (which may trigger this effect again)", WonderType.CULTURE, WonderEra.ANCIENT, 7, Resource.MARBLE, Resource.MERCURY),
                new Wonder("Sydney Opera House", "Rival control tokens contribute toward your cities' maturity", WonderType.CULTURE, WonderEra.MODERN, 10, Resource.MARBLE, Resource.DIAMOND),
                new Wonder("Taj Mahal", "When you resolve a focus card, resolve it as though it is 1 slot farther to the right for each world wonder you control matching the focus cards type", WonderType.CULTURE, WonderEra.MEDIEVAL, 9, Resource.MARBLE, Resource.DIAMOND),
                new Wonder("Terracotta Army", "When attacking, increase your combat value by 2", WonderType.MILITARY, WonderEra.ANCIENT, 8, Resource.OIL, Resource.MARBLE),
                new Wonder("University of Sankore", "At the end of your turn, if you replaced 1 or more of your focus cards this turn, you may swap any 2 non-science cards in your focus row", WonderType.SCIENCE, WonderEra.MEDIEVAL, 9, Resource.MERCURY, Resource.OIL),
                new Wonder("Venetian Arsenal", "Once per turn, after you resolve the card in the 5th slot of your focus row, you may resolve it again, treating it as if it was in the first slot", WonderType.MILITARY, WonderEra.MEDIEVAL, 10, Resource.OIL, Resource.DIAMOND)
            };
        }
        #endregion
    }
}
