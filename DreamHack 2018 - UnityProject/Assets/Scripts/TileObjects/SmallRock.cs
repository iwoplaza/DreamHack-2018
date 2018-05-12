using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Environment;

namespace Game.TileObjects
{
    public class SmallRock : TileObjectBase 
    {

        public override bool IsStatic { get { return true; } }
        public override string DisplayName { get { return "Small Rock"; } }
        public override bool IsImpenetrable { get { return false; } }

        public override bool CanGoIntoFrom(Pathfinding.MovementDirection entryDirection)
        {
            return true;
        }

        public override bool CanComeOutOfTowards(Pathfinding.MovementDirection direction)
        {
            return true;
        }

        protected override void OnInstalled()
        {
            base.OnInstalled();
        }

        public override void ConstructGameObject()
        {
            if (!Installed)
                return;
            Vector3 origin = InstalledAt.Position.Vector3 + new Vector3(0.5F, 0, 0.5F) + (Vector3.Scale(Random.onUnitSphere,new Vector3(0.5f,0.15f,0.5f)) * 0.75f);

            GameObject prefab = WorldPopulationResource.GetResources(WorldPopulationResource.PopulationType.ROCK_SMALL);
            if (prefab != null)
            {
                InstalledGameObject = Object.Instantiate(prefab);
                InstalledGameObject.transform.SetPositionAndRotation(origin, Quaternion.Euler(0.0F, Random.Range(0,360), 0.0F));
            }
        }

        public override GameObject CreateTemporaryDisplay()
        {
            GameObject prefab = WorldPopulationResource.GetResources(WorldPopulationResource.PopulationType.ROCK_SMALL);
            if (prefab != null)
            {
                return Object.Instantiate(prefab);
            }

            return null;
        }
    }
}
