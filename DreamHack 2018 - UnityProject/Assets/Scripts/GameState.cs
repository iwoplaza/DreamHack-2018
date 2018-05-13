using Game.Building;
using Game.Environment;
using Game.Items;
using Game.TileObjects;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Game
{
    public class GameState
    {
        public int WorldIdentifier { get; private set; }
        public string WorldName { get; private set; }
        public string Seed { get; private set; }
        public TileMap TileMap { get; private set; }
        public List<Worker> Workers { get; private set; }
        public Focus Focus { get; private set; }
        public TimeSystem TimeSystem { get; private set; }
        public BuildCatalogue BuildCatalogue { get; private set; }
        public BuildModeManager BuildModeManager { get; private set; }
        public ItemStorage ItemStorage { get; private set; }
        public GameEnvironment GameEnvironment { get; private set; }

        public GameState(int worldIdentifier, string worldName, GameEnvironment gameEnvironment)
        {
            WorldIdentifier = worldIdentifier;
            WorldName = worldName;
            GameEnvironment = gameEnvironment;

            TileMap = new TileMap(512, 512);
            Workers = new List<Worker>();
            Focus = new Focus();
            TimeSystem = new TimeSystem();
            BuildCatalogue = new BuildCatalogue();
            BuildModeManager = new BuildModeManager(this);
            ItemStorage = new ItemStorage();
        }

        public void Start()
        {
            Debug.Log("Starting GameState: " + WorldName);

            WorldMeshResource.UpdateMeshDictionary();
            WorldPopulationResource.UpdatePopulationDirectory();
            GameEnvironment.CliffThreshold = 0.5f;
            GameEnvironment.ChunkSize = new Vector2Int(15, 15);
            GameEnvironment.EmptyRadius = GameEnvironment.WorldSize.magnitude * 0.075f;
        }

        public void GenerateNew()
        {
            Seed = "glory";

            GameEnvironment.WorldSeed = Seed;
            GameEnvironment.WorldSize = new Vector2Int(TileMap.Width, TileMap.Height);
            GameEnvironment.GenerateMap();
            GameEnvironment.PopulateMapForNewWorld();
            GameEnvironment.AfterSetup();

            Worker worker1 = SpawnWorker(new Vector3(TileMap.Width / 2 + 2, 0, TileMap.Height / 2 + 1));
            worker1.FirstName = "James";
            worker1.LastName = "Marz";
            Worker worker2 = SpawnWorker(new Vector3(TileMap.Width / 2 - 2, 0, TileMap.Height / 2 - 1));
            worker2.FirstName = "Hugo";
            worker2.LastName = "Ivanovicz";

            TilePosition center = new TilePosition(TileMap.Width / 2, TileMap.Width / 2);
            TileMap.InstallAt(new MainGeneratorTileObject(), center.GetOffset(-1, -1));
        }

        public Worker SpawnWorker(Vector3 position)
        {
            GameObject workerPrefab = Resources.WorkerPrefab;
            if(workerPrefab == null)
            {
                Debug.LogError("The 'Worker' prefab is null. Something's wrong with 'Resources'");
                return null;
            }

            GameObject workerObject = Object.Instantiate(workerPrefab, Vector3.zero, Quaternion.identity);
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

        public void Parse(XElement element)
        {
            if (element == null)
                return;

            XAttribute seed = element.Attribute("seed");
            if(seed != null)
                Seed = seed.Value;

            XElement tileMapElement = element.Element("TileMap");
            TileMap.Parse(tileMapElement);

            XElement timeElement = element.Element("TimeSystem");
            TimeSystem.Parse(timeElement);

            XElement itemStorageElement = element.Element("ItemStorage");
            ItemStorage.Parse(itemStorageElement);

            GameEnvironment.WorldSeed = Seed;
            GameEnvironment.WorldSize = new Vector2Int(TileMap.Width, TileMap.Height);
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
        }

        public void Populate(XElement element)
        {
            element.SetAttributeValue("seed", Seed);

            XElement timeElement = new XElement("TimeSystem");
            element.Add(timeElement);
            TimeSystem.Populate(timeElement);

            XElement itemStorageElement = new XElement("ItemStorage");
            element.Add(itemStorageElement);
            ItemStorage.Populate(itemStorageElement);

            XElement workersElement = new XElement("Workers");
            element.Add(workersElement);
            foreach (Worker worker in Workers)
            {
                XElement workerElement = new XElement("Worker");
                workersElement.Add(workerElement);
                worker.Populate(workerElement);
            }

            XElement tileMapElement = new XElement("TileMap");
            element.Add(tileMapElement);
            TileMap.Populate(tileMapElement);

            XElement gameEnvironmentElement = element.Element("GameEnvironment");
            element.Add(gameEnvironmentElement);
            if (gameEnvironmentElement != null)
                GameEnvironment.Populate(gameEnvironmentElement);
        }

        public void Update()
        {
            TimeSystem.Update();
        }
    }
}