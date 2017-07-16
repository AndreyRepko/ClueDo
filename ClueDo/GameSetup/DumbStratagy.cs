using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClueDo.Utilities;

namespace ClueDo.GameSetup
{
    public interface IStrategy
    {
        Setup ReadyToCharge(PlayerKnowledge knowledge, int self);
        Setup AskForGuess(PlayerKnowledge knowledge, int self);

        SingleCard CanHelpWith(PlayerKnowledge knowledge, Setup setup, int asker);
    }
    public class DumbStrategy : IStrategy
    {
        private Random _random;

        public DumbStrategy()
        {
           _random = new Random(); 
        }

        public Setup ReadyToCharge(PlayerKnowledge knowledge, int self)
        {
            if (knowledge.PotentialPlaces.Count == 1
                && knowledge.PotentialWeapons.Count == 1
                && knowledge.PotentialSlayers.Count == 1)
            {
                return new Setup(knowledge.PotentialPlaces.Single(), knowledge.PotentialSlayers.Single(), knowledge.PotentialWeapons.Single());
            }
            return null;
        }

        public Setup AskForGuess(PlayerKnowledge knowledge, int self)
        {
            var place = knowledge.PotentialPlaces.GetRandomElement(_random);
            var weapon = knowledge.PotentialWeapons.GetRandomElement(_random);
            var slayer = knowledge.PotentialSlayers.GetRandomElement(_random);
            return new Setup(place, slayer, weapon);
        }

        public SingleCard CanHelpWith(PlayerKnowledge knowledge, Setup setup, int asker)
        {
            var ownCards = knowledge.OwnCards;
            if (ownCards.Places.Contains(setup.Place))
                return new SingleCard(setup.Place);
            if (ownCards.Weapons.Contains(setup.Weapon))
                return new SingleCard(setup.Weapon);
            if (ownCards.Persons.Contains(setup.Slayer))
                return new SingleCard(setup.Slayer);

            return null;
        }
    }
}
