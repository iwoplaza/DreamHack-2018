using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.TileObjects;
using System.Xml.Linq;
using System;

namespace Game
{
    public class Tile
    {
        public TileMap Owner { get; private set; }
        public TilePosition Position { get; private set; }
        public TileObjectBase InstalledObject { get; private set; }
        public bool HasObject { get { return InstalledObject != null; } }

        public Tile(TileMap owner, TilePosition position)
        {
            Owner = owner;
            Position = position;
            InstalledObject = null;
        }

        public Tile(TileMap owner, int x, int y) : this(owner, new TilePosition(x, y)) {}

        /// <summary>
        /// Installs the specified object on this tile.
        /// </summary>
        /// <param name="objectToInstall">The object to install</param>
        public void Install(TileObjectBase objectToInstall)
        {
            if(!objectToInstall.Installed)
            {
                objectToInstall.OnInstalledAt(this);
                InstalledObject = objectToInstall;
            }
        }

        public void UninstallObject()
        {
            if(HasObject)
            {
                InstalledObject.OnUninstalled();
                InstalledObject = null;
            }
        }

        public static Tile CreateAndParse(XElement element, TileMap tileMap)
        {
            if(element == null)
                return null;

            int x = 0,
                y = 0;
            XAttribute xAttrib = element.Attribute("x");
            XAttribute yAttrib = element.Attribute("y");

            if (xAttrib != null)
                x = Int32.Parse(xAttrib.Value);
            if(yAttrib != null)
                y = Int32.Parse(yAttrib.Value);

            Tile tile = new Tile(tileMap, x, y);

            XElement installedObjectElement = element.Element("InstalledObject");
            if(installedObjectElement != null)
            {
                tile.InstalledObject = TileObjectBase.CreateAndParse(installedObjectElement, tile);
            }

            return tile;
        }

        public void Populate(XElement element)
        {
            element.SetAttributeValue("x", Position.X);
            element.SetAttributeValue("y", Position.Z);

            if (InstalledObject != null)
            {
                XElement installedObjectElement = new XElement("InstalledObject");
                element.Add(installedObjectElement);
                InstalledObject.Populate(installedObjectElement);
            }
        }

        /// <summary>
        /// Returns if the passer can go through this tile.
        /// More in <see cref="TileObjectBase"/>
        /// </summary>
        /// <param name="passer"></param>
        /// <param name="entryDirection"></param>
        /// <returns></returns>
        public bool IsPassableFor(Living passer, Direction entryDirection)
        {
            if (InstalledObject == null)
                return true;
            else
                return InstalledObject.IsPassableFor(passer, entryDirection);
        }
    }
}