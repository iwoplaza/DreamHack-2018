using System.Collections;

namespace Game
{
    public class EnemySpawnerController : EnvironmentControlComponent
    {
        public override void InitializeComponent()
        {
            WorldController worldController = WorldController.Instance;
        }

        public override void UpdateComponent()
        {
        }
    }
}