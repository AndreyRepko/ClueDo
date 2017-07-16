using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.Utilities
{
    public static class Helpers
    {
        public static T GetRandomElement<T>(this IEnumerable<T> self, Random random)
        {
            return self.ElementAt(random.Next(self.Count()));
        }
    }
}
