using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SavedGame : MonoBehaviour
    {
        public int WorldIdentifier { get; private set; }
        public string WorldName { get; private set; }

        public SavedGame(int worldIdentifier, string worldName)
        {
            WorldIdentifier = worldIdentifier;
            WorldName = worldName;
        }
    }
}