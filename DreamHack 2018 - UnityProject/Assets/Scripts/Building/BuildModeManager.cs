﻿using Game.TileObjects;
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
        public GameObject[] TileDisplayObjects { get; private set; }
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

            if(TileDisplayObjects != null)
            {
                foreach(GameObject obj in TileDisplayObjects)
                {
                    UnityEngine.Object.Destroy(obj);
                }
                TileDisplayObjects = null;
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
                CreateTileDisplayObjects();

                if (m_onHoldHandlers != null)
                    m_onHoldHandlers(tileObjectType);
            }
        }

        void CreateTileDisplayObjects()
        {
            int width = TemporaryProp.Width;
            int length = TemporaryProp.Length;

            TileDisplayObjects = new GameObject[TemporaryProp.Width * TemporaryProp.Length];
            for (int x = 0; x < width; ++x)
            {
                for (int z = 0; z < length; ++z)
                {
                    TileDisplayObjects[z * length + x] = UnityEngine.Object.Instantiate(Resources.TileDisplayHoverPrefab);
                }
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
            if (TemporaryProp != null)
            {
                TemporaryProp.Rotate(PropOrientation);
                UpdateView();
            }
        }

        public void RotatePropRight()
        {
            PropOrientation = DirectionUtils.RotateCW(PropOrientation);
            if (TemporaryProp != null)
            {
                TemporaryProp.Rotate(PropOrientation);
                UpdateView();
            }
        }

        void UpdateView()
        {
            if(TemporaryDisplayObject != null)
            {
                int width = TemporaryProp.Width;
                int length = TemporaryProp.Length;

                Vector2Int dimensions = TemporaryProp.OrientedDimensions;
                TemporaryDisplayObject.transform.position = Cursor.Vector3 + new Vector3(dimensions.x / 2.0F, 0.001F, dimensions.y / 2.0F);
                TemporaryDisplayObject.transform.rotation = Quaternion.Euler(0.0F, DirectionUtils.GetYRotation(PropOrientation), 0.0F);

                for(int x = 0; x < width; ++x)
                {
                    for (int z = 0; z < length; ++z)
                    {
                        TilePosition tile = TilePosition.RotateInBlock(new TilePosition(x, z), width, length, PropOrientation);
                        TileDisplayObjects[z * length + x].transform.position = Cursor.Vector3 + tile.Vector3 + new Vector3(0.5F, 0, 0.5F);
                    }
                }
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