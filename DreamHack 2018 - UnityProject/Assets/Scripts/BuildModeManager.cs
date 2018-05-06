using Game.TileObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BuildModeManager
    {
        public delegate void OnHoldHandler(Type tileObject);
        public Type HeldObjectType { get; private set; }
        public GameObject TemporaryDisplayObject { get; private set; }
        public TilePosition Cursor { get; private set; }

        OnHoldHandler m_onHoldHandlers;
        GameState m_gameState;

        public BuildModeManager(GameState gameState)
        {
            m_gameState = gameState;
        }

        public void OnEnabled()
        {
            Hold(typeof(WallTileObject));
        }

        public void OnDisabled()
        {
            if (TemporaryDisplayObject != null)
                UnityEngine.Object.Destroy(TemporaryDisplayObject);

            HeldObjectType = null;
        }

        public void Hold(Type tileObjectType)
        {
            if(TemporaryDisplayObject != null)
            {
                UnityEngine.Object.Destroy(TemporaryDisplayObject);
            }

            HeldObjectType = tileObjectType;
            TileObjectBase tileObject = HeldObjectType.Assembly.CreateInstance(HeldObjectType.FullName) as TileObjectBase;
            if (tileObject != null)
            {
                TemporaryDisplayObject = tileObject.CreateTemporaryDisplay();
                foreach (Collider collider in TemporaryDisplayObject.GetComponentsInChildren<Collider>())
                {
                    collider.isTrigger = true;
                }

                if (m_onHoldHandlers != null)
                    m_onHoldHandlers(tileObjectType);
            }
        }

        public void Place()
        {
            if (HeldObjectType != null)
            {
                Tile tile = m_gameState.TileMap.TileAt(Cursor);
                if (tile != null && !tile.HasObject)
                {
                    TileObjectBase tileObject = HeldObjectType.Assembly.CreateInstance(HeldObjectType.FullName) as TileObjectBase;
                    if(tileObject != null)
                        tile.Install(tileObject);
                }
            }
        }

        public void SetCursorPosition(TilePosition position)
        {
            Cursor = position;
            UpdateView();
        }

        void UpdateView()
        {
            if(TemporaryDisplayObject != null)
            {
                TemporaryDisplayObject.transform.position = Cursor.Vector3;
            }
        }

        public void RegisterOnHoldHandler(OnHoldHandler handler)
        {
            m_onHoldHandlers += handler;
        }
    }
}