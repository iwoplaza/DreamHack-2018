using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TileObjects
{
    public class WallTileObject : TileObjectBase
    {
        public override string DisplayName
        {
            get { return "Wall"; }
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
    }
}