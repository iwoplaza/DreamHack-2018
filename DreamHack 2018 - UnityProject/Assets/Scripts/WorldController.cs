using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class WorldController : MonoBehaviour
    {
        public static WorldController Instance { get; private set; }

        [SerializeField]
        protected GameObject m_tilePrefab;

        public GameObject TilePrefab { get { return m_tilePrefab; } }
        public GameState MainState { get; private set; }

        private Queue<EnvironmentControlComponent> EnvControlToInitialize = new Queue<EnvironmentControlComponent>();

        void Awake()
        {
            Instance = this;
            Resources.LoadAll();
            MainState = new GameState();
        }

        // Use this for initialization
        void Start()
        {
            MainState.Start();
            SaveController.Instance.Load(MainState);
            MainState.TileMap.CreateGameObject();
        }

        public void AddEnvironmentControlObject(EnvironmentControlComponent component){
            EnvControlToInitialize.Enqueue(component);
        }

        // Update is called once per frame
        void Update()
        {
            while(EnvControlToInitialize.Count > 0){
                EnvControlToInitialize.Dequeue().Initialize();
            }
            MainState.Update();
        }

        void OnApplicationQuit()
        {
            Debug.Log("Shutting down...");
            SaveController.Instance.Save(MainState);
        }
    }
}