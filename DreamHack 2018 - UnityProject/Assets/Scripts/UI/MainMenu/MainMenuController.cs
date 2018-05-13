using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.UI.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] GameObject m_titleScreenPanel;
        [SerializeField] Button m_continueButton;

        public Panel CurrentPanel { get; private set; }
        public LoadGamePanel LoadGamePanel { get; private set; }

        void Awake()
        {
            ApplicationState.CreateIfDoesntExist();

            LoadGamePanel = GetComponentInChildren<LoadGamePanel>();
        }

        /// <summary>
        /// Called by the <see cref="ApplicationState"/>.
        /// </summary>
        public void Setup()
        {
            LoadGamePanel.Setup(this);
            OpenPanel(Panel.TITLE_SCREEN);
        }

        public void NewGame()
        {
            ApplicationState.Instance.CreateNewGame();
        }

        public void Continue()
        {
            OpenPanel(Panel.LOAD_GAME);
        }

        public void GoBack()
        {
            switch (CurrentPanel)
            {
                case Panel.LOAD_GAME:
                    OpenPanel(Panel.TITLE_SCREEN);
                    break;
            }
        }

        public void OpenPanel(Panel panel)
        {
            CurrentPanel = panel;

            m_titleScreenPanel.SetActive(false);
            LoadGamePanel.gameObject.SetActive(false);

            switch (panel)
            {
                case Panel.TITLE_SCREEN:
                    m_titleScreenPanel.SetActive(true);
                    m_continueButton.interactable = SaveController.SavedGames.Count > 0;
                    break;
                case Panel.LOAD_GAME:
                    LoadGamePanel.OnOpened();
                    break;
            }
        }

        public void Load(SavedGame savedGame)
        {
            ApplicationState.Instance.LoadGame(savedGame);
        }

        public enum Panel
        {
            TITLE_SCREEN, LOAD_GAME
        }
    }
}