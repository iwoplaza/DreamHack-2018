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

        public GameState()
        {
            TileMap = new TileMap(10, 10);
            Workers = new List<Worker>();
            Focus = new Focus();
            TimeSystem = new TimeSystem();
        }

        public void Start()
        {
            Worker worker1 = SpawnWorker();
            Focus.On(worker1);
        }

        public void Update(){
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

            GameObject workerObject = Object.Instantiate(workerPrefab, new Vector3(4, 1, 4), Quaternion.identity);
            if(workerObject != null)
            {
                Worker worker = workerObject.GetComponent<Worker>();
                if (worker == null)
                {
                    worker = workerObject.AddComponent<Worker>();
                }

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