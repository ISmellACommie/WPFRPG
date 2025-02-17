﻿using System.Collections.Generic;
using System.Linq;
using Engine.Factories;

namespace Engine.Models
{
    public class Location
    {
        public int XCOORD { get; }
        public int YCOORD { get; }
        public string NAME { get; }
        public string DESC { get; }
        public string IMGNAME { get; }
        public List<Quest> QUESTSAVAILABLEHERE { get; } = new List<Quest>();
        public List<MonsterEncounter> MONSTERSHERE { get; } = new List<MonsterEncounter>();
        public Trader TRADERHERE { get; set; }

        public Location(int _xcoord, int _ycoord, string _name, string _desc, string _imgname)
        {
            XCOORD = _xcoord;
            YCOORD = _ycoord;
            NAME = _name;
            DESC = _desc;
            IMGNAME = _imgname;
        }

        public void AddMonster(int _monsterid, int _encounterchance)
        {
            if (MONSTERSHERE.Exists(m => m.MONSTERID == _monsterid))
            {
                //This monster has already been added to this location
                //So overwrite the chance with the new number
                MONSTERSHERE.First(m => m.MONSTERID == _monsterid)
                            .ENCOUNTERCHANCE = _encounterchance;
            }
            else
            {
                //This monster is not already at this location so add it
                MONSTERSHERE.Add(new MonsterEncounter(_monsterid, _encounterchance));
            }

        }
        
        public Monster GetMonster()
        {
            if (!MONSTERSHERE.Any())
            {
                return null;
            }

            //total the percentages of all monsters at this location
            int totalChances = MONSTERSHERE.Sum(m => m.ENCOUNTERCHANCE);

            //select a random numbet between 1 and the total
            int randomNumber = RandomNumberGenerator.NumberBetween(1, totalChances);

            //loop through the monster list
            //adding the monster's percentage chance of appearing to the runningTotal var
            //when randomNumber is lower than runningTotal
            //that is the monster to return
            int runningTotal = 0;

            foreach(MonsterEncounter monsterEncounter in MONSTERSHERE)
            {
                runningTotal += monsterEncounter.ENCOUNTERCHANCE;

                if(randomNumber <= runningTotal)
                {
                    return MonsterFactory.GetMonster(monsterEncounter.MONSTERID);
                }
            }

            //if there was a problem, return the last monster in the list.
            return MonsterFactory.GetMonster(MONSTERSHERE.Last().MONSTERID);
        }
    }
}
