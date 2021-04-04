namespace Engine.Models
{
    public class QuestStatus : BaseNotificationClass
    {
        private bool _isCompleted;

        public Quest PLAYERQUEST { get; set; }
        public bool ISCOMPLETED 
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                OnPropertyChanged();
            }
        }

        public QuestStatus(Quest quest)
        {
            PLAYERQUEST = quest;
            ISCOMPLETED = false;
        }
    }
}
