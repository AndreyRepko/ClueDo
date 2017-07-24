using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.GameSetup
{
    public static class KnowledgeSanityCheck
    {
        public static void CheckWrongDeduction(Cards ownCards, int owner, PlayerKnowledge knowledge)
        {
            foreach(var slayer in ownCards.Persons)
            if (knowledge.SlayerStatus[slayer][owner].FinalStatus == FinalStatus.NotPresent)
                    throw new InvalidOperationException();

            foreach (var weapon in ownCards.Weapons)
                if (knowledge.WeaponStatus[weapon][owner].FinalStatus == FinalStatus.NotPresent)
                    throw new InvalidOperationException();

            foreach (var place in ownCards.Places)
                if (knowledge.PlaceStatus[place][owner].FinalStatus == FinalStatus.NotPresent)
                    throw new InvalidOperationException();
        }
    }
}
