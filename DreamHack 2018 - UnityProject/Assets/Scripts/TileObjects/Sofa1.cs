﻿using Game.Pathfinding;
using Game.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TileObjects
{
    public class Sofa1 : TileObjectBase
    {
        public override string DisplayName { get { return "Sofa"; } }
        public override bool IsImpenetrable { get { return false; } }
        public override bool CanSkimThrough { get { return false; } }

        public override int MetalCost { get { return 5; } }

        AudioSource AudioSource { get; set; }

        public Sofa1()
        {
            Health = new HealthComponent(15);
        }


        public GameObject GetPrefab()
        {
            return Resources.TileObjectPrefabs.Find("Sofa1");
        }

        public override void ConstructGameObject()
        {
            if (!Installed)
                return;
            Vector3 origin = InstalledAt.Position.Vector3 + new Vector3(0.5F, 0, 0.5F);

            GameObject prefab = GetPrefab();
            if (prefab != null)
            {
                InstalledGameObject = UnityEngine.Object.Instantiate(prefab);
                InstalledGameObject.transform.SetPositionAndRotation(origin, Quaternion.Euler(0.0F, DirectionUtils.GetYRotation(Orientation), 0.0F));
            }
        }

        public override bool CanGoIntoFrom(TilePosition position, Pathfinding.MovementDirection entryDirection)
        {
            return false;
        }

        public override bool CanComeOutOfTowards(TilePosition position, Pathfinding.MovementDirection direction)
        {
            return true;
        }

        public override GameObject CreateTemporaryDisplay()
        {
            GameObject prefab = GetPrefab();
            if (prefab != null)
            {
                return UnityEngine.Object.Instantiate(prefab);
            }

            return null;
        }

        public override void Damage(int damage, GameObject attacker)
        {
            base.Damage(damage, attacker);
            PlayDamageSound();
        }

        void PlayDamageSound()
        {
            if(AudioSource != null)
            {
                AudioClip clip = Resources.Sounds.Find("MetalicPunch");
                if (clip != null)
                {
                    AudioSource.PlayOneShot(clip);
                }
            }
        }
    }
}