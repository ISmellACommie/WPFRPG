namespace Engine.Models
{
    public class ItemPercentage
    {
        public int ID { get; }
        public int PERCENTAGE { get; }

        public ItemPercentage(int _id, int _percentage)
        {
            ID = _id;
            PERCENTAGE = _percentage;
        }
    }
}
