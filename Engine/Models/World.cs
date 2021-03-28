using System.Collections.Generic;

namespace Engine.Models
{
    public class World
    {
        private List<Location> _locations = new List<Location>();
        internal void AddLocation(int _xcoord, int _ycoord, string _name, string _desc, string _imgname)
        {
            Location loc = new Location();
            loc.XCOORD = _xcoord;
            loc.YCOORD = _ycoord;
            loc.NAME = _name;
            loc.DESC = _desc;
            loc.IMGNAME = $"/Engine;component/Images/Locations/{_imgname}";

            _locations.Add(loc);
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
