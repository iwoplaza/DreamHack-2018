using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.TileObjects;

namespace Game
{
    public class TileMap
    {
        protected TileData[,] m_tiles;
        protected int m_width;
        protected int m_height;
        protected GameObject m_mapObject;

        public int Width { get { return m_width; } }
        public int Height { get { return m_height; } }

        public TileMap(int width, int height)
        {
            m_width = width;
            m_height = height;
            m_tiles = new TileData[m_width, m_height];

            for (int x = 0; x < m_width; ++x)
            {
                for (int y = 0; y < m_height; ++y)
                {
                    m_tiles[x, y] = new TileData(x, y);
                }
            }
        }

        public GameObject CreateGameObject()
        {
            if(m_mapObject != null)
            {
                Object.Destroy(m_mapObject);
            }

            m_mapObject = new GameObject("TileMap");
            m_mapObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            for(int x = 0; x < m_width; ++x)
            {
                for (int y = 0; y < m_height; ++y)
                {
                    if (m_tiles[x, y] != null)
                    {
                        GameObject tileObject = Object.Instantiate(WorldController.Instance.TilePrefab, m_mapObject.transform);
                        tileObject.name = "Tile (" + x + "," + y + ")";
                        tileObject.transform.SetParent(m_mapObject.transform);
                        tileObject.transform.SetPositionAndRotation(new Vector3(x, 0, y), Quaternion.identity);
                    }
                }
            }

            return m_mapObject;
        }

        public TileData TileAt(TilePosition position)
        {
            if (position.X < 0 || position.X >= Width ||
                position.Y < 0 || position.Y >= Height)
                return null;

            return m_tiles[position.X, position.Y];
        }

        public bool InstallAt(TileObjectBase objectToInstall, TilePosition targetPosition)
        {
            TileData targetTile = TileAt(targetPosition);
            if (targetTile == null)
                return false;

            targetTile.Install(objectToInstall);
            return true;
        }
    }
}