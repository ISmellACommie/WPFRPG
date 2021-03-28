
namespace Engine.EventArgs
{
    public class GameMessageEventArgs : System.EventArgs
    {
        public string MESSAGE
        {
            get;
            private set;
        }

        public GameMessageEventArgs(string _message)
        {
            MESSAGE = _message;
        }
    }
}
