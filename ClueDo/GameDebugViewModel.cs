using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClueDo.Annotations;
using ClueDo.Utilities;
using ClueDo.GameSetup;

namespace ClueDo
{
    public class GameDebugViewModel : INotifyPropertyChanged
    {
        private int _playersNumber = 4;
        private Game _game;

        private RelayCommand _startGame;
        private RelayCommand _nextTurn;

        public RelayCommand StartGameCommand
        {
            get
            {
                return _startGame = _startGame ?? (_startGame = new RelayCommand(StartGame));
            }
        }

        public Setup GameSetup => _game?.Murder;
        public Cards GameOpenCards => _game?.PublicCards;

        public Player[] GamePlayers => _game?.Players;

        public Game Game => _game;

        public GameDebugViewModel()
        {
            StartGame();
        }

        private void StartGame()
        {
            _game = new Game();
            _game.Initialize(PlayersNumber);
            NotifyPropertyChanged(nameof(Game));
            NotifyPropertyChanged(nameof(GameSetup));
            NotifyPropertyChanged(nameof(GamePlayers));
            NotifyPropertyChanged(nameof(GameOpenCards));
        }

        private void NextTurn()
        {
            _game.DoNextTurn();
        }

        public int PlayersNumber
        {
            get { return _playersNumber; }
            set
            {
                if (_playersNumber != value)
                {
                    _playersNumber = value;
                    NotifyPropertyChanged(nameof(PlayersNumber));
                }
            }
        }

        public RelayCommand NextTurnCommand
        {
            get
            {
                return _nextTurn = _nextTurn ?? (_nextTurn = new RelayCommand(NextTurn));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
