using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.MainMenu
{
    public class SavedGameEntry : MonoBehaviour
    {
        [SerializeField] Text m_worldNameText;

        public LoadGamePanel LoadGamePanel { get; private set; }
        public SavedGame SavedGame { get; private set; }

        public void Setup(LoadGamePanel loadGamePanel, SavedGame savedGame)
        {
            LoadGamePanel = loadGamePanel;
            SavedGame = savedGame;

            m_worldNameText.text = savedGame.WorldName;
        }

        public void Load()
        {
            LoadGamePanel.Load(SavedGame);
        }
    }
}