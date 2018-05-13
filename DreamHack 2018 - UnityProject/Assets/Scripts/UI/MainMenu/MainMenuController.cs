using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        void Awake()
        {
            ApplicationState.CreateIfDoesntExist();
        }

        /// <summary>
        /// Called by the <see cref="ApplicationState"/>.
        /// </summary>
        public void Setup()
        {

        }

        public void NewGame()
        {
            SceneManager.LoadScene(Globals.GAME_SCENE);
        }
    }
}