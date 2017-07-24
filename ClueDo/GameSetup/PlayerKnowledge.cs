using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using ClueDo.Annotations;
using ClueDo.Utilities;

namespace ClueDo.GameSetup
{
    public enum FinalStatus
    {
        Unknown,
        Present,
        PartOfSetup,
        NotPresent
    }

    public class Status
    {
        public List<Setup> PartOf;
        public FinalStatus FinalStatus;

        public void UpdateStatus(FinalStatus status)
        {
            if (FinalStatus == FinalStatus.NotPresent && status == FinalStatus.Present)
                throw new ArgumentException("Could not change final status");
            if (FinalStatus == FinalStatus.Present && status == FinalStatus.NotPresent)
                throw new ArgumentException("Could not change final status");

            if (FinalStatus == FinalStatus.Present || FinalStatus == FinalStatus.NotPresent)
                return;
            FinalStatus = status;
        }

        public override string ToString()
        {
            switch (FinalStatus)
            {
                case FinalStatus.Unknown:
                    return "";
                    case FinalStatus.Present:
                    return "+";
                    case FinalStatus.PartOfSetup:
                    return "?";
                    case FinalStatus.NotPresent:
                    return "X";
            }
            return "Status not supported";
        }
    }

    public class PlayerKnowledge: INotifyPropertyChanged
    {
        private readonly int _playersNumber;
        private readonly int _selfNumber;
        public Cards OwnCards { get; }

        public List<Slayer> PotentialSlayers
        {
            get
            {
                var slayers = SlayerStatus.Where(p=>p.Value.All(s=>s.FinalStatus != FinalStatus.Present)).Select(p => p.Key);
                return slayers.ToList();
            }
        }

        public List<Place> PotentialPlaces
        {
            get
            {
                var places = PlaceStatus.Where(p => p.Value.All(s => s.FinalStatus != FinalStatus.Present)).Select(p => p.Key);
                return places.ToList();
            }
        }

        public List<Weapon> PotentialWeapons
        {
            get
            {
                var weapons = WeaponStatus.Where(p => p.Value.All(s => s.FinalStatus != FinalStatus.Present)).Select(p => p.Key);
                return weapons.ToList();
            }
        }

        public Dictionary<Place, Status[]> PlaceStatus { get; private set; }
        public Dictionary<Slayer, Status[]> SlayerStatus { get; private set; } 
        public Dictionary<Weapon, Status[]> WeaponStatus { get; private set; }

        public List<Status[]> GeneralStatus
        {
            get
            {
                var list = new List<Status[]>();
                list.AddRange(PlaceStatus.Values);
                list.AddRange(SlayerStatus.Values);
                list.AddRange(WeaponStatus.Values);
                return list;
            }
        }

        public Dictionary<int, List<Setup>> AskedSetups { get; set; }

        public List<Setup> OwnQuestions => AskedSetups.ContainsKey(_selfNumber) ? new List<Setup>(AskedSetups[_selfNumber]) : null;

        public PlayerKnowledge(Cards publicCards, Cards ownCards, int playersNumber, int selfNumber)
        {
            if (publicCards.Any())
                _playersNumber = playersNumber + 1; //Last one for dummy player/public cards
            else
                _playersNumber = playersNumber;

            _selfNumber = selfNumber;
            OwnCards = ownCards;
            CardsCount = OwnCards.Count();

            AskedSetups = new Dictionary<int, List<Setup>>();

            SetupInitialStatus();
            if (publicCards.Any())
            {
                RemoveCardsFromPotentialSetup(publicCards, _playersNumber-1);
                MarkAllAsNotPresentForPlayer(_playersNumber-1);
            }
            RemoveCardsFromPotentialSetup(ownCards, selfNumber);
        }

        public int CardsCount { get; set; }

