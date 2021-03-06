﻿using Game.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Acting.Actions
{
    public class PerformTaskAction : ActionBase
    {
        public TaskBase TaskToPerform { get; private set; }

        public override string DisplayName { get { return TaskToPerform.ActionQuickie; } }

        public PerformTaskAction(TaskBase taskToPerform)
        {
            TaskToPerform = taskToPerform;
        }
    }
}