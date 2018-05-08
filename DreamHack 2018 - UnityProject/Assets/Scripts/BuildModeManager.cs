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
        public TileObjectBase TemporaryObject { get; private set; }
        public GameObject TemporaryDisplayObject { get; private set; }
        public TilePosition Cursor { get; private set; }
        public Direction PropOrientation { get; private set; }
        public int PropVariant { get; private set; }

        OnHoldHandler m_onHoldHandlers;
        GameState m_gameState;

        public BuildModeManager(GameState gameState)
        {
            m_gameState = gameState;
            WorldController.Instance.RegisterModeChangeHandler(OnPlayModeChange);
        }

        public void OnPlayModeChange(PlayMode playMode)
        {
            if (playMode == PlayMode.BUILD_MODE)
            {
                OnEnabled();
            }
            else
            {
                OnDisabled();
            }
        }

        public void OnEnabled()
        {
            PropOrientation = Direction.POSITIVE_Z;
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

            PropVariant = 0;
            TemporaryObject = null;
            HeldObjectType = tileObjectType;
            TileObjectBase tileObject = HeldObjectType.Assembly.CreateInstance(HeldObjectType.FullName) as TileObjectBase;
            if (tileObject != null)
            {
                TemporaryObject = tileObject;
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
                    if (tileObject != null)
                    {
                        tileObject.Rotate(PropOrientation);
                        tile.Install(tileObject);
                    }
                }
            }
        }

        public void SetCursorPosition(TilePosition position)
        {
            Cursor = position;
            UpdateView();
        }

        public void RotatePropLeft()
        {
            PropOrientation = DirectionUtils.RotateCCW(PropOrientation);
            UpdateView();
        }

        public void RotatePropRight()
        {
            PropOrientation = DirectionUtils.RotateCW(PropOrientation);
            UpdateView();
        }

        void UpdateView()
        {
            if(TemporaryDisplayObject != null)
            {
                TemporaryDisplayObject.transform.position = Cursor.Vector3 + new Vector3(0.5F, 0, 0.5F);
                TemporaryDisplayObject.transform.rotation = Quaternion.Euler(0.0F, DirectionUtils.GetYRotation(PropOrientation), 0.0F);
            }
        }

        public void RegisterOnHoldHandler(OnHoldHandler handler)
        {
            m_onHoldHandlers += handler;
        }

        public void SetVariant(int variant)
        {
            PropVariant = variant;
        }
    }
}