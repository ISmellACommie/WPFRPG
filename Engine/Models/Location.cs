using System.Collections.Generic;

namespace Engine.Models
{
    public class Location
    {
        public int XCOORD
        {
            get;
            set;
        }
        public int YCOORD
        {
            get;
            set;
        }
        public string NAME
        {
            get;
            set;
        }
        public string DESC
        {
            get;
            set;
        }
        public string IMGNAME
        {
            get;
            set;
        }
        public List<Quest> QuestsAvailableHere
        {
            get;
            set;
        } = new List<Quest>();
        public List<MonsterEncounter> MonstersHere
        {
            get;
            set;
        } = new List<MonsterEncounter>();

        public void AddMonster(int _monsterid, int _encounterchance)
        {
            if (MonstersHere.Exists(m => m.MONSTERID == _monsterid))
            {
                //This monster has already been added to this location
                //So overwrite the chance with the new number
                MonstersHere.First(m => m.MONSTERID = _monsterid)
                            .ENCOUNTERCHANCE = _encounterchance;
            }
            else
            {
                //This monster is not already at this location so add it
                MonstersHere.Add(new MonsterEncounter(_monsterid, _encounterchance));
            }

        }
    }
}
