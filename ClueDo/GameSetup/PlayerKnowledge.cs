using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClueDo.Annotations;

namespace ClueDo.GameSetup
{
    public class PlayerKnowledge: INotifyPropertyChanged
    {
        public Cards OwnCards { get; }

        public PlayerKnowledge(Cards publicCards)
        {
            RemoveCardsFromPotentialSetup(publicCards);
        }

        public PlayerKnowledge(Cards publicCards, Cards ownCards) : this(publicCards)
        {
            OwnCards = ownCards;
            RemoveCardsFromPotentialSetup(ownCards);
        }

        private void RemoveCardsFromPotentialSetup(Cards cards)
        {
            PotentialWeapons.RemoveAll(cards.Weapons.Contains);
            PotentialSlayers.RemoveAll(cards.Persons.Contains);
            PotentialPlaces.RemoveAll(cards.Places.Contains);

            NotifyPropertyChanged(nameof(PotentialCards));

            if (PotentialWeapons.Count == 0)
                throw new Exception("weapon error");
            if (PotentialSlayers.Count == 0)
                throw new Exception("slayer error");
            if (PotentialPlaces.Count == 0)
                throw new Exception("place error");
        }

        public List<Slayer> PotentialSlayers = new List<Slayer> { Slayer.Green, Slayer.Mustard, Slayer.Peacock, Slayer.Plum, Slayer.Scarlet, Slayer.White};
        public List<Place> PotentialPlaces = new List<Place> { Place.Bath, Place.Bedroom, Place.Cabinet, Place.DinnerRoom, Place.Garage,
            Place.InnerGarden, Place.Kitchen, Place.LivingRoom, Place.Poolhall};
        public List<Weapon> PotentialWeapons = new List<Weapon> {Weapon.Candle, Weapon.Gun, Weapon.Knife, Weapon.Rope, Weapon.Spanner, Weapon.SteelPipe};

        public Cards PotentialCards
        {
            get
            {
                var cards = new Cards();
                cards.Persons.AddRange(PotentialSlayers);
                cards.Weapons.AddRange(PotentialWeapons);
                cards.Places.AddRange(PotentialPlaces);
                return cards;
            }
        }

        public void RegisterKnownCard(SingleCard help)
        {
            RemoveCardsFromPotentialSetup(help);
        }

        public void RemoveAllButSetup(Setup askedSetup)
        {
            PotentialPlaces.RemoveAll(p => p != askedSetup.Place);
            PotentialSlayers.RemoveAll(s => s != askedSetup.Slayer);
            PotentialWeapons.RemoveAll(w => w != askedSetup.Weapon);
            NotifyPropertyChanged(nameof(PotentialCards));
        }

        public void RegisterNoHelp(Setup askedSetup, int noHelper)
        {
            //
        }

        public void RegisterHelpedWithSetup(Setup askedSetup, int i)
        {
            //
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
