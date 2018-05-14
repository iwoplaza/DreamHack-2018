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

        public bool Initialised { get; private set; }

        [SerializeField] GameObject m_physicsPlane;

        void Awake()
        {
            ApplicationState.CreateIfDoesntExist();
            Initialised = false;

            Instance = this;
        }

        /// <summary>
        /// Called by <see cref="ApplicationState"/>
        /// </summary>
        public void SetupNewGame(int worldIdentifier, string worldName)
        {
            GameEnvironment gameEnvironment = GetComponentInChildren<GameEnvironment>();

            if (gameEnvironment != null)
                MainState = new GameState(worldIdentifier, worldName, gameEnvironment);
            else
            {
                Debug.LogError("The GameEnvironment child GameObject is missing from the WorldController");
                return;
            }
            MainState.Start();
            MainState.GenerateNew();
            MainState.TileMap.CreateMapComponent();

            AfterSetup();
        }

        /// <summary>
        /// Called by <see cref="ApplicationState"/>
        /// </summary>
        public void SetupFromState(SavedGame savedGame)
        {
            GameEnvironment gameEnvironment = GetComponentInChildren<GameEnvironment>();

            if (gameEnvironment != null)
                MainState = new GameState(savedGame.WorldIdentifier, savedGame.WorldName, gameEnvironment);
            else
            {
                Debug.LogError("The GameEnvironment child GameObject is missing from the WorldController");
                return;
            }
            MainState.Start();
            SaveController.Load(MainState);
            MainState.TileMap.CreateMapComponent();

            AfterSetup();
        }

        void AfterSetup()
        {
            EnvironmentControlComponent[] environmentControls = GetComponentsInChildren<EnvironmentControlComponent>();
            foreach (EnvironmentControlComponent component in environmentControls)
            {
                component.Initialize();
            }

            m_physicsPlane.transform.position = new Vector3(MainState.TileMap.Width / 2, 0, MainState.TileMap.Length / 2);
            m_physicsPlane.transform.localScale = new Vector3(MainState.TileMap.Width / 10, 0, MainState.TileMap.Length / 10);

            Initialised = true;
            Debug.Log("WorldController initialised.");
        }

        // Update is called once per frame
        void Update()
        {
            if (Initialised)
            {
                MainState.Update();
            }
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