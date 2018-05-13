using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Game
{
    public enum Direction
    {
        NONE, NEGATIVE_X, POSITIVE_X, NEGATIVE_Y, POSITIVE_Y, NEGATIVE_Z, POSITIVE_Z
    }

    public class DirectionUtils
    {
        public static Direction RotateCW(Direction input)
        {
            switch (input)
            {
                case Direction.NEGATIVE_X:
                    return Direction.POSITIVE_Z;
                case Direction.POSITIVE_Z:
                    return Direction.POSITIVE_X;
                case Direction.POSITIVE_X:
                    return Direction.NEGATIVE_Z;
                case Direction.NEGATIVE_Z:
                    return Direction.NEGATIVE_X;
            }

            return Direction.NONE;
        }

        public static Direction RotateCCW(Direction input)
        {
            switch (input)
            {
                case Direction.NEGATIVE_X:
                    return Direction.NEGATIVE_Z;
                case Direction.NEGATIVE_Z:
                    return Direction.POSITIVE_X;
                case Direction.POSITIVE_X:
                    return Direction.POSITIVE_Z;
                case Direction.POSITIVE_Z:
                    return Direction.NEGATIVE_X;
            }

            return Direction.NONE;
        }

        public static float GetYRotation(Direction direction)
        {
            switch (direction)
            {
                case Direction.POSITIVE_Z:
                    return 0.0F;
                case Direction.POSITIVE_X:
                    return 90.0F;
                case Direction.NEGATIVE_Z:
                    return 180.0F;
                case Direction.NEGATIVE_X:
                    return 270.0F;
            }

            return 0.0F;
        }

        public static Direction OrientTowards(Direction input, Direction direction)
        {
            switch (direction)
            {
                case Direction.POSITIVE_X:
                    return RotateCW(input);
                case Direction.NEGATIVE_Z:
                    return RotateCW(RotateCW(input));
                case Direction.NEGATIVE_X:
                    return RotateCCW(input);
            }

            return input;
        }

        public static Direction OrientTowardsInverse(Direction input, Direction direction)
        {
            switch (direction)
            {
                case Direction.POSITIVE_X:
                    return RotateCCW(input);
                case Direction.NEGATIVE_Z:
                    return RotateCW(RotateCW(input));
                case Direction.NEGATIVE_X:
                    return RotateCW(input);
            }

            return input;
        }

        public static bool IsAlignedWith(Direction d, Axis axis)
        {
            if (axis == Axis.X)
                return d == Direction.NEGATIVE_X || d == Direction.POSITIVE_X;
            if (axis == Axis.Y)
                return d == Direction.NEGATIVE_Y || d == Direction.POSITIVE_Y;
            if (axis == Axis.Z)
                return d == Direction.NEGATIVE_Z || d == Direction.POSITIVE_Z;
            return false;
        }
    }
}