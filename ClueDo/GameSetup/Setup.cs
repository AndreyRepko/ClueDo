using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;

namespace ClueDo.GameSetup
{
    public enum Slayer
    {
        Green = 0,
        White = 1,
        Mustard = 2,
        Scarlet = 3,
        Plum = 4,
        Peacock = 5
    }

    public enum Weapon
    {
        SteelPipe = 0,
        Rope = 1,
        Spanner = 2,
        Gun = 3,
        Knife = 4,
        Candle = 5
    }

    public enum Place
    {
        InnerGarden = 0,
        DinnerRoom = 1,
        Cabinet = 2,
        Bath = 3,
        Garage = 4,
        Poolhall = 5,
        Kitchen = 6,
        Bedroom = 7,
        LivingRoom = 8
    }

    public static class SetupExtensions
    {
        public static Place GetRandomPlace(this Random random)
        {
            return (Place) random.Next(9);
        }

        public static Weapon GetRandomDevice(this Random random)
        {
            return (Weapon)random.Next(6);
        }

        public static Slayer GetRandomPerson(this Random random)
        {
            return (Slayer)random.Next(6);
        }
    }

    public class Setup
    {
        public Setup(Place place, Slayer slayer, Weapon weapon)
        {
            Place = place;
            Slayer = slayer;
            Weapon = weapon;
        }

        public Place Place { get; }
        public Slayer Slayer { get; }
        public Weapon Weapon { get; }

        public override bool Equals(object obj)
        {
            if (!(obj is Setup))
                return false;
            var second = (Setup) obj;

            return Place == second.Place && Slayer == second.Slayer && Weapon == second.Weapon;
        }

        protected bool Equals(Setup other)
        {
            return Place == other.Place && Slayer == other.Slayer && Weapon == other.Weapon;
        }

        public override string ToString()
        {
            return $"Place {Place}, Slayer {Slayer}, Weapon {Weapon}";
        }

        public static bool operator ==(Setup a, Setup b)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            return a.Equals(b);
        }

        public static bool operator !=(Setup a, Setup b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Place;
                hashCode = (hashCode * 397) ^ (int) Slayer;
                hashCode = (hashCode * 397) ^ (int) Weapon;
                return hashCode;
            }
        }
    }

    public class Cards
    {
        public List<Place> Places { get; } = new List<Place>();
        public List<Slayer> Persons { get; } = new List<Slayer>();
        public List<Weapon> Weapons { get; } = new List<Weapon>();

        public bool Any()
        {
            return Places.Any() || Persons.Any() || Weapons.Any();
        }

        public int Count()
        {
            return Places.Count + Persons.Count + Weapons.Count;
        }
    }

    public class SingleCard : Cards
    {
        public SingleCard(Place place)
        {
            Places.Add(place);
        }

        public SingleCard(Weapon weapon)
        {
            Weapons.Add(weapon);
        }

        public SingleCard(Slayer person)
        {
            Persons.Add(person);
        }
    }
}
