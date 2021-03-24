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
    }
}
