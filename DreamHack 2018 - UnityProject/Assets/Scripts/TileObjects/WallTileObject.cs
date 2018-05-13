using Game.Pathfinding;
using Game.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TileObjects
{
    public class WallTileObject : TileObjectBase
    {
        public override string DisplayName { get { return "Wall"; } }
        public override bool IsImpenetrable { get { return false; } }
        public override bool CanSkimThrough { get { return false; } }

        public override int MetalCost { get { return 10; } }

        public WallTileObject()
        {
        }

        public override bool CanGoIntoFrom(TilePosition position, MovementDirection entryDirection)
        {
            MovementDirection localEntryDirection = MovementDirectionUtils.OrientTowardsInverse(
                entryDirection, MovementDirectionUtils.NewFrom(Orientation)
            );

            if (localEntryDirection == MovementDirection.POSITIVE_X || //Side
                localEntryDirection == MovementDirection.NEGATIVE_X || //Side
                localEntryDirection == MovementDirection.POSITIVE_Z || //Front
                localEntryDirection == MovementDirection.POSITIVE_Z_POSITIVE_X || //FrontSideDiagonal
                localEntryDirection == MovementDirection.POSITIVE_Z_NEGATIVE_X)    //FrontSideDiagonal
            {
                return true;
            }

            return false;
        }

        public override bool CanComeOutOfTowards(TilePosition position, MovementDirection direction)
        {
            MovementDirection localDirection = MovementDirectionUtils.OrientTowardsInverse(
                direction, MovementDirectionUtils.NewFrom(Orientation)
            );

            if (localDirection == MovementDirection.POSITIVE_Z ||
                localDirection == MovementDirection.POSITIVE_Z_NEGATIVE_X ||
                localDirection == MovementDirection.POSITIVE_Z_POSITIVE_X)
            {
                return false;
            }

            return true;
        }

        public GameObject GetPrefab()
        {
            return Resources.TileObjectPrefabs.Find(Variant == 0 ? "Wall_Straight" : "Wall_Windowed");
        }

        public override void ConstructGameObject()
        {
            if (!Installed)
                return;
            Vector3 origin = InstalledAt.Position.Vector3 + new Vector3(0.5F, 0, 0.5F);

            GameObject prefab = GetPrefab();
            if (prefab != null)
            {
                InstalledGameObject = UnityEngine.Object.Instantiate(prefab);
                InstalledGameObject.transform.SetPositionAndRotation(origin, Quaternion.Euler(0.0F, DirectionUtils.GetYRotation(Orientation), 0.0F));
            }
        }

        public override GameObject CreateTemporaryDisplay()
        {
            GameObject prefab = GetPrefab();
            if (prefab != null)
            {
                return UnityEngine.Object.Instantiate(prefab);
            }

            return null;
        }
    }
}