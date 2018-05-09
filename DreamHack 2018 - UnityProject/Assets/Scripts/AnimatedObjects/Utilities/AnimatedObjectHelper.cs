using UnityEngine;

namespace Game
{
    public static class AnimatedObjectHelper
    {        
        public static float CosineInterpolation(float f1, float f2, float t)
        {
            t = Mathf.Clamp01(t);
            float t1 = (1f - Mathf.Cos(t * Mathf.PI))/2f;
            return (f1 * (1f - t1) + (f2 * t1));
        }

        public static Vector3 Vector3CosineInterpolation(Vector3 v1, Vector3 v2, float t)
        {
            float x = CosineInterpolation(v1.x,v2.x,t);
            float y = CosineInterpolation(v1.y,v2.y,t);
            float z = CosineInterpolation(v1.z,v2.z,t);

            return new Vector3(x,y,z);
        }
    }
}