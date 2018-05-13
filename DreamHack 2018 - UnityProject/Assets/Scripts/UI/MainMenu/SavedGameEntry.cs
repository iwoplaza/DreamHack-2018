using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.MainMenu
{
    public class SavedGameEntry : MonoBehaviour
    {
       [SerializeField] Text m_worldNameText;

        public MainMenuController MainMenuController { get; private set; }

        void Setup(MainMenuController mainMenuController)
        {
            MainMenuController = mainMenuController;
        }

        public void Load()
        {

        }
    }
}