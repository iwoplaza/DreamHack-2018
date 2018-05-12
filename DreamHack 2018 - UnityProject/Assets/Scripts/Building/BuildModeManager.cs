using Game.TileObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Building
{
    public class BuildModeManager
    {
        public delegate void OnHoldHandler(Type tileObject);
        public Type HeldObjectType { get; private set; }
        public TileProp TemporaryProp { get; private set; }
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
        }

        public void OnDisabled()
        {
            if (TemporaryDisplayObject != null)
                UnityEngine.Object.Destroy(TemporaryDisplayObject);

            HeldObjectType = null;
        }

        public void Hold(Type tileObjectType, int variant = 0)
        {
            if(TemporaryDisplayObject != null)
            {
                UnityEngine.Object.Destroy(TemporaryDisplayObject);
            }

            SetVariant(variant);
            TemporaryProp = null;
            HeldObjectType = tileObjectType;
            TileProp tileProp = HeldObjectType.Assembly.CreateInstance(HeldObjectType.FullName) as TileProp;
            if (tileProp != null)
            {
                tileProp.Variant = PropVariant;
                TemporaryProp = tileProp;
                TemporaryDisplayObject = tileProp.CreateTemporaryDisplay();
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
                if (tile != null && tile.CanInstallObject)
                {
                    TileProp tileProp = HeldObjectType.Assembly.CreateInstance(HeldObjectType.FullName) as TileProp;
                    if (tileProp != null)
                    {
                        tileProp.Variant = PropVariant;
                        tileProp.Rotate(PropOrientation);
                        tile.InstallAsRoot(tileProp);
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
                Vector2Int dimensions = TemporaryProp.OrientedDimensions;
                TemporaryDisplayObject.transform.position = Cursor.Vector3 + new Vector3(TemporaryProp.Width / 2.0F, 0.001F, TemporaryProp.Length / 2.0F);
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