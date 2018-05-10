using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tasks
{
    public class AttackTask : TaskBase
    {
        float m_timeToComplete;

        public bool IsComplete { get { return m_timeToComplete <= 0; } }

        public AttackTask()
        {
            m_timeToComplete = 2.0F; // 2 seconds
        }

        public override string Description { get { return ""; } }
        public override string DisplayName { get { return "Attack"; } }

        public override void OnUpdate()
        {
            base.OnUpdate();

            m_timeToComplete -= Time.deltaTime;
        }
    }
}