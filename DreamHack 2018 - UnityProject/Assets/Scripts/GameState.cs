using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class GameState
    {
        public TileMap TileMap { get; set; }

        /// <summary>
        /// Used only right before serialization.
        /// </summary>
        [SerializeField] protected Data.TileMapData m_tileMapData;

        public GameState()
        {
            TileMap = new TileMap(10, 10);
        }

        public void PrepareForSerialization()
        {
            m_tileMapData = TileMap.GetAsData();
        }
    }
}