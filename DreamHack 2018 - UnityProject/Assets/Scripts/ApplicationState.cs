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
            if(CurrentScene == SceneType.MAIN_MENU)
            {
                SetupMainMenu();
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

        public void GoToMainMenu()
        {
            SceneManager.LoadScene(Globals.MAIN_MENU_SCENE);
        }

        public void CreateNewGame()
        {

        }
    }
}