        private void MarkAllAsNotPresentForPlayer(int playersNumber)
        {
            foreach (var pair in PlaceStatus)
            {
                var status = pair.Value[playersNumber];
                if (status.FinalStatus != FinalStatus.Present)
                    status.FinalStatus = FinalStatus.NotPresent;
            }
            foreach (var pair in WeaponStatus)
            {
                var status = pair.Value[playersNumber];
                if (status.FinalStatus != FinalStatus.Present)
                    status.FinalStatus = FinalStatus.NotPresent;
            }
            foreach (var pair in SlayerStatus)
            {
                var status = pair.Value[playersNumber];
                if (status.FinalStatus != FinalStatus.Present)
                    status.FinalStatus = FinalStatus.NotPresent;
            }
        }

        private void SetupInitialStatus()
        {
            PlaceStatus = new Dictionary<Place, Status[]>();
            PlaceStatus[Place.Bath] = GetInitialStatusArray(_playersNumber);
            PlaceStatus[Place.Bedroom] = GetInitialStatusArray(_playersNumber);
            PlaceStatus[Place.Cabinet] = GetInitialStatusArray(_playersNumber);
            PlaceStatus[Place.DinnerRoom] = GetInitialStatusArray(_playersNumber);
            PlaceStatus[Place.Garage] = GetInitialStatusArray(_playersNumber);
            PlaceStatus[Place.InnerGarden] = GetInitialStatusArray(_playersNumber);
            PlaceStatus[Place.LivingRoom] = GetInitialStatusArray(_playersNumber);
            PlaceStatus[Place.Kitchen] = GetInitialStatusArray(_playersNumber);
            PlaceStatus[Place.Poolhall] = GetInitialStatusArray(_playersNumber);

            SlayerStatus = new Dictionary<Slayer, Status[]>();
            SlayerStatus[Slayer.Green] = GetInitialStatusArray(_playersNumber);
            SlayerStatus[Slayer.Mustard] = GetInitialStatusArray(_playersNumber);
            SlayerStatus[Slayer.Peacock] = GetInitialStatusArray(_playersNumber);
            SlayerStatus[Slayer.Plum] = GetInitialStatusArray(_playersNumber);
            SlayerStatus[Slayer.Scarlet] = GetInitialStatusArray(_playersNumber);
            SlayerStatus[Slayer.White] = GetInitialStatusArray(_playersNumber);

            WeaponStatus = new Dictionary<Weapon, Status[]>();
            WeaponStatus[Weapon.Candle] = GetInitialStatusArray(_playersNumber);
            WeaponStatus[Weapon.Gun] = GetInitialStatusArray(_playersNumber);
            WeaponStatus[Weapon.Knife] = GetInitialStatusArray(_playersNumber);
            WeaponStatus[Weapon.Rope] = GetInitialStatusArray(_playersNumber);
            WeaponStatus[Weapon.Spanner] = GetInitialStatusArray(_playersNumber);
            WeaponStatus[Weapon.SteelPipe] = GetInitialStatusArray(_playersNumber);
        }

        private Status[] GetInitialStatusArray(int playerNumber)
        {
            var result = new Status[playerNumber];

            for (var i = 0; i < playerNumber; i++)
                result[i] = new Status() {FinalStatus = FinalStatus.Unknown, PartOf = new List<Setup>()};
            return result;
        }

        private void RemoveCardsFromSetup<T>(List<T> cards, Dictionary<T, Status[]> statusOf, int player)
        {
            foreach (var card in cards)
            {
                var line = statusOf[card];
                foreach (var status in line)
                {
                    status.FinalStatus = FinalStatus.NotPresent;
                }
                if (player != -1)
                    line[player].FinalStatus = FinalStatus.Present;
            }
        }

        private void SetNotPresentCard(Setup setup, int player)
        {
            WeaponStatus[setup.Weapon][player].FinalStatus = FinalStatus.NotPresent;
            PlaceStatus[setup.Place][player].FinalStatus = FinalStatus.NotPresent;
            SlayerStatus[setup.Slayer][player].FinalStatus = FinalStatus.NotPresent;
        }

