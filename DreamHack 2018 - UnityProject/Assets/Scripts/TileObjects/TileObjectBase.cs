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

                Debug.Log("Found an installed object." + tileObject);

                return tileObject;
            }

            return null;
        }

        public virtual void Parse(XElement element, Tile optionalTile = null)
        {
            Debug.Log("Parsing installed object.");
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