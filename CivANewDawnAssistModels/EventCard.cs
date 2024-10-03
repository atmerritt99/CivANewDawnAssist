using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivANewDawnAssistModels
{
    public class EventCard
    {
        public string Name { get; }
        public string Event { get; set; }
        public int MinPlayers { get; }
        public EventCardEra Era { get; }
        public EventCardType Type { get; }
        public EventCard(string name, string _event, int minPlayers, EventCardEra era, EventCardType type)
        {
            Name = name;
            Event = _event;
            MinPlayers = minPlayers;
            Era = era;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Name}\n{Event}";
        }
    }
}
