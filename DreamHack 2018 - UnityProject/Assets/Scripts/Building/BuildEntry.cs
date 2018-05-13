﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Building
{
    public class BuildEntry
    {
        public string DisplayName { get; private set; }
        public Type PropType { get; private set; }

        public BuildEntry(string displayName, Type propType)
        {
            DisplayName = displayName;
            PropType = propType;
        }
    }
}