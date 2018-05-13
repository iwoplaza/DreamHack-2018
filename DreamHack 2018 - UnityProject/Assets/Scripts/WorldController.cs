using Game.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class WorldController : MonoBehaviour
    {
        public static WorldController Instance { get; private set; }

        public GameState MainState { get; private set; }
        public PlayMode Mode { get; private set; }

        private Queue<EnvironmentControlComponent> EnvControlToInitialize = new Queue<EnvironmentControlComponent>();

        void Awake()
        {
            Instance = this;
            Resources.LoadAll();

            GameEnvironment gameEnvironment = GetComponentInChildren<GameEnvironment>();
            if (gameEnvironment != null)
            {
                MainState = new GameState(gameEnvironment);
            }
            else
            {
                Debug.LogError("The GameEnvironment child GameObject is missing from the WorldController");
            }
        }

        // Use this for initialization
        void Start()
        {
            MainState.Start();
            SaveController.Load(MainState);
            MainState.TileMap.CreateMapComponent();
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
            SaveController.Save(MainState);
        }

        public void AddEnvironmentControlObject(EnvironmentControlComponent component)
        {
            EnvControlToInitialize.Enqueue(component);
        }

        public delegate void ModeChangeHandler(PlayMode mode);
        ModeChangeHandler m_modeChangeHandlers;

        public void RegisterModeChangeHandler(ModeChangeHandler handler)
        {
            m_modeChangeHandlers += handler;
        }

        public void SwitchMode(PlayMode newMode)
        {
            Mode = newMode;

            if (m_modeChangeHandlers != null)
                m_modeChangeHandlers(newMode);
        }

        public void ToggleBuildMode()
        {
            if(Mode == PlayMode.BUILD_MODE)
            {
                SwitchMode(PlayMode.DEFAULT_MODE);
            }
            else
            {
                SwitchMode(PlayMode.BUILD_MODE);
            }
        }
    }
}