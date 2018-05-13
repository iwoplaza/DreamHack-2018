using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.MainMenu
{
    public class LoadGamePanel : MonoBehaviour
    {
        [SerializeField] GameObject m_entryPrefab;
        [SerializeField] Transform m_entriesHolder;

        public MainMenuController MainMenuController { get; private set; }
        public List<SavedGameEntry> SavedGameEntries { get; private set; }

        public void Setup(MainMenuController mainMenuController)
        {
            MainMenuController = mainMenuController;
        }

        public void OnOpened()
        {
            if(SavedGameEntries != null)
            {
                foreach (SavedGameEntry entry in SavedGameEntries)
                {
                    Destroy(entry.gameObject);
                }
            }

            SavedGameEntries = new List<SavedGameEntry>();

            foreach (SavedGame savedGame in SaveController.SavedGames)
            {
                GameObject gameObject = Instantiate(m_entryPrefab, m_entriesHolder);
                SavedGameEntry entry = gameObject.GetComponent<SavedGameEntry>();
                entry.Setup(this, savedGame);
                SavedGameEntries.Add(entry);
            }

            gameObject.SetActive(true);
        }

        public void Load(SavedGame savedGame)
        {
            MainMenuController.Load(savedGame);
        }
    }
}