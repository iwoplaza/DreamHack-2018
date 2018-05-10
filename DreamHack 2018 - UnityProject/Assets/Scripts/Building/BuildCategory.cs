using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Building
{
    public class BuildCategory
    {
        public string DisplayName { get; private set; }
        public string IconName { get; private set; }

        public List<BuildEntry> Entries { get; private set; }

        public BuildCategory(string displayName, string iconName)
        {
            DisplayName = displayName;
            IconName = iconName;
            Entries = new List<BuildEntry>();
        }

        public BuildCategory Add(BuildEntry entry)
        {
            Entries.Add(entry);
            return this;
        }
    }
}