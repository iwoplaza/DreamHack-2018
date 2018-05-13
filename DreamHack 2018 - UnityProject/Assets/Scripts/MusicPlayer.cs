using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MusicPlayer : MonoBehaviour
    {
        public AudioSource AudioSource { get; private set; }

        void Start()
        {
            AudioClip music = Resources.Sounds.Find("Serene");

            if (music != null)
            {
                AudioSource = gameObject.AddComponent<AudioSource>();
                AudioSource.loop = true;

                AudioSource.clip = music;
            }
            else
            {
                Debug.LogError("Couldn't find music.");
            }

            if (AudioSource != null)
                AudioSource.Play();
        }
    }
}