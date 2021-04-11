using System.Collections.Generic;

namespace Engine.Models
{
    public class World
    {
        private readonly List<Location> _locations = new List<Location>();
        internal void AddLocation(Location location)
        {
            _locations.Add(location);
        }

        public Location LocationAt(int _xcoord, int _ycoord)
        {
            foreach(Location loc in _locations)
            {
                if(loc.XCOORD == _xcoord && loc.YCOORD == _ycoord)
                {
                    return loc;
                }
            }
            return null;
        }
    }
}
