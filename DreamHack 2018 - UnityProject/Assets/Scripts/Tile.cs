using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.TileObjects;
using System.Xml.Linq;
using System;
using Game.TileFloors;

namespace Game
{
    public class Tile
    {
        public TileMap Owner { get; private set; }
        public TilePosition Position { get; private set; }
        public TileObjectBase InstalledObject { get; private set; }
        public TileFloorBase InstalledFloor { get; private set; }
        public bool HasCliff { get; private set; }
        public bool HasObject { get { return InstalledObject != null; } }
        public bool HasFloor { get { return InstalledFloor != null; } }
        public bool Empty { get { return !HasObject && !HasFloor && !HasCliff; } }
        public bool CanInstallObject { get { return !HasObject && !HasCliff; } }
        public bool CanInstallFloor { get { return !HasFloor && !HasCliff; } }

        public Tile(TileMap owner, TilePosition position)
        {
            Owner = owner;
            Position = position;
            InstalledObject = null;
            InstalledFloor = null;
        }

        public Tile(TileMap owner, int x, int y) : this(owner, new TilePosition(x, y)) {}

        /// <summary>
        /// Installs the specified object on this tile.
        /// </summary>
        /// <param name="objectToInstall">The object to install</param>
        public void Install(TileObjectBase objectToInstall)
        {
            if(CanInstallObject && !objectToInstall.Installed)
            {
                objectToInstall.OnInstalledAt(this);
                InstalledObject = objectToInstall;
            }
            Owner.OnModifyEvent(Position);
        }

        public void Install(TileProp tileProp)
        {
            if (tileProp is TileObjectBase)
                Install(tileProp as TileObjectBase);
            else if (tileProp is TileFloorBase)
                Install(tileProp as TileFloorBase);
        }

        /// <summary>
        /// Installs the specified object on this tile.
        /// </summary>
        /// <param name="objectToInstall">The object to install</param>
        public void Install(TileFloorBase floorToInstall)
        {
            if (CanInstallFloor && !floorToInstall.Installed)
            {
                floorToInstall.OnInstalledAt(this);
                InstalledFloor = floorToInstall;
            }
            Owner.OnModifyEvent(Position);
        }

        public void UninstallObject()
        {
            if(HasObject)
            {
                InstalledObject.OnUninstalled();
                InstalledObject = null;
            }
            Owner.OnModifyEvent(Position);
        }

        public void UninstallFloor()
        {
            if (HasFloor)
            {
                InstalledFloor.OnUninstalled();
                InstalledFloor = null;
            }
            Owner.OnModifyEvent(Position);
        }

        public void SetHasCliff(bool flag)
        {
            HasCliff = flag;
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
            if (installedObjectElement != null)
                tile.InstalledObject = TileObjectBase.CreateAndParse(installedObjectElement, tile);

            XElement installedFloorElement = element.Element("InstalledFloor");
            if (installedFloorElement != null)
                tile.InstalledFloor = TileFloorBase.CreateAndParse(installedFloorElement, tile);

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

            if (InstalledFloor != null)
            {
                XElement installedFloorElement = new XElement("InstalledFloor");
                element.Add(installedFloorElement);
                InstalledFloor.Populate(installedFloorElement);
            }
        }

        /// <summary>
        /// Returns if the passer can go through this tile.
        /// More in <see cref="TileProp"/>
        /// </summary>
        /// <param name="passer"></param>
        /// <param name="entryDirection"></param>
        /// <returns></returns>
        public bool IsPassableFor(Living passer, Direction entryDirection)
        {
            bool passable = true;

            if (InstalledObject != null)
                passable = passable && InstalledObject.IsPassableFor(passer, entryDirection);

            if (InstalledFloor != null)
                passable = passable && InstalledFloor.IsPassableFor(passer, entryDirection);

            return passable;
        }
    }
}