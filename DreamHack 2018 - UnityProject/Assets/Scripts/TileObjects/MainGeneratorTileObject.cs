using Game.Pathfinding;
using Game.Scene;
using Game.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TileObjects
{
    public class MainGeneratorTileObject : TileObjectBase
    {
        public override string DisplayName { get { return "Main Generator"; } }
        public override bool IsImpenetrable { get { return true; } }
        public override bool CanSkimThrough { get { return false; } }
        public override int Width { get { return 2; } }
        public override int Length { get { return 2; } }

        AudioSource AudioSource { get; set; }

        public MainGeneratorTileObject()
        {
            Health = new HealthComponent(200);
        }

        public override bool CanGoIntoFrom(TilePosition globalPosition, MovementDirection entryDirection)
        {
            return false;
        }

        public override bool CanComeOutOfTowards(TilePosition globalPosition, MovementDirection direction)
        {
            return false;
        }

        public GameObject GetPrefab()
        {
            return Resources.TileObjectPrefabs.Find("MainGenerator");
        }

        public override void ConstructGameObject()
        {
            if (!Installed)
                return;

            Vector2Int dimensions = OrientedDimensions;
            Vector3 origin = InstalledAt.Position.Vector3 + new Vector3(dimensions.x / 2.0F, 0.0F, dimensions.y / 2.0F);

            GameObject prefab = GetPrefab();
            if (prefab != null)
            {
                InstalledGameObject = UnityEngine.Object.Instantiate(prefab);
                InstalledGameObject.transform.SetPositionAndRotation(origin, Quaternion.Euler(0.0F, DirectionUtils.GetYRotation(Orientation), 0.0F));
                AudioSource = InstalledGameObject.AddComponent<AudioSource>();

                MainGeneratorComponent component = InstalledGameObject.AddComponent<MainGeneratorComponent>();
                component.Setup(this);
            }
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

        public override void OnHealthDepleted()
        {
            base.OnHealthDepleted();

            WorldController.Instance.MainState.GameOver();
        }

        public override void Damage(int damage, GameObject attacker)
        {
            base.Damage(damage, attacker);
            PlayDamageSound();
        }

        void PlayDamageSound()
        {
            if (AudioSource != null)
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