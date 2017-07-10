using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.GameSetup
{
    public static class GameConstants
    {
        /// <summary>
        /// Must be read as Players -> Public cards, Players card
        /// </summary>
        public static Dictionary<int, KeyValuePair<int, int>> GameCardsSetup =
            new Dictionary<int, KeyValuePair<int, int>>
            {
                {5, new KeyValuePair<int, int>(3, 3)},
                {4, new KeyValuePair<int, int>(2, 4)}
            };
    }
}
