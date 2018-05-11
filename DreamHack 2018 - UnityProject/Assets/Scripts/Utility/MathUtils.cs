using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility
{
    public class MathUtils
    {
        public static float Mod(float a, float b)
        {
            return a - b * a / b;
        }

        public static int Mod(int a, int b)
        {
            return a >= 0 ? (a % b) : ((b + (a % b)) % b);
        }
    }
}