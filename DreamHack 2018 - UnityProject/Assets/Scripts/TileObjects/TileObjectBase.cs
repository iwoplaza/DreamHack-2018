using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TileObjects
{
    public abstract class TileObjectBase
    {
        public bool Installed { get; private set; }
        public TileData InstalledAt { get; private set; }

        public abstract string DisplayName { get; }

        public TileObjectBase()
        {
            Installed = false;
            InstalledAt = null;
        }

        public void OnInstalledAt(TileData targetTile)
        {
            if(!Installed)
            {
                Installed = true;
                InstalledAt = targetTile;
                OnInstalled();
            }
        }

        /// <summary>
        /// Called when this TileObject is installed at a specific location
        /// in the world.
        /// </summary>
        protected abstract void OnInstalled();
    }
}