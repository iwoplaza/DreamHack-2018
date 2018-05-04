using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TilePosition
    {
        public int X { get; set; }
        public int Y { get; set; }

        public TilePosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}