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

        private Player _currentPlayer;
        private Location _currentLocation;
        private Monster _currentMonster;
        private Trader _currentTrader;

        public Player CurrentPlayer 
        {
            get { return _currentPlayer; }
            set
            {
                if(_currentPlayer != null)
                {
                    _currentPlayer.ONACTIONPERFORMED -= OnCurrentPlayerPerformedAction; 
                    _currentPlayer.ONLEVELEDUP -= OnCurrentPlayerLeveledUp;
                    _currentPlayer.ONKILLED -= OnCurrentPlayerKilled;
                }

                _currentPlayer = value;

                if(_currentPlayer != null)
                {
                    _currentPlayer.ONACTIONPERFORMED += OnCurrentPlayerPerformedAction;
                    _currentPlayer.ONLEVELEDUP += OnCurrentPlayerLeveledUp;
                    _currentPlayer.ONKILLED += OnCurrentPlayerKilled;
                }
            }
        }
        public World CurrentWorld { get; }
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToSouth));

                CompleteQuestsAtLocation();
                GivePlayerQuestsAtLocation();
                GetMonsterAtLocation();

                CurrentTrader = CurrentLocation.TRADERHERE;
            }
        }
        public Monster CurrentMonster
        {
            get { return _currentMonster; }
            set
            {
                if(_currentMonster != null)
                {
                    _currentMonster.ONKILLED -= OnCurrentMonsterKilled;
                }

                _currentMonster = value;

                if(_currentMonster != null)
                {
                    _currentMonster.ONKILLED += OnCurrentMonsterKilled;

                    RaiseMessage("");
                    RaiseMessage($"You see a {CurrentMonster.NAME} here!");
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMonster));
            }
        }
        public Trader CurrentTrader
        {
            get { return _currentTrader; }
            set
            {
                _currentTrader = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasTrader));
            }
        }
        public bool HasLocationToNorth => CurrentWorld.LocationAt(CurrentLocation.XCOORD, CurrentLocation.YCOORD + 1) != null; 
        public bool HasLocationToEast => CurrentWorld.LocationAt(CurrentLocation.XCOORD + 1, CurrentLocation.YCOORD) != null;
        public bool HasLocationToWest => CurrentWorld.LocationAt(CurrentLocation.XCOORD - 1, CurrentLocation.YCOORD) != null;
        public bool HasLocationToSouth => CurrentWorld.LocationAt(CurrentLocation.XCOORD, CurrentLocation.YCOORD - 1) != null;
        public bool HasMonster => CurrentMonster != null;
        public bool HasTrader => CurrentTrader != null;
       
        public GameSession()
        {
            CurrentPlayer = new Player("Viraaj", "Fighter", 0, 10, 10, 1000000);

            if (!CurrentPlayer.WEAPONS.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            }

            CurrentWorld = WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(0, 0);
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

        private void CompleteQuestsAtLocation()
        {
            foreach(Quest quest in CurrentLocation.QUESTSAVAILABLEHERE)
            {
                QuestStatus questToComplete = CurrentPlayer.QUESTS.FirstOrDefault(q => q.PLAYERQUEST.ID == quest.ID && !q.ISCOMPLETED);

                if(questToComplete != null)
                {
                    if (CurrentPlayer.HasAllTheseItems(quest.ITEMSTOCOMPLETE))
                    {
                        foreach(ItemQuantity itemquantity in quest.ITEMSTOCOMPLETE)
                        {
                            for(int i = 0; i < itemquantity.QUANTITY; i++)
                            {
                                CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.INV.First(item => item.ITEMTYPEID == itemquantity.ITEMID));
                            }
                        }

                        RaiseMessage("");
                        RaiseMessage($"You completed '{quest.NAME}' quest.");

                        //give the player the quest rewards
                        RaiseMessage($"You receive {quest.REWARDEXP} experience points.");
                        CurrentPlayer.AddExperience(quest.REWARDEXP);

                        RaiseMessage($"You receive {quest.REWARDGOLD} gold.");
                        CurrentPlayer.ReceiveGold(quest.REWARDGOLD);

                        foreach (ItemQuantity itemquantity in quest.REWARDITEMS)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemquantity.ITEMID);

                            CurrentPlayer.AddItemToInventory(rewardItem);
                            RaiseMessage($"You receive a {rewardItem.NAME}");
                        }

                        //mark the quest as completed
                        questToComplete.ISCOMPLETED = true;
                    }
                }
            }
        }

        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QUESTSAVAILABLEHERE)
            {
                if (!CurrentPlayer.QUESTS.Any(q => q.PLAYERQUEST.ID == quest.ID))
                {
                    CurrentPlayer.QUESTS.Add(new QuestStatus(quest));

                    RaiseMessage("");
                    RaiseMessage($"You receive the '{quest.NAME}' quest.");
                    RaiseMessage(quest.DESC);

                    RaiseMessage("Return with:");
                    foreach(ItemQuantity itemQuantity in quest.ITEMSTOCOMPLETE)
                    {
                        RaiseMessage($"     {itemQuantity.QUANTITY} {ItemFactory.CreateGameItem(itemQuantity.ITEMID).NAME}");
                    }

                    RaiseMessage("And you wil receive:");
                    RaiseMessage($"     {quest.REWARDEXP} experience points");
                    RaiseMessage($"     {quest.REWARDGOLD} gold");
                    foreach(ItemQuantity itemQuantity in quest.REWARDITEMS)
                    {
                        RaiseMessage($"     {itemQuantity.QUANTITY} {ItemFactory.CreateGameItem(itemQuantity.ITEMID).NAME}");
                    }
                }
            }
        }

        private void GetMonsterAtLocation()
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }

        public void AttackCurrentMonster()
        {
            if(CurrentPlayer.CURRENTWEAPON == null)
            {
                RaiseMessage("You must select a weapon to attack.");
                return;
            }

            CurrentPlayer.UseCurrentWeaponOn(CurrentMonster);

            if (CurrentMonster.ISDEAD)
            {
                GetMonsterAtLocation();
            }
            else
            {
                int dmgToPlayer = RandomNumberGenerator.NumberBetween(CurrentMonster.MINDMG, CurrentMonster.MAXDMG);

                if(dmgToPlayer == 0)
                {
                    RaiseMessage($"The {CurrentMonster.NAME} attacks you for no damage.");
                }
                else
                {
                    RaiseMessage($"The {CurrentMonster.NAME} hit you for {dmgToPlayer} points.");
                    CurrentPlayer.TakeDamage(dmgToPlayer);
                }
            }
        }

        private void OnCurrentPlayerKilled(object sender, System.EventArgs e)
        {
            RaiseMessage("");
            RaiseMessage($"You have been killed.");

            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            CurrentPlayer.CompletelyHeal();
        }

        private void OnCurrentMonsterKilled(object sender, System.EventArgs e)
        {
            RaiseMessage("");
            RaiseMessage($"You defeated the {CurrentMonster.NAME}!");

            RaiseMessage($"You receive {CurrentMonster.REWARDEXP} experience points");
            CurrentPlayer.AddExperience(CurrentMonster.GOLD);

            RaiseMessage($"You receive {CurrentMonster.GOLD} gold.");
            CurrentPlayer.ReceiveGold(CurrentMonster.GOLD);

            foreach (GameItem gameitem in CurrentMonster.INV)
            {
                CurrentPlayer.AddItemToInventory(gameitem);
                RaiseMessage($"You receive one {gameitem.NAME}");
            }
        }

        private void OnCurrentPlayerLeveledUp(object sender, System.EventArgs e)
        {
            RaiseMessage($"You are now level {CurrentPlayer.LVL}!");
        }

        private void RaiseMessage(string _message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(_message));
        }

        private void OnCurrentPlayerPerformedAction(object sender, string result)
        {
            RaiseMessage(result);
        }
    }
}
