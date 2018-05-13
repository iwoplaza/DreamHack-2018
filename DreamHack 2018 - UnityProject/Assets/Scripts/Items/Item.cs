using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class Item
    {
        public int Identifier { get; private set; }
        public string DisplayName { get; private set; }
        public string IconName { get; private set; }

        public Item(int identifier, string displayName, string iconName)
        {
            Identifier = identifier;
            DisplayName = displayName;
            IconName = iconName;
        }

        public static Item METAL = new Item(0, "Metal", "Metal");
        public static Item[] Items = {
            METAL
        };

        public static Item Get(int identifier)
        {
            if(identifier < 0 || identifier >= Items.Length)
                return null;

            return Items[identifier];
        }
    }
}