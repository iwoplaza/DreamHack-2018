using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tasks
{
    public class GoToTask : TaskBase
    {
        public TilePosition TargetPosition { get; private set; }

        public override string DisplayName { get { return "Walk"; } }
        public override string Description { get { return ""; } }

        public GoToTask(TilePosition targetPosition)
        {
            TargetPosition = targetPosition;
        }
    }
}