using Game.Pathfinding;
using Game.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TileObjects
{
    public class MinerTileObject : TileObjectBase
    {
        public override string DisplayName { get { return "Miner"; } }
        public override bool IsImpenetrable { get { return true; } }
        public override bool CanSkimThrough { get { return false; } }
        public override int Width { get { return 1; } }
        public override int Length { get { return 1; } }
        public override int MetalCost { get { return 100; } }

        public MinerTileObject() { }

        protected override void OnInstalled()
        {
            ConstructGameObject();
        }

        public override bool CanGoIntoFrom(TilePosition globalPosition, MovementDirection entryDirection)
        {
            return false;
        }

        public override bool CanComeOutOfTowards(TilePosition globalPosition, MovementDirection direction)
        {
            return false;
        }

        public GameObject GetPrefab()
        {
            return Resources.TileObjectPrefabs.Find("Miner");
        }

        public override void ConstructGameObject()
        {
            if (!Installed)
                return;

            Vector2Int dimensions = OrientedDimensions;
            Vector3 origin = InstalledAt.Position.Vector3 + new Vector3(dimensions.x / 2.0F, 0.0F, dimensions.y / 2.0F);

            GameObject prefab = GetPrefab();
            if(prefab.GetComponent<Miner>() != null)
            {
                prefab.GetComponent<Miner>().MinerTilePosition = InstalledAt.Position;
                prefab.GetComponent<Miner>().StartMineRoutine();
            }
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

        protected void Mine()
        {
            GameState gameState = WorldController.Instance.MainState;

            int radiusX = 10;
            int radiusZ = 10;

            TilePosition center = InstalledAt.Position;
            for (int x = -radiusX; x <= radiusX; ++x)
            {
                for (int z = -radiusZ; z <= radiusZ; ++z)
                {
                    gameState.GameEnvironment.MetalMap.SetMetalAmountAt(center.GetOffset(x, z), 16);
                }
            }
        }
    }
}