using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pathfinding
{
    public enum MovementDirection
    {
        NONE, POSITIVE_Z, POSITIVE_Z_POSITIVE_X, POSITIVE_X, NEGATIVE_Z_POSITIVE_X, NEGATIVE_Z, NEGATIVE_Z_NEGATIVE_X, NEGATIVE_X, POSITIVE_Z_NEGATIVE_X, 
    }

    public class MovementDirectionUtils
    {
        public static MovementDirection RotateCW(MovementDirection direction)
        {
            if (direction == MovementDirection.NONE)
                return MovementDirection.NONE;

            return (MovementDirection)(((int)direction % 8) + 1);
        }

        public static MovementDirection RotateCCW(MovementDirection direction)
        {
            if (direction == MovementDirection.NONE)
                return MovementDirection.NONE;

            return ((direction - 2 % 8) + 1);
        }

        public static MovementDirection OrientTowardsInverse(MovementDirection input, MovementDirection direction)
        {
            if (direction == MovementDirection.NONE || input == MovementDirection.NONE)
                return MovementDirection.NONE;

            int rotation = (int)direction - 1;

            MovementDirection result = (MovementDirection)(((int)input - 1 - rotation)%8 + 1);
            return result;
        }

        public static MovementDirection NewFrom(Direction dir)
        {
            switch(dir)
            {
                case Direction.POSITIVE_Z:
                    return MovementDirection.POSITIVE_Z;
                case Direction.POSITIVE_X:
                    return MovementDirection.POSITIVE_X;
                case Direction.NEGATIVE_Z:
                    return MovementDirection.NEGATIVE_Z;
                case Direction.NEGATIVE_X:
                    return MovementDirection.NEGATIVE_X;
                default:
                    return MovementDirection.NONE;
            }
        }
    }
}