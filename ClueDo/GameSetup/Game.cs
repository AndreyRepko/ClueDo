using System;
using System.Linq;

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
            var deck = new Deck();
            Murder = deck.GetMurderSetup();
            PublicCards = deck.GetKnownCards(GameConstants.GameCardsSetup[playersNumber].Key);
            Players = new Player[playersNumber];
            var playerCardsNumber = GameConstants.GameCardsSetup[playersNumber].Value;
            for (var i = 0; i < playersNumber; i++)
            {
                Players[i] = new Player($"Player{i}", playersNumber, i,
                    PublicCards, deck.GetKnownCards(playerCardsNumber),
                    GameConstants.GameCardsSetup[playersNumber].Value)
                {
                    Strategy = new DumbStrategy()
                };
            }
            deck.CheckNoCards();
            CurrentPlayer = 0;
        }

        public void DoNextTurn()
        {
            if (Charge()) return;

            var askedSetup = Players[CurrentPlayer].AskSetup();

            Array.ForEach(Players, p => p.RegisterAskedSetup(askedSetup, CurrentPlayer));

            for (var i = CurrentPlayer+1;; i++)
            {
                if (i == Players.Length) i = 0;
                if (i == CurrentPlayer)
                {
                    Array.ForEach(Players, p => p.RegisterWinSetup(askedSetup));
                    Charge();
                    break;
                }

                var help = Players[i].CanHelpWith(askedSetup, CurrentPlayer);

                if (help == null)
                {
                    Array.ForEach(Players, p => p.RegisterNoHelp(askedSetup, i));
                }
                else
                {
                    Players[CurrentPlayer].RegisterHelp(askedSetup, help, i);
                    foreach (var player in Players.Where(p => p != Players[CurrentPlayer]))
                        player.RegisterHelpGiven(askedSetup, CurrentPlayer, i);
                    break;
                }
            }
            Array.ForEach(Players, p => p.DoDeduction());

            Array.ForEach(Players, p1 => Array.ForEach(Players, p2 => KnowledgeSanityCheck.CheckWrongDeduction(p1.OwnCards, p1.SelfNumber, p2.Knowledge)));

            CurrentPlayer = (CurrentPlayer+1)% Players.Length;
        }

        private bool Charge()
        {
            var charge = Players[CurrentPlayer].AskForCharge();
            if (charge != null)
            {
                if (Equals(charge, Murder))
                {
                    Winner = CurrentPlayer;
                    throw new Exception($"winner is {Winner}");
                }
                return true;
            }
            return false;
        }

        public int Winner { get; set; }
    }
}
