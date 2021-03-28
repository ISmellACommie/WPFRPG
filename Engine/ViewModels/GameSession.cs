using System;
using System.Linq;
using Engine.Models;
using Engine.Factories;
using Engine.EventArgs;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        #region Properties

        private Location _currentLocation;
        private Monster _currentMonster;
        public Player CurrentPlayer
        {
            get;
            set;
        }
        public World CurrentWorld
        {
            get;
            set;
        }
        public Location CurrentLocation
        {
            get
            {
                return _currentLocation;
            }
            set
            {
                _currentLocation = value;

                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToSouth));

                GivePlayerQuestsAtLocation();
                GetMonsterAtLocation();
            }
        }
        public Monster CurrentMonster
        {
            get
            {
                return _currentMonster;
            }
            set
            {
                _currentMonster = value;

                OnPropertyChanged(nameof(CurrentMonster));
                OnPropertyChanged(nameof(HasMonster));

                if(CurrentMonster != null)
                {
                    RaiseMessage("");
                    RaiseMessage($"You see a {CurrentMonster.NAME}");
                }
            }
        }
        public bool HasLocationToNorth
        {
            get
            {
                return CurrentWorld.LocationAt(CurrentLocation.XCOORD, CurrentLocation.YCOORD + 1) != null; 
            }
        }
        public bool HasLocationToEast
        {
            get
            {
                return CurrentWorld.LocationAt(CurrentLocation.XCOORD + 1, CurrentLocation.YCOORD) != null;
            }
        }
        public bool HasLocationToWest
        {
            get
            {
                return CurrentWorld.LocationAt(CurrentLocation.XCOORD - 1, CurrentLocation.YCOORD) != null;
            }
        }
        public bool HasLocationToSouth
        {
            get
            {
                return CurrentWorld.LocationAt(CurrentLocation.XCOORD, CurrentLocation.YCOORD - 1) != null;
            }
        }

        public bool HasMonster => CurrentMonster != null;

        #endregion

        public GameSession()
        {
            CurrentPlayer = new Player
            {
                NAME = "Viraaj",
                CHARCLASS = "Fighter",
                HP = 10,
                GOLD = 1000000,
                EXP = 0,
                LVL = 1
            };

            CurrentWorld = WorldFactory.CreateWorld();

            CurrentLocation = CurrentWorld.LocationAt(0, 0);

            CurrentPlayer.INV.Add(ItemFactory.CreateGameItem(1001));
            CurrentPlayer.INV.Add(ItemFactory.CreateGameItem(1002));
        }
        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCOORD, CurrentLocation.YCOORD + 1);
            }
        }

        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCOORD + 1, CurrentLocation.YCOORD);
            }
        }
        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCOORD - 1, CurrentLocation.YCOORD);
            }
        }

        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCOORD, CurrentLocation.YCOORD - 1);
            }
        }
        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.QUESTS.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.QUESTS.Add(new QuestStatus(quest));
                }
            }
        }
        private void GetMonsterAtLocation()
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }

        private void RaiseMessage(string _message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(_message));
        }
    }
}
