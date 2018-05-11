using Game.Acting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Acting.Actions;
using Game.Tasks;
using System;
using Game.Animation;

namespace Game.Enemies
{
    public class EnergyLeech : Enemy, ISubject, IFocusTarget
    {
        public override string DisplayName { get { return "Energy Leech"; } }
        public override int MaxHealth { get { return 10; } }
        Transform IFocusTarget.PortraitPivot { get { return null; } }

        public EnergyLeechVisual Visual { get; private set; }
        public bool IsWalking { get; private set; }

        override protected void Awake()
        {
            base.Awake();
            Visual = GetComponent<EnergyLeechVisual>();
        }

        List<ActionBase> ISubject.GetActionsFor(IActor actor)
        {
            List<ActionBase> actions = new List<ActionBase>();
            actions.Add(new PerformTaskAction(new AttackTask(this)));

            return actions;
        }

        override protected void Update()
        {
            if (Visual != null)
                Visual.UpdateAnimator();
        }

        public void OnFocusGained()
        {
        }

        public void OnFocusLost()
        {
        }
    }
}