using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class TilePosition
    {
        [SerializeField] protected int m_x;
        [SerializeField] protected int m_y;

        public int X { get { return m_x; } set { m_x = value; } }
        public int Y { get { return m_y; } set { m_y = value; } }

        public TilePosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}