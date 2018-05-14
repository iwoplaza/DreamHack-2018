using System.Collections;
using System.Xml.Linq;
using UnityEngine;

namespace Game
{
    public class EnemySpawnerController
    {
        public float SpawnCooldown { get; private set; }

        public const float MaximumCooldown = 5; // 2 minutes

        public EnemySpawnerController()
        {
            SpawnCooldown = 0;
        }

        public void InitializeComponent()
        {
            WorldController worldController = WorldController.Instance;
        }

        public void UpdateComponent()
        {
            if(WorldController.Instance.MainState.TimeSystem.DayCount >= 0)
            {
                if(SpawnCooldown > 0)
                {
                    SpawnCooldown -= WorldController.Instance.MainState.TimeSystem.DeltaTime;

                    if(SpawnCooldown <= 0)
                    {
                        Spawn();
                        SpawnCooldown = MaximumCooldown;
                    }
                }
                else
                {
                    if (SpawnCooldown <= 0)
                    {
                        Spawn();
                        SpawnCooldown = MaximumCooldown;
                    }
                }
            }
        }

        void Spawn()
        {
            GameState gameState = WorldController.Instance.MainState;

            float radius = (gameState.TileMap.Width / 2) * 0.1F;
            float angle = Random.value * Mathf.PI * 2;
            Vector3 center = gameState.TileMap.Center;
            Vector3 position = center + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius;

            gameState.SpawnEnergyLeech(position);
        }

        public void Parse(XElement element)
        {
            XAttribute spawnCooldownAttrib = element.Attribute("spawnCooldown");
            if (spawnCooldownAttrib != null)
                SpawnCooldown = float.Parse(spawnCooldownAttrib.Value);
        }

        public void Populate(XElement element)
        {
            element.SetAttributeValue("spawnCooldown", SpawnCooldown);
        }
    }
}