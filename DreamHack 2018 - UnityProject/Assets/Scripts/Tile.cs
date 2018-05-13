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
        public TileProp[] InstalledProps { get; private set; }
        public bool[] PropRootFlags { get; private set; }
        public bool HasCliff { get; private set; }
        public int PathfindingWeight { get; private set; }
        public bool Empty { get { return !Has(PropType.OBJECT) && !Has(PropType.FLOOR) && !HasCliff; } }
        /// <summary>
        /// Tells the Populate function if it should create an element for this Tile
        /// when serializing.
        /// </summary>
        public bool IsWorthSaving { get { return Has(PropType.OBJECT) || Has(PropType.FLOOR); } }

        public bool IsImpenetrable
        {
            get
            {
                bool impenetrable = false;
                foreach(TileProp prop in InstalledProps)
                {
                    if (prop != null && prop.IsImpenetrable)
                    {
                        impenetrable = true;
                        break;
                    }
                }
                return impenetrable;
            }
        }

        public Tile(TileMap owner, TilePosition position)
        {
            Owner = owner;
            Position = position;

            int propTypeAmount = Enum.GetNames(typeof(PropType)).Length;
            InstalledProps = new TileProp[propTypeAmount];
            PropRootFlags = new bool[propTypeAmount];
        }

        public Tile(TileMap owner, int x, int y) : this(owner, new TilePosition(x, y)) {}

        public bool Has(PropType type)
        {
            return InstalledProps[(int) type] != null;
        }

        public TileProp GetProp(PropType type)
        {
            return InstalledProps[(int) type];
        }

        public bool IsRootForProp(PropType type)
        {
            return PropRootFlags[(int) type];
        }

        public bool CanInstall(PropType type)
        {
            return !Has(type) && !HasCliff;
        }

        public bool CanInstall(Type type)
        {
            if (type.IsSubclassOf(typeof(TileObjectBase)))
                return CanInstall(PropType.OBJECT);
            else if (type.IsSubclassOf(typeof(TileFloorBase)))
                return CanInstall(PropType.FLOOR);
            return false;
        }

        /// <summary>
        /// Installs the specified object on this tile.
        /// </summary>
        /// <param name="objectToInstall">The object to install</param>
        public bool InstallAsRoot(TileProp propToInstall, PropType type)
        {
            Vector2Int dimensions = propToInstall.OrientedDimensions;
            if ((dimensions.x > 1 || dimensions.y > 1) && !Owner.CanInstallPropAtArea(type, Position, dimensions))
            {
                return false;
            }

            if(CanInstall(type) && !propToInstall.Installed)
            {
                propToInstall.OnInstalledAt(this);
                InstalledProps[(int) type] = propToInstall;
                PropRootFlags[(int) type] = true;

                for (ushort x = 0; x < dimensions.x; ++x)
                {
                    for (ushort z = 0; z < dimensions.y; ++z)
                    {
                        if (x == 0 && z == 0)
                            continue;
                        TilePosition position = Position.GetOffset(x, z);
                        Tile tile = Owner.TileAt(position);
                        if (tile != null)
                            tile.Install(propToInstall, type);
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
                return InstallAsRoot(tileProp, PropType.OBJECT);
            else if (tileProp is TileFloorBase)
                return InstallAsRoot(tileProp, PropType.FLOOR);
            else
                return false;
        }

        void Install(TileProp propToInstall, PropType type)
        {
            if (CanInstall(type))
            {
                InstalledProps[(int) type] = propToInstall;
                PropRootFlags[(int) type] = false;
                Debug.Log("Installing not root at: " + Position + ", Has " + type + ": " + Has(type));
                Owner.OnModifyEvent(Position);
            }
        }

        public void Uninstall(PropType type)
        {
            Debug.Log("Uninstalling at " + Position + ", has type " + type + ": " + Has(type));
            if (Has(type))
            {
                if (IsRootForProp(type))
                {
                    Vector2Int dimensions = GetProp(type).OrientedDimensions;
                    GetProp(type).OnUninstalled();

                    for (ushort x = 0; x < dimensions.x; ++x)
                    {
                        for (ushort z = 0; z < dimensions.y; ++z)
                        {
                            if (x == 0 && z == 0)
                                continue;
                            TilePosition position = Position.GetOffset(x, z);
                            Tile tile = Owner.TileAt(position);
                            if (tile != null)
                                tile.Uninstall(type);
                        }
                    }
                    PropRootFlags[(int)type] = false;
                }
                else
                {
                    if(GetProp(type).InstalledAt != null)
                        GetProp(type).InstalledAt.Uninstall(type);
                }
                InstalledProps[(int) type] = null;
                Owner.OnModifyEvent(Position);
            }
        }

        public void Uninstall(TileProp tilePropToUninstall)
        {
            foreach (TileProp prop in InstalledProps)
            {
                if(prop != null && prop == tilePropToUninstall)
                {

                }
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

            XElement installedObjectElement = element.Element("InstalledObject");
            if (installedObjectElement != null)
            {
                tile.PropRootFlags[(int)PropType.OBJECT] = true;
                tile.InstalledProps[(int)PropType.OBJECT] = TileObjectBase.CreateAndParse(installedObjectElement);
            }

            XElement installedFloorElement = element.Element("InstalledFloor");
            if (installedFloorElement != null)
            {
                tile.PropRootFlags[(int)PropType.FLOOR] = true;
                tile.InstalledProps[(int)PropType.FLOOR] = TileFloorBase.CreateAndParse(installedFloorElement);
            }

            return tile;
        }

        /// <summary>
        /// This is called after every Tile has been created and parse, so that
        /// you can do stuff that required all tiles to be created.
        /// </summary>
        public void AfterParse()
        {
            for (int i = 0; i < InstalledProps.Length; ++i)
            {
                TileProp prop = InstalledProps[i];
                if (prop != null && IsRootForProp((PropType)i))
                {
                    InstalledProps[i] = null;
                    InstallAsRoot(prop);
                }
            }
        }

        /// <summary>
        /// This only runs when <see cref="IsWorthSaving"/> is true.
        /// </summary>
        /// <param name="element">The element to populate</param>
        public void Populate(XElement element)
        {
            element.SetAttributeValue("x", Position.X);
            element.SetAttributeValue("y", Position.Z);

            if (IsRootForProp(PropType.OBJECT))
            {
                XElement installedObjectElement = new XElement("InstalledObject");
                element.Add(installedObjectElement);
                GetProp(PropType.OBJECT).Populate(installedObjectElement);
            }

            if (IsRootForProp(PropType.FLOOR))
            {
                XElement installedFloorElement = new XElement("InstalledFloor");
                element.Add(installedFloorElement);
                GetProp(PropType.FLOOR).Populate(installedFloorElement);
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
            foreach (TileProp prop in InstalledProps)
                if (prop != null && !prop.CanGoIntoFrom(Position, direction))
                    return false;

            return true;
        }

        /// <summary>
        /// Returns if the passer can go out of this tile.
        /// More in <see cref="TileProp"/>
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool CanComeOutOfTowards(Pathfinding.MovementDirection direction)
        {
            foreach (TileProp prop in InstalledProps)
                if (prop != null && !prop.CanComeOutOfTowards(Position, direction))
                    return false;

            return true;
        }

        public bool CanSkimThrough()
        {
            foreach (TileProp prop in InstalledProps)
                if (prop != null && !prop.CanSkimThrough)
                    return false;

            return true;
        }
    }
}