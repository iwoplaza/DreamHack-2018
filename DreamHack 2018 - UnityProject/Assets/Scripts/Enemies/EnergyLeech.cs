using Game.Acting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Acting.Actions;
using Game.Tasks;
using System;

namespace Game.Enemies
{
    public class EnergyLeech : Enemy, ISubject
    {
        public override string DisplayName { get { return "Energy Leech"; } }
        public override int MaxHealth { get { return 10; } }

        public bool IsWalking { get; private set; }

        List<ActionBase> ISubject.GetActionsFor(IActor actor)
        {
            List<ActionBase> actions = new List<ActionBase>();
            actions.Add(new PerformTaskAction(new AttackTask(this)));

            return actions;
        }
    }
}