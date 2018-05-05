using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Game.TileObjects
{
    public abstract class TileObjectBase
    {
        public Tile InstalledAt { get; private set; }
        public bool Installed { get { return InstalledAt != null; } }

        public GameObject InstalledGameObject { get; set; }

        public abstract string DisplayName { get; }
        /// <summary>
        /// Determines if something/someone can pass through this TileObject.
        /// Used for Path Finding.
        /// </summary>
        /// <param name="passer">The passer that's trying to pass through this object.</param>
        /// <param name="entryDirection">The direction it's coming from.
        ///                              (relative to the object, not the passer)</param>
        /// <returns>Whether or not the passer can pass.</returns>
        public abstract bool IsPassableFor(Living passer, Direction entryDirection);

        /// <summary>
        /// Used for determining how passable compared to others this object is.
        /// </summary>
        public virtual float PassWeight { get { return 1; } }

        public TileObjectBase()
        {
            InstalledAt = null;
        }

        public void OnInstalledAt(Tile targetTile)
        {
            if(InstalledAt == null)
            {
                InstalledAt = targetTile;
                OnInstalled();
            }
        }

        /// <summary>
        /// Called when this TileObject is installed at a specific location
        /// in the world.
        /// </summary>
        protected virtual void OnInstalled()
        {
            ConstructGameObject();
        }

        public virtual void OnUninstalled()
        {
            RemoveGameObject();
        }

        public static TileObjectBase CreateAndParse(XElement element, Tile optionalTile = null)
        {
            XAttribute typeAttrib = element.Attribute("type");
            if (typeAttrib == null)
                return null;

            Type classType = typeof(TileObjectBase).Assembly.GetType(typeAttrib.Value);
            if (classType != null)
            {
                TileObjectBase tileObject = classType.Assembly.CreateInstance(classType.FullName) as TileObjectBase;
                tileObject.Parse(element, optionalTile);

                return tileObject;
            }

            return null;
        }

        public virtual void Parse(XElement element, Tile optionalTile = null)
        {
            if (optionalTile != null)
            {
                InstalledAt = optionalTile;
                ConstructGameObject();
            }
        }

        public virtual void Populate(XElement element)
        {
            String typeName = GetType().FullName;
            element.SetAttributeValue("type", typeName);
        }

        public abstract void ConstructGameObject();
        public virtual void RemoveGameObject()
        {
            UnityEngine.Object.Destroy(InstalledGameObject);
        }
    }
}