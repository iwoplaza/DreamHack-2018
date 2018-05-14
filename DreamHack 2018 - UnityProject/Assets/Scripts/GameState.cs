using Game.Building;
using Game.Enemies;
using Game.Environment;
using Game.Items;
using Game.TileFloors;
using Game.TileObjects;
using Game.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Game
{
    public class GameState
    {
        public delegate void OnGameOverHandler(GameState gameState);
        OnGameOverHandler m_onGameOverHandlers;

        public int WorldIdentifier { get; private set; }
        public string WorldName { get; private set; }
        public string Seed { get; private set; }
        public TileMap TileMap { get; private set; }
        public List<Worker> Workers { get; private set; }
        public List<EnergyLeech> EnergyLeeches { get; private set; }
        public Focus Focus { get; private set; }
        public TimeSystem TimeSystem { get; private set; }
        public BuildCatalogue BuildCatalogue { get; private set; }
        public BuildModeManager BuildModeManager { get; private set; }
        public ItemStorage ItemStorage { get; private set; }
        public GameEnvironment GameEnvironment { get; private set; }
        public EnemySpawnerController EnemySpawnerController { get; private set; }

        public TilePosition MainGeneratorPosition { get; private set; }

        public GameState(int worldIdentifier, string worldName, GameEnvironment gameEnvironment)
        {
            WorldIdentifier = worldIdentifier;
            WorldName = worldName;
            GameEnvironment = gameEnvironment;

            TileMap = new TileMap(512, 512);
            Workers = new List<Worker>();
            EnergyLeeches = new List<EnergyLeech>();
            Focus = new Focus();
            TimeSystem = new TimeSystem();
            BuildCatalogue = new BuildCatalogue();
            BuildModeManager = new BuildModeManager(this);
            ItemStorage = new ItemStorage();
            EnemySpawnerController = new EnemySpawnerController();
        }

        public void Start()
        {
            Debug.Log("Starting GameState: " + WorldName);

            WorldMeshResource.UpdateMeshDictionary();
            WorldPopulationResource.UpdatePopulationDirectory();
            GameEnvironment.CliffThreshold = 0.5f;
            GameEnvironment.ChunkSize = new Vector2Int(15, 15);
            GameEnvironment.EmptyRadius = GameEnvironment.WorldSize.magnitude * 0.075f;
            EnemySpawnerController.InitializeComponent();
        }

        public void GenerateNew()
        {
            Seed = "glory";

            GameEnvironment.WorldSeed = Seed;
            GameEnvironment.WorldSize = new Vector2Int(TileMap.Width, TileMap.Length);
            GameEnvironment.GenerateMap();
            GameEnvironment.PopulateMapForNewWorld();
            GameEnvironment.AfterSetup();

            Worker worker1 = SpawnWorker(new Vector3(TileMap.Width / 2 + 2, 0, TileMap.Length / 2 + 1));
            worker1.FirstName = "James";
            worker1.LastName = "Marz";
            Worker worker2 = SpawnWorker(new Vector3(TileMap.Width / 2 - 2, 0, TileMap.Length / 2 - 1));
            worker2.FirstName = "Hugo";
            worker2.LastName = "Ivanovicz";

            ItemStorage.ItemStacks.Add(new ItemStack(Item.METAL, 500));

            TilePosition center = new TilePosition(TileMap.Width / 2, TileMap.Width / 2);
            MainGeneratorPosition = center.GetOffset(-1, -1);
            TileMap.InstallAt(new MainGeneratorTileObject(), MainGeneratorPosition);

            int radiusX = 5;
            int radiusZ = 5;

            for (int x = -radiusX + 1; x < radiusX - 1; ++x)
            {
                for (int z = -radiusZ + 1; z < radiusZ - 1; ++z)
                {
                    TileMap.InstallAt(new DefaultTileFloor(), center.GetOffset(x, z));
                }
            }

            for(int x = -radiusX + 1; x < radiusX - 1; ++x)
            {
                TileObjectBase tileObject = null;
                if (x == 0)
                    tileObject = new DoorTileObject();
                else if (x == 1)
                    tileObject = null;
                else
                    tileObject = new WallTileObject();

                if(tileObject != null)
                    TileMap.InstallAt(tileObject, center.GetOffset(x, -radiusZ));
            }

            for (int x = -radiusX + 1; x < radiusX - 1; ++x)
            {
                TileObjectBase tileObject = new WallTileObject();
                tileObject.Rotate(Direction.NEGATIVE_Z);
                TileMap.InstallAt(tileObject, center.GetOffset(x, radiusZ - 1));
            }

            for (int z = -radiusZ + 1; z < radiusZ - 1; ++z)
            {
                TileObjectBase tileObject = new WallTileObject();
                if (z == -radiusZ + 2 || z == radiusZ - 3)
                    tileObject.Variant = 1;
                tileObject.Rotate(Direction.POSITIVE_X);
                TileMap.InstallAt(tileObject, center.GetOffset(-radiusX, z));
            }

            for (int z = -radiusZ + 1; z < radiusZ - 1; ++z)
            {
                TileObjectBase tileObject = new WallTileObject();
                if (z == -radiusZ + 2 || z == radiusZ - 3)
                    tileObject.Variant = 1;
                tileObject.Rotate(Direction.NEGATIVE_X);
                TileMap.InstallAt(tileObject, center.GetOffset(radiusX - 1, z));
            }
        }

        public Worker SpawnWorker(Vector3 position)
        {
            GameObject workerPrefab = Resources.WorkerPrefab;
            if(workerPrefab == null)
            {
                Debug.LogError("The 'Worker' prefab is null. Something's wrong with 'Resources'");
                return null;
            }

            GameObject workerObject = UnityEngine.Object.Instantiate(workerPrefab, Vector3.zero, Quaternion.identity);
            if(workerObject != null)
            {
                Worker worker = workerObject.GetComponent<Worker>();
                if (worker == null)
                {
                    worker = workerObject.AddComponent<Worker>();
                }
                worker.Setup(TileMap);
                worker.Position = position;
                Workers.Add(worker);

                return worker;
            }

            return null;
        }

        public void OnWorkerDeath(Worker worker)
        {
            Workers.Remove(worker);
        }

        public EnergyLeech SpawnEnergyLeech(Vector3 position)
        {
            GameObject energyLeechPrefab = Resources.EnergyLeechPrefab;
            if (energyLeechPrefab == null)
            {
                Debug.LogError("The 'EnergyLeech' prefab is null. Something's wrong with 'Resources'");
                return null;
            }

            GameObject enemyObject = UnityEngine.Object.Instantiate(energyLeechPrefab, Vector3.zero, Quaternion.identity);
            if (enemyObject != null)
            {
                EnergyLeech enemy = enemyObject.GetComponent<EnergyLeech>();
                if (enemy == null)
                {
                    enemy = enemyObject.AddComponent<EnergyLeech>();
                }
                enemy.Setup(TileMap);
                enemy.Position = position;
                EnergyLeeches.Add(enemy);

                return enemy;
            }

            return null;
        }

        public void OnEnergyLeachDeath(EnergyLeech energyLeech)
        {
            EnergyLeeches.Remove(energyLeech);
        }

        public void Parse(XElement element)
        {
            if (element == null)
                return;

            XAttribute seed = element.Attribute("seed");
            if(seed != null)
                Seed = seed.Value;

            XElement mainGeneratorPosition = element.Element("MainGeneratorPosition");
            MainGeneratorPosition = new TilePosition(TileMap.Width/2, TileMap.Length/2);
            if(mainGeneratorPosition != null)
                MainGeneratorPosition.Parse(mainGeneratorPosition);

            XElement timeElement = element.Element("TimeSystem");
            TimeSystem.Parse(timeElement);

            XElement itemStorageElement = element.Element("ItemStorage");
            ItemStorage.Parse(itemStorageElement);

            GameEnvironment.WorldSeed = Seed;
            GameEnvironment.WorldSize = new Vector2Int(TileMap.Width, TileMap.Length);
            GameEnvironment.GenerateMap();
            XElement gameEnvironmentElement = element.Element("GameEnvironment");
            if(gameEnvironmentElement != null)
                GameEnvironment.Parse(gameEnvironmentElement);
            GameEnvironment.AfterSetup();

            XElement workersElement = element.Element("Workers");
            IEnumerable workerElements = workersElement.Elements("Worker");
            foreach (XElement workerElement in workerElements)
            {
                Worker worker = SpawnWorker(Vector3.zero);
                worker.Parse(workerElement);
            }

            XElement enemiesElement = element.Element("Enemies");
            IEnumerable energyLeechElements = enemiesElement.Elements("EnergyLeech");
            foreach (XElement energyLeechElement in energyLeechElements)
            {
                EnergyLeech enemy = SpawnEnergyLeech(Vector3.zero);
                enemy.Parse(energyLeechElement);
            }

            XElement tileMapElement = element.Element("TileMap");
            TileMap.Parse(tileMapElement);

            XElement enemySpawnerElement = element.Element("EnemySpawner");
            if(enemySpawnerElement != null)
                EnemySpawnerController.Parse(enemySpawnerElement);
        }

        public void Populate(XElement element)
        {
            element.SetAttributeValue("seed", Seed);

            Debug.Log("Saving MainGeneratorPosition...");
            XElement mainGeneratorPosition = new XElement("MainGeneratorPosition");
            element.Add(mainGeneratorPosition);
            MainGeneratorPosition.Populate(mainGeneratorPosition);
            Debug.Log("DONE");

            XElement timeElement = new XElement("TimeSystem");
            element.Add(timeElement);
            TimeSystem.Populate(timeElement);

            XElement itemStorageElement = new XElement("ItemStorage");
            element.Add(itemStorageElement);
            ItemStorage.Populate(itemStorageElement);

            Debug.Log("Saving workers...");
            XElement workersElement = new XElement("Workers");
            element.Add(workersElement);
            foreach (Worker worker in Workers)
            {
                XElement workerElement = new XElement("Worker");
                workersElement.Add(workerElement);
                worker.Populate(workerElement);
            }
            Debug.Log("DONE");

            Debug.Log("Saving enemies...");
            XElement enemiesElement = new XElement("Enemies");
            element.Add(enemiesElement);
            foreach (EnergyLeech enemy in EnergyLeeches)
            {
                XElement enemyElement = new XElement("EnergyLeech");
                enemiesElement.Add(enemyElement);
                enemy.Populate(enemyElement);
            }
            Debug.Log("DONE");

            XElement tileMapElement = new XElement("TileMap");
            element.Add(tileMapElement);
            TileMap.Populate(tileMapElement);

            XElement gameEnvironmentElement = new XElement("GameEnvironment");
            element.Add(gameEnvironmentElement);
            GameEnvironment.Populate(gameEnvironmentElement);

            XElement enemySpawnerElement = new XElement("EnemySpawner");
            element.Add(enemySpawnerElement);
            EnemySpawnerController.Populate(enemySpawnerElement);
        }

        public void Update()
        {
            TimeSystem.Update();
            EnemySpawnerController.UpdateComponent();
        }

        public void GameOver()
        {
            if(m_onGameOverHandlers != null)
                m_onGameOverHandlers(this);
        }

        public void RegisterOnGameOverHandler(OnGameOverHandler handler)
        {
            m_onGameOverHandlers += handler;
        }
    }
}