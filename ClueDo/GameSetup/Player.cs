using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ClueDo.Annotations;

namespace ClueDo.GameSetup
{

    public sealed class Player : INotifyPropertyChanged
    {
        private int PlayersNumber;
        private int NumberOfCards;

        public Player(string name, int playersNumber, int selfNumber, Cards publicCards, Cards ownCards, int numberOfCards)
        {
            Name = name;
            SelfNumber = selfNumber;
            OwnCards = ownCards;
            PlayersNumber = playersNumber;
            NumberOfCards = numberOfCards;
            Knowledge = new PlayerKnowledge(publicCards, ownCards, PlayersNumber, SelfNumber);
            NotifyPropertyChanged(nameof(Knowledge));
        }

        public IStrategy Strategy { get; set; }

        public PlayerKnowledge Knowledge { get; }

        public string Name { get; set; }
        public int SelfNumber { get; }
        public Cards OwnCards { get; }

        public Setup AskSetup()
        {
            return Strategy.AskForGuess(Knowledge, SelfNumber);
        }

        public Setup AskForCharge()
        {
            return Strategy.ReadyToCharge(Knowledge, SelfNumber);
        }

        public void RegisterWinSetup(Setup askedSetup)
        {
            Knowledge.RemoveAllButSetup(askedSetup);
        }

        public SingleCard CanHelpWith(Setup askedSetup, int currentPlayer)
        {
            return Strategy.CanHelpWith(Knowledge, askedSetup, currentPlayer);
        }

        public void RegisterNoHelp(Setup askedSetup, int noHelper)
        {
            Knowledge.RegisterNoHelp(askedSetup, noHelper);
        }

        public void RegisterHelp(Setup askedSetup, SingleCard help, int i)
        {
            Knowledge.RegisterKnownCard(help, i);
        }

        public void RegisterHelpGiven(Setup askedSetup, int askingPlayer, int i)
        {
            Knowledge.RegisterHelpedWithSetup(askedSetup, i);
        }

        public void RegisterAskedSetup(Setup askedSetup, int currentPlayer)
        {
            Knowledge.RegisterAskedSetup(askedSetup, currentPlayer);
        }

        public void DoDeduction()
        {
            Knowledge.DoDeduction();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
