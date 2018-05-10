using Game.Building;
using Game.TileObjects;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Game
{
    public class GameState
    {
        public TileMap TileMap { get; private set; }
        public List<Worker> Workers { get; private set; }
        public Focus Focus { get; private set; }
        public TimeSystem TimeSystem { get; private set; }
        public BuildCatalogue BuildCatalogue { get; private set; }
        public BuildModeManager BuildModeManager { get; private set; }

        public GameState()
        {
            TileMap = new TileMap(512, 512);
            Workers = new List<Worker>();
            Focus = new Focus();
            TimeSystem = new TimeSystem();
            BuildCatalogue = new BuildCatalogue();
            BuildModeManager = new BuildModeManager(this);
        }

        public void Start()
        {
            Worker worker1 = SpawnWorker();
        }

        public void Update()
        {
            TimeSystem.Update();
        }

        public Worker SpawnWorker()
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
                worker.Position = new Vector3(4, 1, 4);

                return worker;
            }

            return null;
        }

        public void Parse(XElement element)
        {
            if (element == null)
                return;

            XElement tileMapElement = element.Element("TileMap");
            TileMap.Parse(tileMapElement);

            XElement timeElement = element.Element("TimeSystem");
            TimeSystem.Parse(timeElement);
        }

        public void Populate(XElement element)
        {
            XElement tileMapElement = new XElement("TileMap");
            element.Add(tileMapElement);
            TileMap.Populate(tileMapElement);

            XElement timeElement = new XElement("TimeSystem");
            element.Add(timeElement);
            TimeSystem.Populate(timeElement);
        }
    }
}