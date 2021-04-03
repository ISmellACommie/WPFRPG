using System.Collections.Generic;

namespace Engine.Models
{
    public class World
    {
        private readonly List<Location> _locations = new List<Location>();
        internal void AddLocation(int _xcoord, int _ycoord, string _name, string _desc, string _imgname)
        {
            _locations.Add(new Location(_xcoord, _ycoord, _name, _desc, $"/Engine;component/Images/Locations/{_imgname}"));
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
