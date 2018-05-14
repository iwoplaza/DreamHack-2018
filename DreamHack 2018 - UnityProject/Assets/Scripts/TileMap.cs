using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.TileObjects;
using System.Xml.Linq;
using System;
using Game.Scene;

namespace Game
{
    public class TileMap
    {
        protected Tile[,] m_tiles;

        public delegate void TileMapEventHandler(TilePosition modifiedPos);

        private TileMapEventHandler m_TileMapModifiedHandler;

        /// <summary>
        /// The representation of this TileMap in the Scene.
        /// </summary>
        public TileMapComponent Component { get; private set; }

        public int Width { get; private set; }
        public int Length { get; private set; }

        public TileMap(int width, int height)
        {
            Width = width;
            Length = height;
            m_tiles = new Tile[Width, Length];

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Length; ++y)
                {
                    m_tiles[x, y] = new Tile(this, x, y);
                }
            }
        }

        public void Parse(XElement element)
        {
            if (element == null)
                return;

            XAttribute widthAttrib = element.Attribute("width");
            XAttribute heightAttrib = element.Attribute("height");

            if (widthAttrib != null)
                Width = Int32.Parse(widthAttrib.Value);
            if (heightAttrib != null)
                Length = Int32.Parse(heightAttrib.Value);

            m_tiles = new Tile[Width, Length];
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Length; ++y)
                {
                    m_tiles[x, y] = new Tile(this, x, y);
                }
            }

            IEnumerable<XElement> tileElements = element.Elements("Tile");
            foreach(XElement tileElement in tileElements)
            {
                Tile tile = Tile.CreateAndParse(tileElement, this);
                if (tile != null)
                    m_tiles[tile.Position.X, tile.Position.Z] = tile;
            }

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Length; ++y)
                {
                    if (m_tiles[x, y] != null)
                        m_tiles[x, y].AfterParse();
                }
            }
        }

        public void Populate(XElement element)
        {
            element.SetAttributeValue("width", Width);
            element.SetAttributeValue("height", Length);

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Length; ++y)
                {
                    if (m_tiles[x, y] != null && m_tiles[x, y].IsWorthSaving)
                    {
                        XElement tileElement = new XElement("Tile");
                        element.Add(tileElement);
                        m_tiles[x, y].Populate(tileElement);
                    }
                }
            }
        }

        public TileMapComponent CreateMapComponent()
        {
            if(Component != null)
            {
                UnityEngine.Object.Destroy(Component.gameObject);
            }

            GameObject mapObject = new GameObject("TileMap");
            mapObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            Component = mapObject.AddComponent<TileMapComponent>();
            Component.Setup(this);

            return Component;
        }

        public Tile TileAt(TilePosition position)
        {
            if (position.X < 0 || position.X >= Width ||
                position.Z < 0 || position.Z >= Length)
                return null;

            return m_tiles[position.X, position.Z];
        }

        public bool InstallAt(TileProp propToInstall, TilePosition targetPosition)
        {
            Tile targetTile = TileAt(targetPosition);
            if (targetTile == null)
                return false;

            targetTile.InstallAsRoot(propToInstall);
            return true;
        }

        public bool CanInstallPropAtArea(PropType type, TilePosition origin, Vector2Int dimensions)
        {
            for (ushort x = 0; x < dimensions.x; ++x)
            {
                for (ushort z = 0; z < dimensions.y; ++z)
                {
                    TilePosition position = origin.GetOffset(x, z);
                    Tile tile = TileAt(position);
                    if (tile == null || !tile.CanInstall(type))
                        return false;
                }
            }
            return true;
        }

        public void RegisterEventHandler(TileMapEventHandler newListener, TileMapEvent eventType)
        {
            switch(eventType)
            {
                case TileMapEvent.TILEMAP_MODIFIED:
                    m_TileMapModifiedHandler += newListener;
                    break;                
            }
        }

        public void OnModifyEvent(TilePosition modifiedPos)
        {
            if(m_TileMapModifiedHandler != null)
                m_TileMapModifiedHandler(modifiedPos);
        }
    }
}