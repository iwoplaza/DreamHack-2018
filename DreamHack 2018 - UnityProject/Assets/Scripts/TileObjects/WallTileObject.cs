using System.Collections;
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

        public WallTileObject(int variant = 0) : base(variant)
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
            Vector3 origin = InstalledAt.Position.Vector3 + new Vector3(0.5F, 0, 0.5F);

            GameObject prefab = Resources.FindTileObjectPrefab("Wall_Straight_0");
            if (prefab != null)
            {
                InstalledGameObject = Object.Instantiate(prefab);
                InstalledGameObject.transform.SetPositionAndRotation(origin, Quaternion.Euler(0.0F, DirectionUtils.GetYRotation(Orientation), 0.0F));
            }
        }

        public override GameObject CreateTemporaryDisplay()
        {
            GameObject prefab = Resources.FindTileObjectPrefab("Wall_Straight_0");
            if (prefab != null)
            {
                return Object.Instantiate(prefab);
            }

            return null;
        }
    }
}