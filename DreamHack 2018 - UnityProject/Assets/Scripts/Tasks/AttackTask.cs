using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tasks
{
    public class AttackTask : TaskBase
    {
        public IAttackable Target { get; private set; }

        public bool IsComplete
        {
            get
            {
                return Target == null || Target.IsDestroyed;
            }
        }

        public AttackTask(Living target)
        {
            Target = target;
        }

        public override string ActionQuickie { get { return "Attack"; } }
        public override string DisplayName { get { return "Attack " + Target.DisplayName; } }
        public override string Description { get { return ""; } }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}