        private void SetPresentSetup(Setup setup, int player)
        {
            WeaponStatus[setup.Weapon][player].UpdateStatus(FinalStatus.PartOfSetup);
            WeaponStatus[setup.Weapon][player].PartOf.Add(setup);

            PlaceStatus[setup.Place][player].UpdateStatus(FinalStatus.PartOfSetup);
            PlaceStatus[setup.Place][player].PartOf.Add(setup);

            SlayerStatus[setup.Slayer][player].UpdateStatus(FinalStatus.PartOfSetup);
            SlayerStatus[setup.Slayer][player].PartOf.Add(setup);

            TrackedSetups.Add((setup, player));
        }

        private static bool DeductResolvedSetup(Status[] setupStatus)
        {
            if (setupStatus.Count(s => s.FinalStatus == FinalStatus.NotPresent) == 2)
            {
                var setupToChange = setupStatus.Single(s => s.FinalStatus != FinalStatus.NotPresent);
                setupToChange.FinalStatus = FinalStatus.Present;
                return true;
            }
            return false;
        }

        public List<(Setup setup, int player)> TrackedSetups { get; } = new List<(Setup setup, int player)>();

        private Status[] GetSetupStatus(Setup setup, int player)
        {
            var setupStatus = new Status[3];
            setupStatus[0] = WeaponStatus[setup.Weapon][player];
            setupStatus[1] = PlaceStatus[setup.Place][player];
            setupStatus[2] = SlayerStatus[setup.Slayer][player];
            return setupStatus;
        }

        private void RemoveCardsFromPotentialSetup(Cards cards, int player)
        {
            PotentialWeapons.RemoveAll(cards.Weapons.Contains);
            PotentialSlayers.RemoveAll(cards.Persons.Contains);
            PotentialPlaces.RemoveAll(cards.Places.Contains);

            RemoveCardsFromSetup(cards.Weapons, WeaponStatus, player);
            RemoveCardsFromSetup(cards.Places, PlaceStatus, player);
            RemoveCardsFromSetup(cards.Persons, SlayerStatus, player);


            NotifyPropertyChanged(nameof(PersonTableStatus));
            NotifyPropertyChanged(nameof(WeaponTableStatus));
            NotifyPropertyChanged(nameof(PlaceTableStatus));

            NotifyPropertyChanged(nameof(PotentialCards));

            if (PotentialWeapons.Count == 0)
                throw new Exception("weapon error");
            if (PotentialSlayers.Count == 0)
                throw new Exception("slayer error");
            if (PotentialPlaces.Count == 0)
                throw new Exception("place error");
        }


        public List<PlayersStatus> PlaceTableStatus
        {
            get
            {
                var result = new List<PlayersStatus>();
                foreach (var placeStatus in PlaceStatus)
                {
                    var line = new PlayersStatus();
                    line.Name = placeStatus.Key.ToString();
                    if (placeStatus.Value.Length > 0)
                        line.Player1 = placeStatus.Value[0];
                    if (placeStatus.Value.Length > 1)
                        line.Player2 = placeStatus.Value[1];
                    if (placeStatus.Value.Length > 2)
                        line.Player3 = placeStatus.Value[2];
                    if (placeStatus.Value.Length > 3)
                        line.Player4 = placeStatus.Value[3];
                    if (placeStatus.Value.Length > 4)
                        line.Player5 = placeStatus.Value[4];

                    result.Add(line);
                }
                return result;
            }
        }

        public List<PlayersStatus> WeaponTableStatus
        {
            get
            {
                var result = new List<PlayersStatus>();
                foreach (var placeStatus in WeaponStatus)
                {
                    var line = new PlayersStatus();
                    line.Name = placeStatus.Key.ToString();
                    if (placeStatus.Value.Length > 0)
                        line.Player1 = placeStatus.Value[0];
                    if (placeStatus.Value.Length > 1)
                        line.Player2 = placeStatus.Value[1];
                    if (placeStatus.Value.Length > 2)
                        line.Player3 = placeStatus.Value[2];
                    if (placeStatus.Value.Length > 3)
                        line.Player4 = placeStatus.Value[3];
                    if (placeStatus.Value.Length > 4)
                        line.Player5 = placeStatus.Value[4];

                    result.Add(line);
                }
                return result;
            }
        }

