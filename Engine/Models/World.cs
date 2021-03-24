using System.Collections.Generic;

namespace Engine.Models
{
    public class World
    {
        private List<Location> _locations = new List<Location>();
        internal void AddLocation(int XCOORD, int YCOORD, string name, string description, string imgname)
        {
            Location loc = new Location();
            loc.XCOORD = XCOORD;
            loc.YCOORD = YCOORD;
            loc.NAME = name;
            loc.DESC = description;
            loc.IMGNAME = imgname;

            _locations.Add(loc);
        }

        public Location LocationAt(int XCOORD, int YCOORD)
        {
            foreach(Location loc in _locations)
            {
                if(loc.XCOORD == XCOORD && loc.YCOORD == YCOORD)
                {
                    return loc;
                }
            }
            return null;
        }
    }
}
