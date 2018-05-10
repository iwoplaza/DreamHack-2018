using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TileObjects
{
    public class WallTileObject : TileObjectBase
    {
        public override string DisplayName { get { return "Wall"; } }

        public WallTileObject()
        {
        }

        protected override void OnInstalled()
        {
            base.OnInstalled();
        }

        public override bool IsPassableFor(Living passer, Direction entryDirection)
        {
            return false;
        }

        public GameObject GetPrefab()
        {
            return Resources.FindTileObjectPrefab(Variant == 0 ? "Wall_Straight" : "Wall_Windowed");
        }

        public override void ConstructGameObject()
        {
            if (!Installed)
                return;
            Vector3 origin = InstalledAt.Position.Vector3 + new Vector3(0.5F, 0, 0.5F);

            GameObject prefab = GetPrefab();
            if (prefab != null)
            {
                InstalledGameObject = Object.Instantiate(prefab);
                InstalledGameObject.transform.SetPositionAndRotation(origin, Quaternion.Euler(0.0F, DirectionUtils.GetYRotation(Orientation), 0.0F));
            }
        }

        public override GameObject CreateTemporaryDisplay()
        {
            GameObject prefab = GetPrefab();
            if (prefab != null)
            {
                return Object.Instantiate(prefab);
            }

            return null;
        }
    }
}