        public List<PlayersStatus> PersonTableStatus
        {
            get
            {
                var result = new List<PlayersStatus>();
                foreach (var placeStatus in SlayerStatus)
                {
                    var line = new PlayersStatus();
                    line.Name = placeStatus.Key.ToString();
                    if (placeStatus.Value.Length > 0)
                        line.Player1 = placeStatus.Value[0];
                    if (placeStatus.Value.Length > 1)
                        line.Player2 = placeStatus.Value[1];
                    if (placeStatus.Value.Length > 2)
                        line.Player3 = placeStatus.Value[2];
                    if (placeStatus.Value.Length > 3)
                        line.Player4 = placeStatus.Value[3];
                    if (placeStatus.Value.Length > 4)
                        line.Player5 = placeStatus.Value[4];

                    result.Add(line);
                }
                return result;
            }
        }

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

        public void RegisterKnownCard(SingleCard help, int player)
        {
            RemoveCardsFromPotentialSetup(help, player);
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
            SetNotPresentCard(askedSetup, noHelper);
        }

        public void RegisterHelpedWithSetup(Setup askedSetup, int i)
        {
            SetPresentSetup(askedSetup, i);
        }

        public void RegisterAskedSetup(Setup askedSetup, int currentPlayer)
        {
            if (!AskedSetups.ContainsKey(currentPlayer))
                AskedSetups[currentPlayer] = new List<Setup>();
            AskedSetups[currentPlayer].Add(askedSetup);
            if (currentPlayer == _selfNumber)
                NotifyPropertyChanged(nameof(OwnQuestions));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void DoDeduction()
        {
            var removeTrackedSetup = new List<(Setup, int)>();
            foreach ((var setup, var player) in TrackedSetups)
            {
                var setupStatus = GetSetupStatus(setup, player);
                if (setupStatus.Any(s => s.FinalStatus == FinalStatus.Present))
                {
                    removeTrackedSetup.Add((setup, player));
                    continue;
                }
                if (DeductResolvedSetup(setupStatus))
                {
                    removeTrackedSetup.Add((setup, player));
                    continue;
                }
            }

            if (removeTrackedSetup.Any())
            {
                TrackedSetups.RemoveAll(removeTrackedSetup.Contains);
                DoDeduction();
                return;
            }

            foreach (var line in GeneralStatus.Where(l=>l.Any(s => s.FinalStatus ==FinalStatus.Present))
                                              .Where(l=>l.Any(s=> s.FinalStatus != FinalStatus.NotPresent &&
                                                                  s.FinalStatus != FinalStatus.Present)))
            {
                Array.ForEach(line, s => {if (s.FinalStatus != FinalStatus.Present) s.FinalStatus = FinalStatus.NotPresent;});
                DoDeduction();
                return;
            }

            for (var i = 0; i < _playersNumber; i++)
            {
                var column = GeneralStatus.Select(s => s[i]).ToArray();
                if ((column.Count(s => s.FinalStatus == FinalStatus.Present) == CardsCount) && 
                    column.Any(l => l.FinalStatus != FinalStatus.NotPresent &&
                                    l.FinalStatus != FinalStatus.Present))
                {
                    Array.ForEach(column, s => { if (s.FinalStatus != FinalStatus.Present) s.FinalStatus = FinalStatus.NotPresent; });
                    DoDeduction();
                    return;
                }
            }

            NotifyPropertyChanged(nameof(PersonTableStatus));
            NotifyPropertyChanged(nameof(WeaponTableStatus));
            NotifyPropertyChanged(nameof(PlaceTableStatus));
        }
    }
}
