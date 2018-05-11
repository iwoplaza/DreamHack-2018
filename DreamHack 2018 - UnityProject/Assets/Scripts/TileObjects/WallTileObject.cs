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

        public WallTileObject()
        {
        }

        public override bool CanGoIntoFrom(MovementDirection entryDirection)
        {
            MovementDirection localEntryDirection = MovementDirectionUtils.OrientTowardsInverse(
                entryDirection, MovementDirectionUtils.NewFrom(Orientation)
            );

            Debug.Log(entryDirection + " -> " + localEntryDirection);
            Debug.Log("-8 % 8 == " + MathUtils.Mod(-8, 8));

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

        public override bool CanComeOutOfTowards(MovementDirection direction)
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

            Debug.Log("Returned true for " + localDirection);
            return true;
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