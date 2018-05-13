using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.TileObjects;

namespace Game.Tasks
{
    public class BuildTask : TaskBase
    {
        public TileObjectBase ObjectToBuild { get; private set; }
        public int TargetX { get; private set; }
        public int TargetY { get; private set; }

        public BuildTask(TileObjectBase objectToBuild, int targetX, int targetY)
        {
            ObjectToBuild = objectToBuild;
            TargetX = targetX;
            TargetY = targetY;
        }

        public override string ActionQuickie { get { return "Build"; } }

        public override string DisplayName
        {
            get
            {
                return "Build " + ObjectToBuild.DisplayName;
            }
        }

        public override string Description
        {
            get
            {
                return "This worker will build " + ObjectToBuild.DisplayName +
                       " at (" + TargetX + ", " + TargetY + ")";
            }
        }
    }
}