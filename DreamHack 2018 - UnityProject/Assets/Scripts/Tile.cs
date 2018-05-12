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
        public bool IsRootForObject { get; private set; }
        public bool IsRootForFloor { get; private set; }
        public bool HasCliff { get; private set; }
        public int PathfindingWeight { get; private set; }
        public bool HasObject { get { return InstalledObject != null; } }
        public bool HasFloor { get { return InstalledFloor != null; } }
        public bool Empty { get { return !HasObject && !HasFloor && !HasCliff; } }
        public bool CanInstallObject { get { return !HasObject && !HasCliff; } }
        public bool CanInstallFloor { get { return !HasFloor && !HasCliff; } }
        /// <summary>
        /// Tells the Populate function if it should create an element for this Tile
        /// when serializing.
        /// </summary>
        public bool IsWorthSaving { get { return !Empty; } }

        public bool IsImpenetrable
        {
            get
            {
                bool impenetrable = false;
                if (InstalledObject != null)
                    impenetrable = impenetrable || InstalledObject.IsImpenetrable;
                if (InstalledFloor != null)
                    impenetrable = impenetrable || InstalledFloor.IsImpenetrable;
                return impenetrable;
            }
        }

        public Tile(TileMap owner, TilePosition position)
        {
            Owner = owner;
            Position = position;
            InstalledObject = null;
            InstalledFloor = null;
            IsRootForObject = false;
            IsRootForFloor = false;
        }

        public Tile(TileMap owner, int x, int y) : this(owner, new TilePosition(x, y)) {}

        public bool CanInstallProp(TileProp.PropType type)
        {
            if (type == TileProp.PropType.OBJECT)
                return CanInstallObject;
            else if (type == TileProp.PropType.FLOOR)
                return CanInstallFloor;
            else
                return false;
        }

        /// <summary>
        /// Installs the specified object on this tile.
        /// </summary>
        /// <param name="objectToInstall">The object to install</param>
        public bool InstallAsRoot(TileObjectBase objectToInstall)
        {
            Vector2Int dimensions = objectToInstall.OrientedDimensions;
            if (dimensions.x > 1 || dimensions.y > 1 && !Owner.CanInstallPropAtArea(TileProp.PropType.OBJECT, Position, dimensions))
            {
                return false;
            }

            if(CanInstallObject && !objectToInstall.Installed)
            {
                objectToInstall.OnInstalledAt(this);
                InstalledObject = objectToInstall;
                IsRootForObject = true;

                for (ushort x = 0; x < dimensions.x; ++x)
                {
                    for (ushort z = 0; z < dimensions.y; ++z)
                    {
                        if (x == 0 && z == 0)
                            continue;
                        TilePosition position = Position.GetOffset(x, z);
                        Tile tile = Owner.TileAt(position);
                        if (tile != null)
                            tile.Install(objectToInstall);
                    }
                }

                Owner.OnModifyEvent(Position);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Installs the specified object on this tile.
        /// </summary>
        /// <param name="floorToInstall">The floor to install</param>
        public bool InstallAsRoot(TileFloorBase floorToInstall)
        {
            Vector2Int dimensions = floorToInstall.OrientedDimensions;
            if (dimensions.x > 1 || dimensions.y > 1 && !Owner.CanInstallPropAtArea(TileProp.PropType.FLOOR, Position, dimensions))
            {
                return false;
            }

            if (CanInstallFloor && !floorToInstall.Installed)
            {
                floorToInstall.OnInstalledAt(this);
                InstalledFloor = floorToInstall;
                IsRootForFloor = true;

                if (dimensions.x > 1 || dimensions.y > 1)
                {
                    for (ushort x = 0; x < dimensions.x; ++x)
                    {
                        for (ushort z = 0; z < dimensions.y; ++z)
                        {
                            if (x == 0 && z == 0)
                                continue;
                            TilePosition position = Position.GetOffset(x, z);
                            Tile tile = Owner.TileAt(position);
                            if (tile != null)
                                tile.Install(floorToInstall);
                        }
                    }
                }

                Owner.OnModifyEvent(Position);
                return true;
            }
            return false;
        }

        public bool InstallAsRoot(TileProp tileProp)
        {
            if (tileProp is TileObjectBase)
                return InstallAsRoot(tileProp as TileObjectBase);
            else if (tileProp is TileFloorBase)
                return InstallAsRoot(tileProp as TileFloorBase);
            else
                return false;
        }

        void Install(TileObjectBase objectToInstall)
        {
            if (CanInstallObject)
            {
                InstalledObject = objectToInstall;
                IsRootForObject = false;
                Owner.OnModifyEvent(Position);
            }
        }

        void Install(TileFloorBase floorToInstall)
        {
            if (CanInstallObject)
            {
                InstalledFloor = floorToInstall;
                IsRootForFloor = false;
                Owner.OnModifyEvent(Position);
            }
        }

        public void UninstallObject()
        {
            if(HasObject)
            {
                if (IsRootForObject)
                {
                    InstalledObject.OnUninstalled();
                    IsRootForObject = false;
                }
                else
                {
                    InstalledObject.InstalledAt.UninstallObject();
                }
                InstalledObject = null;
                Owner.OnModifyEvent(Position);
            }
        }

        public void UninstallFloor()
        {
            if (HasFloor)
            {
                if (IsRootForFloor)
                {
                    InstalledFloor.OnUninstalled();
                    IsRootForFloor = false;
                }
                else
                {
                    InstalledFloor.InstalledAt.UninstallFloor();
                }
                InstalledFloor = null;
                Owner.OnModifyEvent(Position);
            }
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

            XAttribute rootForObject = element.Attribute("rootForObject");
            XAttribute rootForFloor = element.Attribute("rootForFloor");
            if (rootForObject != null)
                tile.IsRootForObject = true;
            if (rootForFloor != null)
                tile.IsRootForFloor = true;

            XElement installedObjectElement = element.Element("InstalledObject");
            if (installedObjectElement != null)
                tile.InstalledObject = TileObjectBase.CreateAndParse(installedObjectElement, tile);

            XElement installedFloorElement = element.Element("InstalledFloor");
            if (installedFloorElement != null)
                tile.InstalledFloor = TileFloorBase.CreateAndParse(installedFloorElement, tile);

            return tile;
        }

        /// <summary>
        /// This only runs when <see cref="IsWorthSaving"/> is true.
        /// </summary>
        /// <param name="element">The element to populate</param>
        public void Populate(XElement element)
        {
            element.SetAttributeValue("x", Position.X);
            element.SetAttributeValue("y", Position.Z);

            if (IsRootForObject)
                element.SetAttributeValue("rootForObject", true);
            if (IsRootForFloor)
                element.SetAttributeValue("rootForFloor", true);

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
        /// Returns if the passer can go into this tile.
        /// More in <see cref="TileProp"/>
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool CanGoIntoFrom(Pathfinding.MovementDirection direction)
        {
            bool passable = true;

            if (InstalledObject != null) 
                passable = passable && InstalledObject.CanGoIntoFrom(Position, direction);

            if (InstalledFloor != null)
                passable = passable && InstalledFloor.CanGoIntoFrom(Position,direction);

            return passable;
        }

        /// <summary>
        /// Returns if the passer can go out of this tile.
        /// More in <see cref="TileProp"/>
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool CanComeOutOfTowards(Pathfinding.MovementDirection direction)
        {
            bool passable = true;

            if (InstalledObject != null)
                passable = passable && InstalledObject.CanComeOutOfTowards(Position, direction);

            if (InstalledFloor != null)
                passable = passable && InstalledFloor.CanComeOutOfTowards(Position, direction);

            return passable;
        }

        public bool CanSkimThrough()
        {
            bool passable = true;

            if (InstalledObject != null)
                passable = passable && InstalledObject.CanSkimThrough;

            if (InstalledFloor != null)
                passable = passable && InstalledFloor.CanSkimThrough;

            return passable;
        }
    }
}