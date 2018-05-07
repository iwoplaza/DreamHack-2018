﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TileObjects
{
    public class WallTileObject : TileObjectBase
    {
        public override string DisplayName { get { return "Wall"; } }
        public override bool IsPassableFor(Living passer, Direction entryDirection)
        {
            return false;
        }

        public WallTileObject()
        {

        }

        protected override void OnInstalled()
        {
            base.OnInstalled();
        }

        public override void ConstructGameObject()
        {
            if (!Installed)
                return;
            Vector3 origin = InstalledAt.Position.Vector3;

            GameObject prefab = Resources.FindTileObjectPrefab("Wall");
            if (prefab != null)
            {
                InstalledGameObject = Object.Instantiate(prefab);
                InstalledGameObject.transform.SetPositionAndRotation(origin, Quaternion.identity);
            }
        }

        public override GameObject CreateTemporaryDisplay()
        {
            GameObject prefab = Resources.FindTileObjectPrefab("Wall");
            if (prefab != null)
            {
                return Object.Instantiate(prefab);
            }

            return null;
        }
    }
}