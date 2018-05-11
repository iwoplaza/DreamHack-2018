using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility
{
    public class AudioUtils
    {
        public static AudioClip GetRandom(AudioClip[] array)
        {
            if(array.Length <= 0)
                return null;

            int index = Mathf.FloorToInt(Random.value * array.Length);
            if (index >= array.Length)
                index = array.Length - 1;

            return array[index];
        }
    }
}