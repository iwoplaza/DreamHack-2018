using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TileFloors
{
    public class DefaultTileFloor : TileFloorBase
    {
        public override string DisplayName { get { return "Default Floor"; } }
        public override bool IsImpenetrable { get { return false; } }

        public override bool IsPassableFor(Direction entryDirection)
        {
            return true;
        }

        public GameObject GetPrefab()
        {
            return Resources.FindTileFloorPrefab("Floor_" + (Variant + 1));
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
