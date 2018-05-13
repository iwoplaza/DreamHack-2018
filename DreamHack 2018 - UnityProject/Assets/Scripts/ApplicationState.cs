using Game.UI;
using Game.UI.MainMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class ApplicationState : MonoBehaviour
    {
        public static ApplicationState Instance;

        public SceneType CurrentScene;

        public static void CreateIfDoesntExist()
        {
            GameObject gameObject = new GameObject("ApplicationState");
            DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<ApplicationState>();
        }

        void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            if (SceneManager.GetActiveScene().buildIndex == Globals.MAIN_MENU_SCENE)
            {
                CurrentScene = SceneType.MAIN_MENU;
            }
            else if (SceneManager.GetActiveScene().buildIndex == Globals.GAME_SCENE)
            {
                CurrentScene = SceneType.GAME;
            }
            else
            {
                Debug.LogError("Unknown scene loaded");
                Application.Quit();
            }
        }

        // Use this for initialization
        void Start()
        {
            SaveController.Setup();

            if(CurrentScene == SceneType.MAIN_MENU)
            {
                SetupMainMenu();
            }

            if (CurrentScene == SceneType.GAME)
            {
                SetupGame();
                SetupGameHudAndCamera();
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        void SetupMainMenu()
        {
            GameObject mainMenuObject = GameObject.Find("MainMenu");
            if(mainMenuObject == null)
            {
                Debug.LogError("Couldn't find the MainMenu object in the scene. Setup aborted");
                return;
            }

            MainMenuController mainMenu = mainMenuObject.GetComponent<MainMenuController>();
            mainMenu.Setup();
        }

        void SetupGame()
        {
            GameObject worldControllerObject = GameObject.FindGameObjectWithTag("WorldController");
            if (worldControllerObject == null)
            {
                Debug.LogError("Couldn't find the WorldController object in the scene. Setup aborted");
                return;
            }

            WorldController worldController = worldControllerObject.GetComponent<WorldController>();
            int worldIdentifier = SaveController.FindFreeWorldIdentifier();
            worldController.SetupNewGame(worldIdentifier, "World " + worldIdentifier);
        }

        void SetupGame(SavedGame savedGame)
        {
            GameObject worldControllerObject = GameObject.FindGameObjectWithTag("WorldController");
            if (worldControllerObject == null)
            {
                Debug.LogError("Couldn't find the WorldController object in the scene. Setup aborted");
                return;
            }

            WorldController worldController = worldControllerObject.GetComponent<WorldController>();
            worldController.SetupFromState(savedGame);
        }

        void SetupGameHudAndCamera()
        {
            GameObject gameHudObject = GameObject.FindGameObjectWithTag("GameHUD");
            if (gameHudObject == null)
            {
                Debug.LogError("Couldn't find the GameHUD object in the scene. Setup aborted");
                return;
            }
            gameHudObject.GetComponent<GameHUD>().Setup();

            GameObject cameraObject = GameObject.FindGameObjectWithTag("CameraRig");
            if (cameraObject == null)
            {
                Debug.LogError("Couldn't find the CameraRig object in the scene. Setup aborted");
                return;
            }

            cameraObject.GetComponent<CameraController>().Setup();
        }

        public void GoToMainMenu()
        {
            StartCoroutine(GoToMainMenuCoroutine());
        }

        public void CreateNewGame()
        {
            StartCoroutine(LoadGameCoroutine(null));
        }

        public void LoadGame(SavedGame savedGame)
        {
            StartCoroutine(LoadGameCoroutine(savedGame));
        }

        IEnumerator GoToMainMenuCoroutine()
        {
            SceneManager.LoadScene(Globals.MAIN_MENU_SCENE);
            yield return null; // Wait one frame for the scene to load.
            SetupMainMenu();
        }

        IEnumerator LoadGameCoroutine(SavedGame savedGame)
        {
            SceneManager.LoadScene(Globals.GAME_SCENE);
            yield return null; // Wait one frame for the scene to load.
            CurrentScene = SceneType.GAME;
            if (savedGame != null)
                SetupGame(savedGame);
            else
                SetupGame();
            SetupGameHudAndCamera();
        }
    }
}