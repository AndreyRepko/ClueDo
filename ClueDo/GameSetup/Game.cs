using System;

namespace ClueDo.GameSetup
{
    public class Game
    {
        public Cards PublicCards { get; set; }
        public Setup Murder { get; set; }
        public Player[] Players { get; set; }
        public int CurrentPlayer { get; set; }

        public void Initialize(int playersNumber)
        {
            Players = new Player[playersNumber];
            for(var i=0;i<playersNumber;i++)
                Players[i] = new Player {Name = $"Player{i}"};
            var deck = new Deck();
            Murder = deck.GetMurderSetup();
            PublicCards = deck.GetKnownCards(GameConstants.GameCardsSetup[playersNumber].Key);
            foreach (var player in Players)
            {
                player.Cards = deck.GetKnownCards(GameConstants.GameCardsSetup[playersNumber].Value);
            }
            deck.CheckNoCards();
        }
    }
}
