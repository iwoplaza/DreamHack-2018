using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}