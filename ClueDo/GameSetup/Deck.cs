using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace ClueDo.GameSetup
{
    public class Deck
    {
        private List<Place> _places  = new List<Place>();
        private List<Slayer> _persons = new List<Slayer>();
        private List<Weapon> _devices = new List<Weapon>();
        private Random _random;

        public Deck()
        {
            for (var i =0; i<9;i++)
                _places.Add((Place)i);
            for (var i = 0; i < 6; i++)
                _persons.Add((Slayer)i);
            for (var i = 0; i < 6; i++)
                _devices.Add((Weapon)i);
            _random = new Random(DateTime.Now.Millisecond);
        }

        public Setup GetMurderSetup()
        {
            var murderPlace = _random.GetRandomPlace();
            var murderPerson = _random.GetRandomPerson();
            var murderDevice = _random.GetRandomDevice();
            _places.Remove(murderPlace);
            _persons.Remove(murderPerson);
            _devices.Remove(murderDevice);

            return new Setup(murderPlace, murderPerson, murderDevice);
        }

        public Cards GetKnownCards(int cardsToPull)
        {
            var cards = new Cards();
            for (var i = 0; i < cardsToPull; i++)
            {
                var count = _places.Count + _persons.Count + _devices.Count;
                var position = _random.Next(count);
                if (position < _places.Count)
                    cards.Places.Add(PullPlace(position)); 
                else if (position < _places.Count + _persons.Count)
                    cards.Persons.Add(PullPerson(position - _places.Count));
                else
                    cards.Weapons.Add(PullDevice(position - _places.Count - _persons.Count));
            }
            return cards;
        }

        private Weapon PullDevice(int position)
        {
            var value = _devices.ElementAt(position);
            _devices.RemoveAt(position);
            return value;
        }

        private Slayer PullPerson(int position)
        {
            var value = _persons.ElementAt(position);
            _persons.RemoveAt(position);
            return value;
        }

        private Place PullPlace(int position)
        {
            var value = _places.ElementAt(position);
            _places.RemoveAt(position);
            return value;
        }

        public void CheckNoCards()
        {
            if (_places.Any() || _persons.Any() || _devices.Any())
                throw new Exception("Not all cards removed from the deck! Setup is incorrect!");
        }
    }
}
