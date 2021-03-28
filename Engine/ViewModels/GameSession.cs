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
        public Weapon CurrentWeapon
        {
            get;
            set;
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

            if (!CurrentPlayer.WEAPONS.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            }

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

        public void AttackCurrentMonster()
        {
            if(CurrentWeapon == null)
            {
                RaiseMessage("You must select a weapon to attack.");
                return;
            }

            //determine damage to monster
            int dmgToMonster = RandomNumberGenerator.NumberBetween(CurrentWeapon.MINDMG, CurrentWeapon.MINDMG);

            if (dmgToMonster == 0)
            {
                RaiseMessage($"You missed the {CurrentMonster.NAME}");
            }
            else
            {
                CurrentMonster.HP -= dmgToMonster;
                RaiseMessage($"You hit the {CurrentMonster.NAME} for {dmgToMonster} points.");
            }

            //if monster is killed, collect rewards and loot
            if (CurrentMonster.HP <= 0)
            {
                RaiseMessage("");
                RaiseMessage($"You defeated the {CurrentMonster.NAME}");

                CurrentPlayer.EXP += CurrentMonster.REWARDEXP;
                RaiseMessage($"You receive {CurrentMonster.REWARDEXP} experience points.");

                CurrentPlayer.GOLD += CurrentMonster.REWARDGOLD;
                RaiseMessage($"You receive {CurrentMonster.REWARDGOLD} gold.");

                foreach (ItemQuantity _itemquantity in CurrentMonster.INV)
                {
                    GameItem item = ItemFactory.CreateGameItem(_itemquantity.ITEMID);
                    CurrentPlayer.AddItemToInventory(item);
                    RaiseMessage($"You receive {_itemquantity.QUANTITY} {item.NAME}.");
                }

                //get another monster to fight
                GetMonsterAtLocation();
            }
            else
            {
                //if monster is still alive, let the monster attack the player
                int dmgToPlayer = RandomNumberGenerator.NumberBetween(CurrentMonster.MINDMG, CurrentMonster.MAXDMG);

                if(dmgToPlayer == 0)
                {
                    RaiseMessage("The monster attacks, but misses you.");
                }
                else
                {
                    CurrentPlayer.HP -= dmgToPlayer;
                    RaiseMessage($"The {CurrentMonster.NAME} hit you for {dmgToPlayer} points.");
                }

                //if player is killed, move them back to  their home
                if(CurrentPlayer.HP <= 0)
                {
                    RaiseMessage("");
                    RaiseMessage($"The {CurrentMonster.NAME} killed you.");

                    CurrentLocation = CurrentWorld.LocationAt(0, -1);
                    CurrentPlayer.HP = CurrentPlayer.LVL * 10;
                }
            }
        }

        private void RaiseMessage(string _message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(_message));
        }
    }
}
