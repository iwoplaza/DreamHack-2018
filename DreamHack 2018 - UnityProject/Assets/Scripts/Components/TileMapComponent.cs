using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components
{
    public class TileMapComponent : MonoBehaviour
    {
        TileDisplayComponent[,] m_tiles;
        TilePosition m_currentHoveredOverPosition;
        protected bool m_showNeutralOutline = false;

        public TileMap TileMap { get; private set; }

        public void Start()
        {
            WorldController.Instance.RegisterModeChangeHandler(OnPlayModeChange);
        }

        public void OnPlayModeChange(PlayMode playMode)
        {
            if(playMode == PlayMode.BUILD_MODE)
            {
                ShowNeutralOutline(true);
            }
            else
            {
                ShowNeutralOutline(false);
            }
        }

        public void Setup(TileMap tileMap)
        {
            if (TileMap == null)
            {
                TileMap = tileMap;

                m_tiles = new TileDisplayComponent[TileMap.Width, TileMap.Height];

                for (int x = 0; x < TileMap.Width; ++x)
                {
                    for(int z = 0; z < TileMap.Height; ++z)
                    {
                        TilePosition position = new TilePosition(x, z);
                        if (TileMap.TileAt(position) != null)
                        {
                            GameObject tileObject = UnityEngine.Object.Instantiate(WorldController.Instance.TilePrefab, transform);
                            tileObject.name = "Tile (" + x + "," + z + ")";
                            tileObject.transform.SetPositionAndRotation(new Vector3(x, 0, z), Quaternion.identity);
                            TileDisplayComponent tileComponent = tileObject.GetComponent<TileDisplayComponent>();
                            m_tiles[x, z] = tileComponent;
                        }
                    }
                }
            }
        }

	    // Update is called once per frame
	    void Update () {
		
	    }

        public TileDisplayComponent TileAt(TilePosition position)
        {
            if (position.X < 0 || position.X >= TileMap.Width ||
                position.Z < 0 || position.Z >= TileMap.Height)
            {
                return null;
            }
            return m_tiles[position.X, position.Z];
        }

        public void HoverOver(TilePosition position)
        {
            if (m_currentHoveredOverPosition != position)
            {
                if (m_currentHoveredOverPosition != null)
                {
                    TileDisplayComponent oldTile = TileAt(m_currentHoveredOverPosition);
                    if (oldTile != null)
                        oldTile.OnMouseExit();
                }

                m_currentHoveredOverPosition = position;
                TileDisplayComponent newTile = TileAt(m_currentHoveredOverPosition);
                if (newTile != null)
                    newTile.OnMouseEnter();
            }
        }

        public void ShowNeutralOutline(bool show)
        {
            if (show != m_showNeutralOutline)
            {
                m_showNeutralOutline = show;

                for (int x = 0; x < TileMap.Width; ++x)
                {
                    for (int z = 0; z < TileMap.Height; ++z)
                    {
                        m_tiles[x, z].ShowNeutralOutline(m_showNeutralOutline);
                    }
                }
            }
        }
    }
}