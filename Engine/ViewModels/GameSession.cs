﻿using System;
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
                OnPropertyChanged(nameof(HASLOCATIONTONORTH));
                OnPropertyChanged(nameof(HASLOCATIONTOEAST));
                OnPropertyChanged(nameof(HASLOCATIONTOWEST));
                OnPropertyChanged(nameof(HASLOCATIONTOSOUTH));

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
                    _currentMonster.ONACTIONPERFORMED -= OnCurrentMonsterPerformedAction;
                    _currentMonster.ONKILLED -= OnCurrentMonsterKilled;
                }

                _currentMonster = value;

                if(_currentMonster != null)
                {
                    _currentMonster.ONACTIONPERFORMED += OnCurrentMonsterPerformedAction;
                    _currentMonster.ONKILLED += OnCurrentMonsterKilled;

                    RaiseMessage("");
                    RaiseMessage($"You see a {CurrentMonster.NAME} here!");
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(HASMONSTER));
            }
        }
        public Trader CurrentTrader
        {
            get { return _currentTrader; }
            set
            {
                _currentTrader = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HASTRADER));
            }
        }
        public bool HASLOCATIONTONORTH => CurrentWorld.LocationAt(CurrentLocation.XCOORD, CurrentLocation.YCOORD + 1) != null; 
        public bool HASLOCATIONTOEAST => CurrentWorld.LocationAt(CurrentLocation.XCOORD + 1, CurrentLocation.YCOORD) != null;
        public bool HASLOCATIONTOWEST => CurrentWorld.LocationAt(CurrentLocation.XCOORD - 1, CurrentLocation.YCOORD) != null;
        public bool HASLOCATIONTOSOUTH => CurrentWorld.LocationAt(CurrentLocation.XCOORD, CurrentLocation.YCOORD - 1) != null;
        public bool HASMONSTER => CurrentMonster != null;
        public bool HASTRADER => CurrentTrader != null;
       
        public GameSession()
        {
            CurrentPlayer = new Player("Viraaj", "Fighter", 0, 10, 10, 1000000);

            if (!CurrentPlayer.INV.WEAPONS.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            }

            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2001));
            CurrentPlayer.LearnRecipe(RecipeFactory.RecipeByID(1));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3001));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3003));

            CurrentWorld = WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(0, 0);
        }
        public void MoveNorth()
        {
            if (HASLOCATIONTONORTH)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCOORD, CurrentLocation.YCOORD + 1);
            }
        }

        public void MoveEast()
        {
            if (HASLOCATIONTOEAST)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCOORD + 1, CurrentLocation.YCOORD);
            }
        }
        public void MoveWest()
        {
            if (HASLOCATIONTOWEST)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCOORD - 1, CurrentLocation.YCOORD);
            }
        }

        public void MoveSouth()
        {
            if (HASLOCATIONTOSOUTH)
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
                    if (CurrentPlayer.INV.HasAllTheseItems(quest.ITEMSTOCOMPLETE))
                    {
                        CurrentPlayer.RemoveItemsFromInventory(quest.ITEMSTOCOMPLETE);

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
            if(CurrentMonster == null)
            {
                return;
            }

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
                CurrentMonster.UseCurrentWeaponOn(CurrentPlayer);
            }
        }

        public void UseCurrentConsumable()
        {
            if (CurrentPlayer.CURRENTCONSUMABLE != null)
            {
                CurrentPlayer.UseCurrentConsumable();
            }
        }

        public void CraftItemUsing(Recipe _recipe)
        {
            if (CurrentPlayer.INV.HasAllTheseItems(_recipe.INGREDIENTS))
            {
                CurrentPlayer.RemoveItemsFromInventory(_recipe.INGREDIENTS);

                foreach (ItemQuantity _itemquantity in _recipe.OUTPUTITEMS)
                {
                    for (int i = 0; i < _itemquantity.QUANTITY; i++)
                    {
                        GameItem _outputitem = ItemFactory.CreateGameItem(_itemquantity.ITEMID);
                        CurrentPlayer.AddItemToInventory(_outputitem);
                        RaiseMessage($"You craft 1 {_outputitem.NAME}");
                    }
                }
            }
            else
            {
                RaiseMessage("You do not have the required ingredients:");
                foreach(ItemQuantity _itemquantity in _recipe.INGREDIENTS)
                {
                    RaiseMessage($"     {_itemquantity.QUANTITY} {ItemFactory.ItemName(_itemquantity.ITEMID)}");
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

            foreach (GameItem gameitem in CurrentMonster.INV.ITEMS)
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

        private void OnCurrentMonsterPerformedAction(object sender, string result)
        {
            RaiseMessage(result);
        }
    }
}
