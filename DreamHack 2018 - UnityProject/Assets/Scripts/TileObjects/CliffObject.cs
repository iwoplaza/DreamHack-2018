using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TileObjects
{
    public class CliffObject : TileObjectBase 
    {

        public override bool IsStatic { get { return true; } }
        public override string DisplayName { get { return "Cliff"; } }
        public override bool IsImpenetrable { get { return true; } }

        public override bool IsPassableFor(Direction entryDirection)
        {
            return false;
        }

        public CliffObject()
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

            GameObject prefab = Resources.FindEnvironmentObjectPrefab("Cliff");
            if (prefab != null)
            {
                InstalledGameObject = Object.Instantiate(prefab);
                InstalledGameObject.transform.SetPositionAndRotation(origin, Quaternion.Euler(0.0F, DirectionUtils.GetYRotation(Orientation), 0.0F));
            }
        }

        public override GameObject CreateTemporaryDisplay()
        {
            GameObject prefab = Resources.FindEnvironmentObjectPrefab("Cliff");
            if (prefab != null)
            {
                return Object.Instantiate(prefab);
            }

            return null;
        }
    }
}
