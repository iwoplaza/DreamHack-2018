using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [System.Serializable]
    public class TileMapData
    {
        public int width;
        public int height;
        public List<Tile> tiles;

        public TileMapData()
        {
            tiles = new List<Tile>();
        }
    }
}