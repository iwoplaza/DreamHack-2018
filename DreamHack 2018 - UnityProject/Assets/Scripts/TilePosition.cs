﻿using Game.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class TilePosition
    {
        [SerializeField] protected ushort m_x;
        [SerializeField] protected ushort m_y;

        public ushort X { get { return m_x; } set { m_x = value; } }
        public ushort Z { get { return m_y; } set { m_y = value; } }

        public Vector3 Vector3
        {
            get
            {
                return new Vector3(X, 0, Z);
            }
        }

        public TilePosition(TilePosition position)
        {
            X = position.X;
            Z = position.Z;
        }

        public TilePosition(ushort x, ushort z)
        {
            X = x;
            Z = z;
        }

        public TilePosition(int x, int z)
        {
            X = x < 0 ? (ushort)0u : (ushort)x;
            Z = z < 0 ? (ushort)0u : (ushort)z;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            TilePosition other = obj as TilePosition;
            
            return (other.X == X && other.Z == Z);
        }
        
        public override int GetHashCode()
        {
            System.Int32 hashCode = X;
            hashCode <<= 16;
            hashCode |= Z;
            return hashCode;
        }

        public void Offset(MovementDirection direction)
        {
            if (direction == MovementDirection.POSITIVE_Z ||
                direction == MovementDirection.POSITIVE_Z_NEGATIVE_X ||
                direction == MovementDirection.POSITIVE_Z_POSITIVE_X)
            {
                Z++;
            }
            if (direction == MovementDirection.NEGATIVE_Z ||
                direction == MovementDirection.NEGATIVE_Z_NEGATIVE_X ||
                direction == MovementDirection.NEGATIVE_Z_POSITIVE_X)
            {
                Z--;
            }
            if (direction == MovementDirection.POSITIVE_X ||
                direction == MovementDirection.NEGATIVE_Z_POSITIVE_X ||
                direction == MovementDirection.POSITIVE_Z_POSITIVE_X)
            {
                X++;
            }
            if (direction == MovementDirection.NEGATIVE_X ||
                direction == MovementDirection.NEGATIVE_Z_NEGATIVE_X ||
                direction == MovementDirection.POSITIVE_Z_NEGATIVE_X)
            {
                X--;
            }
        }

        public TilePosition GetOffset(MovementDirection direction)
        {
            TilePosition position = new TilePosition(this);
            position.Offset(direction);
            return position;
        }

        public static bool operator ==(TilePosition a, TilePosition b)
        {
            if((object)a == null)
            {
                if((object)b == null)
                    return true;
                else
                    return false;                
            }
            return a.Equals(b);
        }

        public static bool operator !=(TilePosition a, TilePosition b)
        {
            if((object)a == null)
            {
                if((object)b == null)
                    return false;
                else
                    return true;
            }
            return !a.Equals(b);
        }

        public static TilePosition operator +(TilePosition a, TilePosition b)
        {
            return new TilePosition(a.X + b.X, a.Z + b.Z);
        }

        public static TilePosition operator -(TilePosition a, TilePosition b)
        {
            return new TilePosition(a.X - b.X, a.Z - b.Z);
        }

        public override string ToString()
        {
            return "(" + X.ToString() + ", " + Z.ToString() + ")";
        }

        public static TilePosition FromWorldPosition(Vector3 worldPos)
        {
            return new TilePosition((short)Mathf.FloorToInt(worldPos.x), (short)Mathf.FloorToInt(worldPos.z));
        }

        public static TilePosition RotateInBlock(TilePosition local, int width, int length, Direction direction)
        {
            switch(direction)
            {
                case Direction.POSITIVE_X:
                    return new TilePosition(local.Z, length - local.X);
                case Direction.NEGATIVE_Z:
                    return new TilePosition(width - local.X, length - local.Z);
                case Direction.NEGATIVE_X:
                    return new TilePosition(width - local.Z, local.X);
                default:
                    return local;
            }
        }
    }
}