using Game.Tasks;

namespace Game
{
    public class Worker : Living
    {
        override public int MaxHealth { get { return 100; } }

        public TaskQueue TaskQueue { get; private set; }

        override public void Start()
        {
            base.Start();
        }

        override public void Update()
        {
            base.Update();
        }
    }
}