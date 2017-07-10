using System;
using System.Collections.Generic;
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
    }

    public class Cards
    {
        public List<Place> Places { get; } = new List<Place>();
        public List<Slayer> Persons { get; } = new List<Slayer>();
        public List<Weapon> Weapons { get; } = new List<Weapon>();
    }

    public class PlayerKnowledge
    {
        public Setup[] AskedSetup { get; set; }
        public Cards Cards { get; set; }
        public Cards ShownToPlayerCards { get; set; }
        public Setup[] CanHelpWith { get; set; }
        public Setup[] NotPresent { get; set; }
    }

    public class Player
    {
        public string Name { get; set; }
        public Cards Cards { get; set; }
        public Dictionary<Player, PlayerKnowledge> Knowledge { get; set; }
    }
}
