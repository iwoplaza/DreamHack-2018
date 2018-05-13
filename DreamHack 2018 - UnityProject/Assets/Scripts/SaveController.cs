using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace Game
{
    public class SaveController : MonoBehaviour
    {
        private const string SAVE_FILE_NAME = "save0.xml";
        private const string SAVE_DIRECTORY = "Saves/";

        public static List<SavedGame> SavesGames { get; private set; }

        /// <summary>
        /// Called by the <see cref=">ApplicationState"/>
        /// </summary>
        public static void Setup()
        {
            SavesGames = new List<SavedGame>();
            string fileDirectoryPath = Path.Combine(Application.persistentDataPath, SAVE_DIRECTORY);

            string[] fileNames = Directory.GetFiles(fileDirectoryPath, "save_*.xml");
            foreach (string fileName in fileNames)
            {
                
            }
        }

        public static void Save(GameState gameStateToSave)
        {
            XElement document = new XElement("root");
            XElement gameStateElement = new XElement("GameState");
            document.Add(gameStateElement);

            gameStateToSave.Populate(gameStateElement);

            string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            File.WriteAllText(filePath, document.ToString());
        }

        public static bool Load(GameState gameStateToLoad)
        {
            string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

            if (File.Exists(filePath))
            {
                XElement document = XElement.Parse(File.ReadAllText(filePath));
                XElement gameStateElement = document.Element("GameState");

                Debug.Log("Loading game state...");

                try
                {
                    gameStateToLoad.Parse(gameStateElement);
                    return true;
                }
                catch (FormatException)
                {
                    Debug.LogError("Tried to load a map with a wrong format.");
                }
                catch (ArgumentNullException)
                {
                    Debug.LogError("Tried to load a map with a wrong format.");
                }
            }

            return false;
        }

        public static string GetFileNameForIndex(int index)
        {
            return "save" + index + ".xml";
        }

        public static string FindFreeSaveName()
        {
            int freeSaveIndex = 0;

            string fileDirectoryPath = Path.Combine(Application.persistentDataPath, SAVE_DIRECTORY);
            string[] fileNames = Directory.GetFiles(fileDirectoryPath, "save*.xml");
            int index = 0;

            bool free = false;
            while (!free)
            {
                free = true;
                foreach(string fileName in fileNames)
                {
                    if(fileName == GetFileNameForIndex(freeSaveIndex))
                    {
                        free = false;
                        freeSaveIndex++;
                        continue;
                    }
                }

                if (free)
                    break;
            }

            return GetFileNameForIndex(freeSaveIndex);
        }
    }
}