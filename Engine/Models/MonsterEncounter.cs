
namespace Engine.Models
{
    public class MonsterEncounter
    {
        public int MONSTERID
        {
            get;
            set;
        }
        public int ENCOUNTERCHANCE
        {
            get;
            set;
        }

        public MonsterEncounter(int _monsterid, int _encounterchance)
        {
            MONSTERID = _monsterid;
            ENCOUNTERCHANCE = _encounterchance;
        }
    }
}
