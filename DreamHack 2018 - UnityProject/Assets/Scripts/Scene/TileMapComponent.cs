using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scene
{
    public class TileMapComponent : MonoBehaviour
    {
        GameObject m_tileDisplaysHolder;
        TileDisplayComponent[,] m_tileDisplays;
        GameObject m_tileHoverDisplay;

        TilePosition m_currentHoveredOverPosition;
        protected bool m_showOutline = false;

        public TileMap TileMap { get; private set; }
        /// <summary>
        /// The maximum number of tiles in the X axis that are going to
        /// be displayed at a time.
        /// </summary>
        public int DisplayWidth { get; private set; }
        /// <summary>
        /// The maximum number of tiles in the Z axis that are going to
        /// be displayed at a time.
        /// </summary>
        public int DisplayHeight { get; private set; }

        /// <summary>
        /// Determines how offset the display is from the actual
        /// TileMap.
        /// Used to draw the partial outline of the TileMap.
        /// </summary>
        public TilePosition DisplayOffset { get; private set; }

        public static int MAX_DISPLAY_WIDTH = 80;
        public static int MAX_DISPLAY_HEIGHT = 80;

        public void Start()
        {
            WorldController.Instance.RegisterModeChangeHandler(OnPlayModeChange);
        }

        public void OnPlayModeChange(PlayMode playMode)
        {
            if(playMode == PlayMode.BUILD_MODE)
            {
                ShowOutline(true);
            }
            else
            {
                ShowOutline(false);
            }
        }

        public void Setup(TileMap tileMap)
        {
            if (TileMap == null)
            {
                TileMap = tileMap;
                DisplayOffset = new TilePosition(0, 0);
                DisplayWidth = Math.Min(TileMap.Width, MAX_DISPLAY_WIDTH);
                DisplayHeight = Math.Min(TileMap.Length, MAX_DISPLAY_HEIGHT);
                m_tileDisplays = new TileDisplayComponent[DisplayWidth, DisplayHeight];
                m_currentHoveredOverPosition = null;

                m_tileDisplaysHolder = new GameObject("TileDisplays");
                m_tileDisplaysHolder.transform.parent = transform;
                m_tileDisplaysHolder.SetActive(false);

                for (int x = 0; x < DisplayWidth; ++x)
                {
                    for(int z = 0; z < DisplayHeight; ++z)
                    {
                        TilePosition position = new TilePosition(x, z);
                        GameObject tileObject = Instantiate(Resources.TileDisplayPrefab, m_tileDisplaysHolder.transform);
                        tileObject.name = "Tile (" + x + "," + z + ")";
                        tileObject.transform.SetPositionAndRotation(position.Vector3, Quaternion.identity);
                        TileDisplayComponent tileComponent = tileObject.GetComponent<TileDisplayComponent>();
                        m_tileDisplays[x, z] = tileComponent;
                    }
                }

                m_tileHoverDisplay = Instantiate(Resources.TileDisplayHoverPrefab, transform);
                m_tileHoverDisplay.SetActive(false);
            }
        }

	    // Update is called once per frame
	    void Update () {
		
	    }

        void UpdateView()
        {
            m_tileDisplaysHolder.transform.position = DisplayOffset.Vector3;
        }

        TileDisplayComponent TileAt(TilePosition position)
        {
            if (position.X < DisplayOffset.X || position.X >= DisplayOffset.X + DisplayWidth ||
                position.Z < DisplayOffset.Z || position.Z >= DisplayOffset.Z + DisplayHeight)
            {
                return null;
            }
            return m_tileDisplays[position.X - DisplayOffset.X, position.Z - DisplayOffset.Z];
        }

        public void HoverOver(TilePosition position)
        {
            if (m_currentHoveredOverPosition != position)
            {
                if (position == null || TileAt(position) == null)
                {
                    m_tileHoverDisplay.SetActive(false);
                    m_currentHoveredOverPosition = null;
                }
                else
                {
                    m_currentHoveredOverPosition = position;
                    m_tileHoverDisplay.SetActive(true);
                    m_tileHoverDisplay.transform.position = m_currentHoveredOverPosition.Vector3 + new Vector3(0.5F, 0, 0.5F);
                }
            }
        }

        public void UpdateViewpoint(Vector3 viewPoint)
        {
            DisplayOffset = TilePosition.FromWorldPosition(viewPoint);
            var x = DisplayOffset.X - DisplayWidth / 2;
            var z = DisplayOffset.Z - DisplayHeight / 2;

            x = Math.Min(x, TileMap.Width - DisplayWidth);
            z = Math.Min(z, TileMap.Length - DisplayHeight);
            DisplayOffset = new TilePosition(x, z);

            UpdateView();
        }

        public void ShowOutline(bool show)
        {
            if (show != m_showOutline)
            {
                m_showOutline = show;
                m_tileDisplaysHolder.SetActive(m_showOutline);
            }
        }
    }
}