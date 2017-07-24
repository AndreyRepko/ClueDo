using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClueDo.GameSetup;

namespace ClueDo.Utilities
{
    public static class Helpers
    {
        public static T GetRandomElement<T>(this IEnumerable<T> self, Random random)
        {
            return self.ElementAt(random.Next(self.Count()));
        }
    }

    public class PlayersStatus
    {
        public string Name { get; set; }

        public Status Player1 { get; set; }
        public Status Player2 { get; set; }
        public Status Player3 { get; set; }
        public Status Player4 { get; set; }
        public Status Player5 { get; set; }
        public Status Player6 { get; set; }
    }
}
