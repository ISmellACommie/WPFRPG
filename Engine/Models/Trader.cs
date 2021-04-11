namespace Engine.Models
{
    public class Trader : LivingEntity
    {
        public int ID { get; }
        public Trader(int _id, string _name) : base(_name, 9999, 9999, 9999)
        {
            ID = _id;
        }
    }
}
