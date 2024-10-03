using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivANewDawnAssistModels
{
    public class MapTile
    {
        public string Barb { get; }
        public int Num { get; }
        public bool IsStartingTile { get; }
        public bool HasBarb => Barb != string.Empty;

        public MapTile(int num)
        {
            Num = num;
            IsStartingTile = false;
            Barb = string.Empty;
        }

        public MapTile(int num, string barb)
        {
            Num = num;
            IsStartingTile = false;
            Barb = barb;
        }

        public MapTile(int num, bool isCapitolTile)
        {
            Num = num;
            IsStartingTile = isCapitolTile;
            Barb = string.Empty;
        }

        public MapTile(int num, bool isCapitolTile, string barb)
        {
            Num = num;
            IsStartingTile = isCapitolTile;
            Barb = barb;
        }

        public override string ToString()
        {
            return Num.ToString();
        }

    }
}
