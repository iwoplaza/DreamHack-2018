using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Animations;

namespace Game
{
    public abstract class TileProp
    {
        public Tile InstalledAt { get; private set; }
        public bool Installed { get { return InstalledAt != null; } }

        public GameObject InstalledGameObject { get; set; }
        public Direction Orientation { get; private set; }
        public int Variant { get; set; }

        public abstract string DisplayName { get; }
        /// <summary>
        /// Determines if something/someone can pass through this TileObject.
        /// Used for Path Finding.
        /// </summary>
        /// <param name="entryDirection">The direction it's coming from.
        ///                              (relative to the passer, not the object)</param>
        /// <returns>Whether or not the passer can pass.</returns>
        public abstract bool CanGoIntoFrom(TilePosition globalPosition, Pathfinding.MovementDirection entryDirection);
        public abstract bool CanComeOutOfTowards(TilePosition globalPosition, Pathfinding.MovementDirection entryDirection);
        /// <summary>
        /// Determines if something can go to an adjecent tile touching the edge of this tile.
        /// </summary>
        public virtual bool CanSkimThrough { get { return true; } }

        public abstract bool IsImpenetrable { get; }

        /// <summary>
        /// Used for determining how passable compared to others this object is.
        /// </summary>
        public virtual float PassWeight { get { return 1; } }

        /// <summary>
        /// Determines if this TileObject can be removed or destroyed in any way,
        /// by the player or the environment.
        /// </summary>
        public virtual bool IsStatic { get { return false; } }
        public virtual int Width { get { return 1; } }
        public virtual int Length { get { return 1; } }
        public Vector2Int OrientedDimensions {
            get
            {
                return DirectionUtils.IsAlignedWith(Orientation, Axis.Z) ? new Vector2Int(Width, Length) : new Vector2Int(Length, Width);
            }
        }

        public TileProp()
        {
            InstalledAt = null;
            Orientation = Direction.POSITIVE_Z;
        }

        public void OnInstalledAt(Tile targetTile)
        {
            if (InstalledAt == null)
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
            InstalledAt = null;
            RemoveGameObject();
        }

        public virtual void Parse(XElement element, Tile optionalRootTile = null)
        {
            XAttribute variantAttrib = element.Attribute("variant");
            XAttribute orientationAttrib = element.Attribute("orientation");

            if (variantAttrib != null)
                Variant = int.Parse(variantAttrib.Value);

            if (orientationAttrib != null)
            {
                Orientation = (Direction)int.Parse(orientationAttrib.Value);
                if (Orientation == Direction.NONE)
                    Orientation = Direction.POSITIVE_Z;
            }

            if (optionalRootTile != null)
            {
                InstalledAt = optionalRootTile;
                ConstructGameObject();
            }
        }

        public virtual void Populate(XElement element)
        {
            String typeName = GetType().FullName;
            element.SetAttributeValue("type", typeName);
            element.SetAttributeValue("orientation", (int)Orientation);
            element.SetAttributeValue("variant", Variant);
        }

        public abstract void ConstructGameObject();
        public virtual void RemoveGameObject()
        {
            UnityEngine.Object.Destroy(InstalledGameObject);
        }
        public abstract GameObject CreateTemporaryDisplay();

        public virtual void Rotate(Direction direction)
        {
            if (!Installed)
            {
                if (direction != Direction.NONE)
                {
                    Orientation = direction;
                    if (InstalledGameObject != null)
                    {
                        InstalledGameObject.transform.rotation = Quaternion.Euler(0.0F, DirectionUtils.GetYRotation(Orientation), 0.0F);
                    }
                }
            }
        }

        public virtual void RotateLeft()
        {
            Rotate(DirectionUtils.RotateCCW(Orientation));
        }

        public virtual void RotateRight()
        {
            Rotate(DirectionUtils.RotateCW(Orientation));
        }

        public TilePosition GlobalToLocal(TilePosition global)
        {
            TilePosition local = global - InstalledAt.Position;
            return TilePosition.RotateInBlock(local, Width, Length, Orientation);
        }
    }
}