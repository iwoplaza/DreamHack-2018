using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.TileObjects;

namespace Game
{
    public class TileMap
    {
        protected Tile[,] m_tiles;

        protected GameObject m_mapObject;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public TileMap(int width, int height)
        {
            Width = width;
            Height = height;
            m_tiles = new Tile[Width, Height];

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    m_tiles[x, y] = new Tile(x, y);
                }
            }
        }

        public TileMap(Data.TileMapData data)
        {
            Width = data.width;
            Height = data.height;
            m_tiles = new Tile[Width, Height];

            foreach(Tile tile in data.tiles)
            {
                m_tiles[tile.Position.X, tile.Position.Y] = tile;
            }
        }

        /// <summary>
        /// Returns the state of this TileMap in pure data form, ready to be serialized.
        /// </summary>
        /// <returns></returns>
        public Data.TileMapData GetAsData()
        {
            var data = new Data.TileMapData();
            data.width = Width;
            data.height = Height;

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    if(m_tiles[x, y] != null)
                    {
                        data.tiles.Add(m_tiles[x, y]);
                    }
                }
            }

            return data;
        }

        public GameObject CreateGameObject()
        {
            if(m_mapObject != null)
            {
                Object.Destroy(m_mapObject);
            }

            m_mapObject = new GameObject("TileMap");
            m_mapObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            for(int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
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

        public Tile TileAt(TilePosition position)
        {
            if (position.X < 0 || position.X >= Width ||
                position.Y < 0 || position.Y >= Height)
                return null;

            return m_tiles[position.X, position.Y];
        }

        public bool InstallAt(TileObjectBase objectToInstall, TilePosition targetPosition)
        {
            Tile targetTile = TileAt(targetPosition);
            if (targetTile == null)
                return false;

            targetTile.Install(objectToInstall);
            return true;
        }
    }
}