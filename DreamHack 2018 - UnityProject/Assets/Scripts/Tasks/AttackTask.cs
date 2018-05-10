using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tasks
{
    public class AttackTask : TaskBase
    {
        public Living Target { get; private set; }

        public bool IsComplete
        {
            get
            {
                return Target == null || !Target.Alive;
            }
        }

        public AttackTask(Living target)
        {
            Target = target;
        }

        public override string Description { get { return ""; } }
        public override string DisplayName { get { return "Attack " + Target.DisplayName; } }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}