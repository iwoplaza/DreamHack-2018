using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.TileObjects;

namespace Game
{
    public class TileData
    {
        public TilePosition Position { get; private set; }
        public TileObjectBase InstalledObject { get; private set; }

        public TileData(TilePosition position)
        {
            Position = position;
            InstalledObject = null;
        }

        public TileData(int x, int y) : this(new TilePosition(x, y)) {}

        /// <summary>
        /// Installs the specified object on this tile.
        /// </summary>
        /// <param name="objectToInstall">The object to install</param>
        public void Install(TileObjectBase objectToInstall)
        {
            if(!objectToInstall.Installed)
            {
                objectToInstall.OnInstalledAt(this);
            }
        }
    }
}